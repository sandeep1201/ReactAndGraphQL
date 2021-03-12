using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Autofac;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface.Repository;
using DCF.Common.Dates;
using DCF.Common.Extensions;
using DCF.Common.Tasks;
using DCF.Timelimits.Rules.Domain;
using DCF.Timelimits.Tasks;
using DCF.Timelimts.Service;
using EnumsNET;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Serilog.Context;

namespace DCF.Timelimits
{
    /// <summary>
    /// Process Placement Closures and snapshots for every generated Tick. Sync Data To DB2
    /// </summary>
    public class ProcessBatchResultsApplication : BatchApplication<Decimal, ProcessTimelimitEvaluationContext, ProcessTimelimitEvaluationResult>
    {
        public ProcessBatchResultsApplication(ApplicationContext context) : base(context)
        {

        }

        public override void CreateProcessingQueue()
        {
            this._queue = new ActionBlock<ProcessTimelimitEvaluationContext>(async queueItem =>
            {
                await this.ProcessItemAsync(queueItem).ConfigureAwait(false);
            }, new ExecutionDataflowBlockOptions { CancellationToken = this._token, MaxDegreeOfParallelism = this.Context.MaxDegreeOfParallelism, SingleProducerConstrained = true, BoundedCapacity = 3 });
        }

        internal async Task<List<Decimal>> GetPinsToProcess()
        {
            List<Decimal> pinsToProcess = new List<Decimal>();
            var allpins = new List<Decimal>();
            var distinctPins = new List<Decimal>();
            if (!this.Context.inputPins.Any())
            {
                using (var timelimitService = this.Container.Resolve<ITimelimitService>())
                {
                    this._logger.Information("No Pins provided to process. Searching Database for pins");
                    var tlPins = await timelimitService.GetBatchEvaluatedPins(this.Context.Date, this.Context.JobQueuePartion, _token).ConfigureAwait(false);
                    var extPins = await timelimitService.GetExtensionPinsToProcessAsync(this.Context.Date, this.Context.JobQueuePartion, _token).ConfigureAwait(false);
                    allpins.AddRange(tlPins);
                    allpins.AddRange(extPins);
                    distinctPins = allpins.Distinct().ToList();
                    pinsToProcess.AddRange(distinctPins);
                    this._logger.Information($"Found [{distinctPins.Count}] pins to process:");
                }
            }
            else
            {
                this._logger.Information($"Processing [{this.Context.inputPins.Count}] provided pin(s):");
                pinsToProcess.AddRange(this.Context.inputPins);
            }

            return pinsToProcess;
        }

        internal override async Task StartProducerQueue()
        {
            try
            {

                this._logger.Information("Starting Producer queue: Generating items to process...");

                var pinsToProcess = await this.GetPinsToProcess().ConfigureAwait(false);

                //var pins = await db.TimeLimits.Where(x => !x.IsDeleted && x.EffectiveMonth != null && x.EffectiveMonth.Value.Year == this.Context.Date.Year && x.EffectiveMonth.Value.Month == this.Context.Date.Month).Select(x => x.Participant.PinNumber.GetValueOrDefault()).Distinct().ToListAsync(this._token).ConfigureAwait(false);
                //var timelimitIds = await db.TimeLimits.Where(x => !x.IsDeleted && x.EffectiveMonth != null && x.EffectiveMonth.Value.Year == this.Context.Date.Year && x.EffectiveMonth.Value.Month == this.Context.Date.Month).Select(x => x.Id).Distinct().ToListAsync(this._token).ConfigureAwait(false);
                foreach (var pin in pinsToProcess)
                {
                    using (LogContext.PushProperty("Pin", pin.ToString()))
                    {
                        await this.GetQueueItemAsync(pin).ContinueWith(async (x) =>
                        {
                            if (x.Result.Status == JobStatus.ReadyForJobProcessing)
                            {
                                await this.UpdateJobStatusAsync(x.Result, JobStatus.QueuedForJobProcessing);

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
                this._logger.Information($"Stopping Producer queue: All processing Items generated. Generated [{pinsToProcess.Count}] items to process. ");
                this._queue.Complete();
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
            var file_path1 = Path.Combine(this.Context.OutputPath, $"process_results_db2_{this.Context.JobQueuePartion}_{this.Context.Date:MMMM_yyyy}_{Guid.NewGuid().ToString("N").Substring(0, 10)}.xlsx");
            var file_path2 = Path.Combine(this.Context.OutputPath, $"process_results_placement_closure_{this.Context.JobQueuePartion}_{this.Context.Date:MMMM_yyyy}_{Guid.NewGuid().ToString("N").Substring(0, 10)}.xlsx");
            var outputPackage1 = new ExcelPackage();
            var outputPackage2 = new ExcelPackage();

            var placementClosuresSheet = outputPackage2.Workbook.Worksheets.Add("Placement Closures");
            #region create workbook

            placementClosuresSheet.Row(1).Style.Fill.PatternType = ExcelFillStyle.Solid;
            placementClosuresSheet.Row(1).Style.Fill.BackgroundColor.SetColor(Color.FromArgb(13, 215, 245));
            placementClosuresSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            placementClosuresSheet.Row(1).Style.Font.Bold = true;

            placementClosuresSheet.Cells[1, 1].Value = "Pin Number";
            placementClosuresSheet.Cells[1, 2].Value = "Timelimit Type";
            placementClosuresSheet.Cells[1, 3].Value = "Used";
            placementClosuresSheet.Cells[1, 4].Value = "Max";
            placementClosuresSheet.Cells[1, 5].Value = "Remaining";
            placementClosuresSheet.Cells[1, 6].Value = "Clock Max";
            placementClosuresSheet.Cells[1, 7].Value = "OP State Remaining";
            placementClosuresSheet.Cells[1, 8].Value = "Switched";
            placementClosuresSheet.Cells[1, 9].Value = "Primary Out Of Ticks";
            placementClosuresSheet.Cells[1, 10].Value = "Primary Out Of State Ticks";
            placementClosuresSheet.Cells[1, 11].Value = "Secondary Out Of State Ticks";
            placementClosuresSheet.Cells[1, 12].Value = "Should Close?";
            placementClosuresSheet.Cells[1, 13].Value = "Closed";
            placementClosuresSheet.Cells[1, 14].Value = "Placement Is Open";
            placementClosuresSheet.Cells[1, 15].Value = "Placement Type";
            placementClosuresSheet.Cells[1, 16].Value = "Error";
            placementClosuresSheet.Cells[1, 17].Value = "@CaseNumber";
            placementClosuresSheet.Cells[1, 18].Value = "@DatabaseDate";
            placementClosuresSheet.Cells[1, 19].Value = "@InputUserId";
            placementClosuresSheet.Cells[1, 20].Value = "@ExistingEpisodeBeginDate";
            placementClosuresSheet.Cells[1, 21].Value = "@PinNumber";
            placementClosuresSheet.Cells[1, 22].Value = "@ExistingFepId";
            placementClosuresSheet.Cells[1, 23].Value = "@ExistingEpisodeEndDate";
            placementClosuresSheet.Cells[1, 24].Value = "@ExistingPlacementCode";
            placementClosuresSheet.Cells[1, 25].Value = "@ExistingPlacementBeginDate";
            placementClosuresSheet.Cells[1, 26].Value = "@NewPinNumber";
            placementClosuresSheet.Cells[1, 27].Value = "@NewFepIdNumber";
            placementClosuresSheet.Cells[1, 28].Value = "@NewPlacementCode";

            #endregion

            var rowNum = 2;
            foreach (var kvp in this.appOutput)
            {
                //#region Db2 Worksheet
                //var db2Worksheet = outputPackage1.Workbook.Worksheets.Add(kvp.Key.ToString().PadLeft(10, '0'));
                //// Write Headers in output package 1
                //db2Worksheet.Cells[1, 1].Value = "[PIN_NUM]";
                //db2Worksheet.Cells[1, 2].Value = "[BENEFIT_MM]";
                //db2Worksheet.Cells[1, 3].Value = "[HISTORY_SEQ_NUM]";
                //db2Worksheet.Cells[1, 4].Value = "[CLOCK_TYPE_CD]";
                //db2Worksheet.Cells[1, 5].Value = "[CRE_TRAN_CD]";
                //db2Worksheet.Cells[1, 6].Value = "[FED_CLOCK_IND]";
                //db2Worksheet.Cells[1, 7].Value = "[FED_CMP_MTH_NUM]";
                //db2Worksheet.Cells[1, 8].Value = "[FED_MAX_MTH_NUM]";
                //db2Worksheet.Cells[1, 9].Value = "[HISTORY_CD]";
                //db2Worksheet.Cells[1, 10].Value = "[OT_CMP_MTH_NUM]";
                //db2Worksheet.Cells[1, 11].Value = "[OVERRIDE_REASON_CD]";
                //db2Worksheet.Cells[1, 12].Value = "[TOT_CMP_MTH_NUM]";
                //db2Worksheet.Cells[1, 13].Value = "[TOT_MAX_MTH_NUM]";
                //db2Worksheet.Cells[1, 14].Value = "[UPDATED_DT]";
                //db2Worksheet.Cells[1, 15].Value = "[USER_ID]";
                //db2Worksheet.Cells[1, 16].Value = "[WW_CMP_MTH_NUM]";
                //db2Worksheet.Cells[1, 17].Value = "[WW_MAX_MTH_NUM]";
                //db2Worksheet.Cells[1, 18].Value = "[COMMENT_TXT]";

                //var list = kvp.Value.CorrectedTicks;
                //var sortedList = list?.Any() == true ? list.OrderByDescending(x => x.BENEFIT_MM).ThenByDescending(x => x.HISTORY_SEQ_NUM).ToList() : list;
                //var row = 1;
                //foreach (var tick in sortedList)
                //{
                //    row++;
                //    db2Worksheet.Cells[row, 1].Value = tick.PIN_NUM;
                //    db2Worksheet.Cells[row, 1].Value = tick.PIN_NUM;
                //    db2Worksheet.Cells[row, 2].Value = tick.BENEFIT_MM;
                //    db2Worksheet.Cells[row, 3].Value = tick.HISTORY_SEQ_NUM;
                //    db2Worksheet.Cells[row, 4].Value = tick.CLOCK_TYPE_CD;
                //    db2Worksheet.Cells[row, 5].Value = tick.CRE_TRAN_CD;
                //    db2Worksheet.Cells[row, 6].Value = tick.FED_CLOCK_IND;
                //    db2Worksheet.Cells[row, 7].Value = tick.FED_CMP_MTH_NUM;
                //    db2Worksheet.Cells[row, 8].Value = tick.FED_MAX_MTH_NUM;
                //    db2Worksheet.Cells[row, 9].Value = tick.HISTORY_CD;
                //    db2Worksheet.Cells[row, 10].Value = tick.OT_CMP_MTH_NUM;
                //    db2Worksheet.Cells[row, 11].Value = tick.OVERRIDE_REASON_CD;
                //    db2Worksheet.Cells[row, 12].Value = tick.TOT_CMP_MTH_NUM;
                //    db2Worksheet.Cells[row, 13].Value = tick.TOT_MAX_MTH_NUM;
                //    db2Worksheet.Cells[row, 14].Style.Numberformat.Format = "yyyy-MM-dd";
                //    db2Worksheet.Cells[row, 14].Formula = $"=DATE({tick.UPDATED_DT.Year},{tick.UPDATED_DT.Month},{tick.UPDATED_DT.Day})";
                //    db2Worksheet.Cells[row, 15].Value = tick.USER_ID;
                //    db2Worksheet.Cells[row, 16].Value = tick.WW_CMP_MTH_NUM;
                //    db2Worksheet.Cells[row, 17].Value = tick.WW_MAX_MTH_NUM;
                //    db2Worksheet.Cells[row, 18].Value = tick.COMMENT_TXT;
                //}

                //db2Worksheet.Cells.AutoFitColumns();

                //#endregion

                #region Placement Closures
                placementClosuresSheet.Cells[rowNum, 1].Value = kvp.Key;
                var results = kvp.Value;
                if (results != null)
                {
                    placementClosuresSheet.Cells[rowNum, 2].Value = (results.t0459Record?.CLOCK_TYPE_CD ?? "-");
                    placementClosuresSheet.Cells[rowNum, 3].Value = results.ClockUsed?.ToString() ?? "-"; //timeline.GetUsedMonths(lastPlacement.PlacementType.GetValueOrDefault());
                    placementClosuresSheet.Cells[rowNum, 4].Value = results.ClockMax?.ToString() ?? "-"; //timeline.GetMaxMonths(lastPlacement.PlacementType.GetValueOrDefault());
                    placementClosuresSheet.Cells[rowNum, 5].Value = results.ClockRemaining?.ToString() ?? "-"; //timeline.GetRemainingMonths(lastPlacement.PlacementType.GetValueOrDefault());
                    placementClosuresSheet.Cells[rowNum, 6].Value = results.ClockLimit?.ToString() ?? "-"; //lastPlacement.PlacementType?.ToString();
                    placementClosuresSheet.Cells[rowNum, 7].Value = results.OtherStateParentRemaining?.ToString() ?? "-";
                    placementClosuresSheet.Cells[rowNum, 8].Value = "-"; //switched;
                    placementClosuresSheet.Cells[rowNum, 9].Value = results.PrimaryOutOfClockTicks?.ToString() ?? "-"; //;
                    placementClosuresSheet.Cells[rowNum, 10].Value = results.PrimaryOutOfStateTicks?.ToString() ?? "-";
                    ;
                    placementClosuresSheet.Cells[rowNum, 11].Value = results.SecondaryOutOfStateTicks?.ToString() ?? "-";
                    ;
                    placementClosuresSheet.Cells[rowNum, 12].Value = results.ShouldClose;
                    placementClosuresSheet.Cells[rowNum, 13].Value = results.ClosedSuccesfully?.ToString() ?? "-";
                    placementClosuresSheet.Cells[rowNum, 14].Value = results.PlacementIsOpen?.ToString() ?? "-"; //lastPlacement.IsOpen;
                    placementClosuresSheet.Cells[rowNum, 15].Value = FlagEnums.FormatFlags(results.LastPlacement?.PlacementType.GetValueOrDefault() ?? ClockTypes.None);

                    placementClosuresSheet.Cells[rowNum, 17].Value = results.CaseNumber?.ToString() ?? "-";
                    placementClosuresSheet.Cells[rowNum, 18].Value = this.Context.Date;
                    placementClosuresSheet.Cells[rowNum, 19].Value = "WWP";
                    placementClosuresSheet.Cells[rowNum, 20].Value = results.ExistingEpisodeBeginDate?.ToString("d") ?? "-";
                    placementClosuresSheet.Cells[rowNum, 21].Value = results.PinNumber;
                    placementClosuresSheet.Cells[rowNum, 22].Value = results.ExistingFepId;
                    placementClosuresSheet.Cells[rowNum, 23].Value = results.ExistingEpisodeEndDate?.ToString("d") ?? "-";
                    placementClosuresSheet.Cells[rowNum, 24].Value = results.PlacementType ?? "-";
                    placementClosuresSheet.Cells[rowNum, 25].Value = results.ExistingPlacementBeginDate?.ToString("d") ?? "-";
                    placementClosuresSheet.Cells[rowNum, 26].Value = results.ExistingFepId;
                    placementClosuresSheet.Cells[rowNum, 27].Value = this.Context.Date.ToString("d");
                    placementClosuresSheet.Cells[rowNum, 28].Value = results.PlacementType ?? "-";
                    ;

                    if (results.Errors != null)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (var error in results.Errors)
                        {
                            sb.AppendLine(error.Message);
                            sb.AppendLine(error.StackTrace);
                        }

                        placementClosuresSheet.Cells[rowNum, 16].Value = sb.ToString();
                    }
                }

                #endregion
                rowNum++;
            }

            //if (outputPackage1.Workbook.Worksheets.Any())
            //    outputPackage1.SaveAs(new FileInfo(file_path1));
            if (outputPackage2.Workbook.Worksheets.Any())
                outputPackage2.SaveAs(new FileInfo(file_path2));
        }

        internal override async Task<ProcessTimelimitEvaluationContext> GetQueueItemAsync(Decimal id)
        {
            var batchTask = new ProcessTimelimitEvaluationContext() { ExternalJobId = id.ToString(), Result = new ProcessTimelimitEvaluationResult() };
            try
            {
                await this.GetQueueItemAsyncThrottle.WaitAsync(this._token).ConfigureAwait(false);
                await this.CreateJobAsync(batchTask).ConfigureAwait(false);
                await this.UpdateJobStatusAsync(batchTask, JobStatus.CreatingJobForProcessing).ConfigureAwait(false);

                using (var timelimitService = this.Container.Resolve<ITimelimitService>())
                using (var db = this.Container.Resolve<WwpEntities>())
                {
                    //batchTask.CreatedMonth = await timelimitService.(id).ConfigureAwait(false);
                    batchTask.Participant = await timelimitService.GetParticipantAsync(id, this._token).ConfigureAwait(false);
                    batchTask.PinNumber = batchTask.Participant.PinNumber.GetValueOrDefault();
                    batchTask.Timeline = await timelimitService.GetTimelineAsync(batchTask.PinNumber, this._token).ConfigureAwait(false);
                    batchTask.CreatedMonth = await timelimitService.TimeLimitByDateAsync(id, this.Context.Date, this._token).ConfigureAwait(false);
                    batchTask.PlacementData = await db.SpTimelimitPlacementSummaryAsync(batchTask.PinNumber.ToString(), this._token).ConfigureAwait(false);
                    batchTask.OtherAssistanceGroupMembers = await timelimitService.GetOtherAGMembersAsync(batchTask.PinNumber, this.Context.Date.StartOf(DateTimeUnit.Month), this.Context.Date.EndOf(DateTimeUnit.Month), this._token).ConfigureAwait(false);
                    batchTask.Status = JobStatus.ReadyForJobProcessing;
                    await this.UpdateJobStatusAsync(batchTask, batchTask.Status.GetValueOrDefault());

                }
            }
            catch (Exception e)
            {
                this._logger.Error(e, "Error creating Batch Task:");
                batchTask.Status = JobStatus.JobProcessingFailure;
                this.FailedProccessingJobsSinceStart++;
                batchTask.Result.Errors.Add(e);
                //throw;
            }
            finally
            {
                this.GetQueueItemAsyncThrottle.Release();

            }

            return batchTask;
        }

        protected override void LogJobAsProccessed(ProcessTimelimitEvaluationContext queuedItem, ProcessTimelimitEvaluationResult results, TimeSpan stopWatchElapsed)
        {
            base.LogJobAsProccessed(queuedItem, results, stopWatchElapsed);
            this.appOutput.AddOrUpdate(queuedItem.PinNumber, results, (k, v) => results);
        }
    }
}