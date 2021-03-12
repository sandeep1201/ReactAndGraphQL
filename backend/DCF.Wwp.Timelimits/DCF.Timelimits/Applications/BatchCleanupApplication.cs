using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Autofac;
using Dcf.Wwp.Api.Library.Services;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;
using DCF.Common.Dates;
using DCF.Common.Exceptions;
using DCF.Common.Extensions;
using DCF.Common.Tasks;
using DCF.Timelimits.Rules.Domain;
using DCF.Timelimits.Tasks;
using DCF.Timelimts.Service;
using EnumsNET;
using OfficeOpenXml;
using Serilog.Context;

namespace DCF.Timelimits
{
    public class BatchCleanupApplication : BatchApplication<Decimal, BatchCleanupContext, BatchCleanupResult>
    {
        public BatchCleanupApplication(ApplicationContext context) : base(context)
        {
        }

        public override void CreateProcessingQueue()
        {
            this._queue = new ActionBlock<BatchCleanupContext>(async queueItem =>
            {
                await this.ProcessItemAsync(queueItem).ConfigureAwait(false);
            }, new ExecutionDataflowBlockOptions { CancellationToken = this._token, MaxDegreeOfParallelism = this.Context.MaxDegreeOfParallelism, SingleProducerConstrained = true, BoundedCapacity = 3 });
        }

        public async Task<List<Decimal>> GetPinsToProcessAsync()
        {
            using (var db = this.Container.Resolve<WwpEntities>())
            {
                var endMonth = this.Context.Date.StartOf(DateTimeUnit.Month);
                var alreadySyncedPinsQuery = db.TimeLimits.Where(x => x.EffectiveMonth == endMonth && !x.IsDeleted && x.ModifiedDate == null);

                var pinsToProcess = await db.TimeLimitExtensions.Where(x => !x.IsDeleted && x.EndMonth == endMonth
                                                                         && x.DecisionDate == (db.TimeLimitExtensions.Where(y => y.ParticipantId == x.ParticipantId && y.ExtensionSequence == x.ExtensionSequence && y.TimeLimitTypeId == x.TimeLimitTypeId).Max(y => y.DecisionDate))).Select(x => x.Participant.PinNumber).Distinct().ToListAsync(this._token).ConfigureAwait(false);
                this._logger.Information($"Found ({pinsToProcess.Count}) records to process.");
                return pinsToProcess.Select(x => x.GetValueOrDefault()).ToList();
            }
        }

        internal override async Task StartProducerQueue()
        {
            try
            {
                var pins = await this.GetPinsToProcessAsync().ConfigureAwait(false);
                //var tasks = new List<Task<BatchCleanupContext>>();
                this._logger.Information("Starting Producer queue: Generating items to process...");
                foreach (var pin in pins)
                {
                    if (pin < 1)
                    {
                        continue;
                    }

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

                    //tasks.Add(queueItemTask);
                }

                //await Task.WhenAll(tasks).ConfigureAwait(false);
                this._logger.Information($"Stopping Producer queue: All processing Items generated. Generated [{pins.Count}] items to process. ");
                this._queue.Complete();
            }
            catch (Exception e)
            {
                this._logger.Error(e, $"Stopping Producer queue: Error generating Items. ");
                this._queue?.Complete();

                throw;
            }

        }

        protected override void LogJobAsProccessed(BatchCleanupContext queuedItem, BatchCleanupResult results, TimeSpan stopWatchElapsed)
        {
            base.LogJobAsProccessed(queuedItem, results, stopWatchElapsed);
            this.appOutput.TryAdd(queuedItem.PinNumber, results);
        }

        internal override void WriteJobOutput()
        {
            var file_path = Path.Combine(this.Context.OutputPath, $"batch_cleanup_{this.Context.JobQueuePartion}_{this.Context.Date:MMMM_yyyy)}_{Guid.NewGuid().ToString("N").Substring(0, 10)}.xlsx");

            Action<Decimal, List<IT0459_IN_W2_LIMITS>, ExcelPackage> writeOutputAction = (key, ticks, outputPackage) =>
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

                    var list = ticks;
                    var row = 1;

                    var sortedList = list.OrderByDescending(x => x.BENEFIT_MM).ThenByDescending(x => x.HISTORY_SEQ_NUM);
                    foreach (var tick in sortedList)
                    {
                        row++;
                        worksheet.Cells[row, 1].Value = tick.PIN_NUM;
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
                        worksheet.Cells[row, 14].Formula = $"=DATE({tick.UPDATED_DT.Year},{tick.UPDATED_DT.Month},{tick.UPDATED_DT.Day})";
                        worksheet.Cells[row, 15].Value = tick.USER_ID;
                        worksheet.Cells[row, 16].Value = tick.WW_CMP_MTH_NUM;
                        worksheet.Cells[row, 17].Value = tick.WW_MAX_MTH_NUM;
                        worksheet.Cells[row, 18].Value = tick.COMMENT_TXT;
                    }

                    worksheet.Cells.AutoFitColumns();
                };
            ExcelPackage outputPackage1 = new ExcelPackage();

            try
            {
                foreach (var results in this.appOutput)
                {

                    var allTicks = new List<IT0459_IN_W2_LIMITS>();
                    allTicks.AddRange(results.Value.NewTicks);
                    allTicks.AddRange(results.Value.OldTicks);
                    writeOutputAction(results.Key, allTicks.Distinct().ToList(), outputPackage1);
                }
                var file = new FileInfo(file_path);
                if (outputPackage1.Workbook.Worksheets.Any())
                    outputPackage1.SaveAs(file);
            }
            catch (Exception e)
            {
                this._logger.Warning(e, $"Error writing output file \"${file_path}\" for {nameof(EvaluateTimelimitsBatchApplication)}.");
            }

        }

        internal override async Task<BatchCleanupContext> GetQueueItemAsync(Decimal id)
        {
            var batchTask = new BatchCleanupContext { PinNumber = id, ExternalJobId = id.ToString(), Result = new BatchCleanupResult() };
            try
            {
                await this.GetQueueItemAsyncThrottle.WaitAsync().ConfigureAwait(false);

                await this.CreateJobAsync(batchTask).ConfigureAwait(false);
                await this.UpdateJobStatusAsync(batchTask, JobStatus.CreatingJobForProcessing).ConfigureAwait(false);

                using (var _repo = this.Container.Resolve<IRepository>())
                using (var _timelimitService = this.Container.Resolve<ITimelimitService>())
                {
                    var ticksToUpdate = _repo.GetLatestW2LimitsMonthsForEachClockType(id);
                    var timeline = await _timelimitService.GetTimelineAsync(id, this._token).ConfigureAwait(false);
                    batchTask.TicksToUpdate = ticksToUpdate;
                    batchTask.Timeline = timeline;
                    batchTask.Status = JobStatus.ReadyForJobProcessing;
                }
            }
            catch (Exception e)
            {
                this._logger.Error(e, "Error creating Batch Task:");
                batchTask.Status = JobStatus.JobProcessingFailure;
                batchTask.Result.Errors.Add(e);
                this.FailedProccessingJobsSinceStart++;

                //throw;
            }
            finally
            {
                this.GetQueueItemAsyncThrottle.Release();
            }


            return batchTask;
        }
    }
}