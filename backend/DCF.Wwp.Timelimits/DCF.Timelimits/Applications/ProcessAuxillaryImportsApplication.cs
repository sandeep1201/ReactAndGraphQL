using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Autofac;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;
using DCF.Common.Dates;
using DCF.Common.Extensions;
using DCF.Common.Tasks;
using DCF.Timelimits.Rules.Domain;
using DCF.Timelimits.Tasks;
using DCF.Timelimts.Service;
using OfficeOpenXml;
using Serilog.Context;

namespace DCF.Timelimits
{
    public class ProcessAuxillaryImportsApplication : BatchApplication<Decimal, ProcessAuxillaryContext, ProcessAuxillaryResult>
    {
        private Lazy<IChangeReason> _auxChangeReason;
        public ProcessAuxillaryImportsApplication(ApplicationContext context) : base(context)
        {

            base.OnInitialized += this.OnInitialized;

        }

        private Task OnInitialized(IContainer container)
        {
            this._auxChangeReason = new Lazy<IChangeReason>(() =>
            {
                using (var _repo = Container.Resolve<IRepository>())
                {
                    var changeReasons = _repo.ChangeReasons().Where(x => x.Name.ToLower().Contains("auxiliary"));
                    return changeReasons.FirstOrDefault();
                }
            });
            return Task.CompletedTask;
        }


        public override void CreateProcessingQueue()
        {
            this._queue = new ActionBlock<ProcessAuxillaryContext>(async queueItem =>
            {
                await this.ProcessItemAsync(queueItem).ConfigureAwait(false);
            }, new ExecutionDataflowBlockOptions { CancellationToken = this._token, MaxDegreeOfParallelism = this.Context.MaxDegreeOfParallelism, SingleProducerConstrained = true, BoundedCapacity = 3 });
        }

        internal override async Task StartProducerQueue()
        {
            try
            {
                using (var _db = this.Container.Resolve<WwpEntities>())
                {
                    var auxPins = await _db.AuxiliaryPayments.Where(x => x.PIN_NUM > 0 && x.EffectiveMonth == null).Select(x => x.PIN_NUM).Distinct().ToListAsync(this._token).ConfigureAwait(false);
                    var pins = auxPins.Select(x => x.GetValueOrDefault()).ToList();
                    foreach (var pin in pins)
                    {
                        using (LogContext.PushProperty("Pin", pin.ToString()))
                        {
                            await this.GetQueueItemAsync(pin).ContinueWith(async (x) =>
                            {
                                if (x.Result.Status == JobStatus.ReadyForJobProcessing)
                                {
                                    this.UpdateJobStatusAsync(x.Result, JobStatus.QueuedForJobProcessing);

                                    await this._queue.SendAsync(x.Result); //send item to the queue, will wait to items fall out of the queue(this.Context.MaxDegreeOfParallelism)
                                    this._logger.Information($"Adding item to processing queue with ID: {x.Result.GetItemIdentifier()}");
                                }
                                else
                                {
                                    this.UpdateJobStatusAsync(x.Result, x.Result.Status.GetValueOrDefault());
                                    this._logger.Information($"Skipped processing item with ID (marked as {x.Result.Status.GetValueOrDefault()}): {x.Result.GetItemIdentifier()}");
                                }

                                return x.Result;
                            }, TaskContinuationOptions.OnlyOnRanToCompletion).Unwrap().ConfigureAwait(false);

                        }
                    }

                    this._logger.Information($"Stopping Producer queue: All processing Items generated. Generated [{pins.Count}] items to process. ");
                    this._queue.Complete();
                }
            }
            catch (Exception e)
            {
                this._logger.Error(e, $"Stopping Producer queue: Error generating Items. ");
                this._queue?.Complete();

                throw;
            }
        }

        internal override void WriteJobOutput()
        {
            var file_path = Path.Combine(this.Context.OutputPath, $"aux_import_{this.Context.JobQueuePartion}_{this.Context.Date:MMMM_yyyy)}_{Guid.NewGuid().ToString("N").Substring(0, 10)}.xlsx");
            ExcelPackage outputPackage = new ExcelPackage();
            Action<Decimal, ProcessAuxillaryResult> writeOutputAction = (key, payments) =>
                {
                    var worksheet = outputPackage.Workbook.Worksheets.Add(key.ToString().PadLeft(10, '0'));

                    // Write header
                    worksheet.Cells[1, 1].Value = "[PIN_NUM]";
                    worksheet.Cells[1, 2].Value = "[BENEFIT_MM]";
                    worksheet.Cells[1, 3].Value = "[HISTORY_SEQ_NUM]";
                    worksheet.Cells[1, 4].Value = "[CLOCK_TYPE_CD]";
                    worksheet.Cells[1, 5].Value = "[CRE_TRAN_CD]";
                    worksheet.Cells[1, 6].Value = "[FED_CLOCK_IND]";
                    worksheet.Cells[1, 7].Value = "[FED_CMP_MTH_NUM]";
                    worksheet.Cells[1, 8].Value = "[FED_MAX_MTH_NUM]";
                    worksheet.Cells[1, 9].Value = "[HISTORY_CD]";
                    worksheet.Cells[1, 10].Value = "[OT_CMP_MTH_NUM]";
                    worksheet.Cells[1, 11].Value = "[OVERRIDE_REASON_CD]";
                    worksheet.Cells[1, 12].Value = "[TOT_CMP_MTH_NUM]";
                    worksheet.Cells[1, 13].Value = "[TOT_MAX_MTH_NUM]";
                    worksheet.Cells[1, 14].Value = "[UPDATED_DT]";
                    worksheet.Cells[1, 15].Value = "[USER_ID]";
                    worksheet.Cells[1, 16].Value = "[WW_CMP_MTH_NUM]";
                    worksheet.Cells[1, 17].Value = "[WW_MAX_MTH_NUM]";
                    worksheet.Cells[1, 18].Value = "[COMMENT_TXT]";

                    var list = payments.ProcessedPayments;
                    var row = 1;

                    var sortedList = list.OrderByDescending(x => x.BENEFIT_MM).ThenByDescending(x => x.HISTORY_SEQ_NUM);
                    foreach (var tick in sortedList)
                    {
                        row++;
                        worksheet.Cells[row, 1].Value = tick.PIN_NUM;
                        worksheet.Cells[row, 2].Value = tick.BENEFIT_MM;
                        worksheet.Cells[row, 3].Value = tick.HISTORY_SEQ_NUM;
                        worksheet.Cells[row, 4].Value = tick.CLOCK_TYPE_CD;
                        worksheet.Cells[row, 5].Value = tick.CRE_TRAN_CD;
                        worksheet.Cells[row, 6].Value = tick.FED_CLOCK_IND;
                        worksheet.Cells[row, 7].Value = tick.FED_CMP_MTH_NUM;
                        worksheet.Cells[row, 8].Value = tick.FED_MAX_MTH_NUM;
                        worksheet.Cells[row, 9].Value = tick.HISTORY_CD;
                        worksheet.Cells[row, 10].Value = tick.OT_CMP_MTH_NUM;
                        worksheet.Cells[row, 11].Value = tick.OVERRIDE_REASON_CD;
                        worksheet.Cells[row, 12].Value = tick.TOT_CMP_MTH_NUM;
                        worksheet.Cells[row, 13].Value = tick.TOT_MAX_MTH_NUM;
                        worksheet.Cells[row, 14].Style.Numberformat.Format = "yyyy-MM-dd";
                        worksheet.Cells[row, 14].Formula = $"=DATE({tick.UPDATED_DT.GetValueOrDefault().Year},{tick.UPDATED_DT.GetValueOrDefault().Month},{tick.UPDATED_DT.GetValueOrDefault().Day})";
                        worksheet.Cells[row, 15].Value = tick.USER_ID;
                        worksheet.Cells[row, 16].Value = tick.WW_CMP_MTH_NUM;
                        worksheet.Cells[row, 17].Value = tick.WW_MAX_MTH_NUM;
                        worksheet.Cells[row, 18].Value = tick.COMMENT_TXT;
                    }

                    worksheet.Cells.AutoFitColumns();
                };

            try
            {
                foreach (var results in this.appOutput)
                {

                    writeOutputAction(results.Key, results.Value);
                }
                var file = new FileInfo(file_path);
                if (outputPackage.Workbook.Worksheets.Any())
                    outputPackage.SaveAs(file);
            }
            catch (Exception e)
            {
                this._logger.Warning(e, $"Error writing output file \"${file_path}\" for {nameof(ProcessAuxillaryImportsApplication)}.");
            }
        }

        internal override async Task<ProcessAuxillaryContext> GetQueueItemAsync(Decimal id)
        {
            var batchTask = new ProcessAuxillaryContext() { ExternalJobId = id.ToString(), Result = new ProcessAuxillaryResult() };

            try
            {
                using (var timelimitService = this.Container.Resolve<ITimelimitService>())
                {
                    await this.CreateJobAsync(batchTask).ConfigureAwait(false);
                    await this.UpdateJobStatusAsync(batchTask, JobStatus.CreatingJobForProcessing).ConfigureAwait(false);

                    batchTask.AuxChangeReason = this._auxChangeReason.Value;
                    batchTask.PinNumber = id;
                    if (this.Context.IsSimulation)
                    {
                        using (var db = this.Container.Resolve<WwpEntities>())
                        {
                            var startDate = this.Context.Date.StartOf(DateTimeUnit.Month);
                            batchTask.AuxiliaryPayments = await db.AuxiliaryPayments.Where(x => x.PIN_NUM == id && x.EffectiveMonth == null && x.UPDATED_DT >= startDate).ToListAsync(this._token).ConfigureAwait(false);
                        }
                    }
                    else
                    {
                        batchTask.AuxiliaryPayments = await timelimitService.GetAuxillaryPaymentsAsync(id, this._token).ConfigureAwait(false);
                    }

                    batchTask.Participant = await timelimitService.GetParticipantAsync(id, this._token).ConfigureAwait(false);
                    batchTask.Status = JobStatus.ReadyForJobProcessing;
                }
            }
            catch (Exception e)
            {
                this._logger.Error(e, $"Error create batch task Item");
                batchTask.Status = JobStatus.JobProcessingFailure;
                this.FailedProccessingJobsSinceStart++;
                batchTask.Result.Errors.Add(e);
            }

            return batchTask;
        }

        protected override void LogJobAsProccessed(ProcessAuxillaryContext queuedItem, ProcessAuxillaryResult results, TimeSpan stopWatchElapsed)
        {
            base.LogJobAsProccessed(queuedItem, results, stopWatchElapsed);
            this.appOutput.AddOrUpdate(queuedItem.PinNumber, results, (k, v) => results);
        }
    }
}