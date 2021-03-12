using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Transactions;
using Autofac;
using Dcf.Wwp.Api.Library.Services;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;
using DCF.Common.Dates;
using DCF.Common.Extensions;
using DCF.Common.Tasks;
using DCF.Core.IO;
using DCF.Timelimits.Core.Processors;
using DCF.Timelimits.Core.Tasks;
using DCF.Timelimits.Rules.Definitions;
using DCF.Timelimits.Rules.Domain;
using DCF.Timelimits.Rules.Scripting;
using DCF.Timelimits.Tasks;
using DCF.Timelimts.Service;
using EnumsNET;
using Nito.AsyncEx;
using NRules;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using OfficeOpenXml.Style;
using Serilog;
using Serilog.Context;
using IsolationLevel = System.Data.IsolationLevel;

namespace DCF.Timelimits
{
    public class TimelimitsBatchApplicationSimulator : TimelimitsBatchApplication
    {
        ConcurrentDictionary<ClockTypes, StringBuilder> taskOutput = new ConcurrentDictionary<ClockTypes, StringBuilder>();
        ConcurrentDictionary<Decimal, RuleContext> pinOutput = new ConcurrentDictionary<Decimal, RuleContext>();
        ConcurrentDictionary<ClockTypes, List<Decimal>> expectedPinOutput = new ConcurrentDictionary<ClockTypes, List<Decimal>>()
            ;


        public TimelimitsBatchApplicationSimulator(ApplicationContext context) : base(context)
        {
            context.JobQueueName = "Simulator";
            context.JobQueueType = JobQueueType.Simulator;

            base.OnInitialized = this.OnInitialized;
            base.OnContainerInitialized = this.OnContainerInitialized;
        }

        internal ISessionFactory CompileTimelimitRuleNetwork()
        {
            return RulesEngine.CompileTimelimitRuleNetwork();
        }

        private async Task OnInitialized(ContainerBuilder containerBuilder)
        {
            this.mediator = this.Container.Resolve<ITaskMediator>();
            //this._timelimitService = this.Container.Resolve<ITimelimitService>();
            //this.dbContextFactory = () => this.Container.Resolve<WwpEntities>();
            //this._db2TimelimitsServiceFactory = () => this.Container.Resolve<IDb2TimelimitService>();
            //this._repoFactory = () => this.Container.Resolve<IRepository>();

        }

        private void OnContainerInitialized(ContainerBuilder containerBuilder)
        {
            ISessionFactory ruleFactory = this.CompileTimelimitRuleNetwork();
            containerBuilder.RegisterInstance(ruleFactory).As<ISessionFactory>().SingleInstance();
        }

        public override async Task StartAsync()
        {
            this._token = this.tokenSource.Token;
            this._isRunning = true;
            this._isPaused = false;

            Boolean keepRunning = true;
            while (keepRunning)
            {
                Int32 taskId = 0;

                do
                {
                    Console.WriteLine(@"What would you like to do?
                                    --------------------------
                                    1. Fix DB2 Counts
                                    2. Import legacy batch ticks
                                    3. Run test batch
                                    4. Parse Expected Pin Output (uat_expected_batch_results)
                                    5. Created Expected Output from legacy batch run
                                    6. Parse/Process batch run output
                                    7. Write WWP.T0459 record(s) back to DB2 
                                    8. Process Placement Closure(s)
                                    9. Process All Batch Results
                                    10. Exit
                
                ");
                } while (!Int32.TryParse(Console.ReadLine(), out taskId));

                try
                {
                    switch (taskId)
                    {
                        case 1:
                            await this.FixDb2CountsAsync().ConfigureAwait(false);
                            break;
                        case 2:
                            await this.ImportDb2Ticks().ConfigureAwait(false);
                            break;
                        case 3:
                            await this.RunSimulatedBatchJob().ConfigureAwait(false);
                            break;
                        case 4:
                            this.ParseExpectedOutput("uat_expected_batch_results.xlsx", true);
                            break;
                        case 5:
                            await this.CreateExpectedOutputFromLegacyBatch(null).ConfigureAwait(false);
                            break;
                        case 6:
                            await this.ParseBatchOutput().ConfigureAwait(false);
                            break;
                        case 7:
                            await this.WriteTicksBackToDb2().ConfigureAwait(false);
                            break;
                        case 8:
                            await this.ProcessPlacementClosures().ConfigureAwait(false);
                            break;
                        case 9:
                            await this.ProcessAllBatchItemsSpreadsheet().ConfigureAwait(false);
                            break;
                        default:
                            Console.WriteLine("I dunno what you just asked for...");
                            break;
                    }
                }
                catch (Exception e)
                {
                    this._logger.Error(e, "Error running @task", taskId);
                }


                Console.WriteLine("Keep Running?(y/n):");
                keepRunning = Console.ReadLine()?.ToLower() != "n";
            }
        }

        private async Task ProcessAllBatchItemsSpreadsheet()
        {
            var defaultFilePath = "C:\\projects\\1. SOURCE CODE\\WEPASS\\all batch results\\all-batch-results.xlsx";
            Console.WriteLine($"File to process({defaultFilePath}):");
            string filePath;
            do
            {
                Console.WriteLine($"File to process({defaultFilePath}):");
                filePath = Console.ReadLine();
                if (filePath.IsNullOrWhiteSpace())
                {
                    filePath = defaultFilePath;
                }
            } while (!File.Exists(filePath));

            var package = new ExcelPackage();
            using (var file = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
            {
                package.Load(file);
            }

            var worksheets = package.Workbook.Worksheets.ToList();

            Console.WriteLine("Found Worksheets: " + String.Join(", ", worksheets.Select(x => x.Name)));

            using (var outputPackage = new ExcelPackage())
            {
                #region Find Missing Batch records and append
                string outputPath = this.GetOutputPath();

                foreach (var worksheet in worksheets)
                {
                    Console.WriteLine($"Pre-Processing worksheet: {worksheet.Name}...");
                    var contextDate = worksheet.Cells[1, 2].GetValue<DateTime>();
                    this.Context.Date = contextDate;
                    var dMonth = Decimal.Parse(this.Context.Date.ToString("yyyyMM"));


                    Dictionary<Decimal, T0459_IN_W2_LIMITS> legacyTicks;
                    using (var dbContext = this.Container.Resolve<WwpEntities>())
                    {
                        legacyTicks = await dbContext.T0459_IN_W2_LIMITS.Where(x => x.BENEFIT_MM == dMonth && x.CRE_TRAN_CD == "PWCAEP11"
                                                                                                           && x.HISTORY_SEQ_NUM == (dbContext.T0459_IN_W2_LIMITS.Where(y => x.BENEFIT_MM == y.BENEFIT_MM && x.PIN_NUM == y.PIN_NUM && y.CRE_TRAN_CD == "PWCAEP11")).Max(y => y.HISTORY_SEQ_NUM)
                        ).ToDictionaryAsync(x => x.PIN_NUM).ConfigureAwait(false);
                    }

                    Console.WriteLine("Looking for missing records from batch results...");
                    var missingBatchPins = new Dictionary<Decimal, Decimal>();

                    // Add missing records where the legacy batch ticked and the new batch didn't process the pin
                    var batchPins = new HashSet<Decimal>();
                    var row = 3;
                    string sPin;
                    do
                    {
                        sPin = worksheet.Cells[row, 1].GetValue<String>();
                        Decimal pin;
                        if (!sPin.IsNullOrWhiteSpace() && Decimal.TryParse(sPin, out pin) && !batchPins.Contains(pin))
                        {
                            batchPins.Add(pin);
                        }

                        row++;
                    } while (!sPin.IsNullOrWhiteSpace());

                    missingBatchPins = legacyTicks.Keys.Except(batchPins).ToDictionary(x => x);

                    if (missingBatchPins.Any())
                    {
                        this.CreateProcessingQueue();


                        await this.RunActionByPartitions(missingBatchPins.Keys.ToList(), async (pin) =>
                        {
                            try
                            {
                                var itemToProcess = await this.GetQueueItemAsync(pin).ConfigureAwait(false);
                                if (itemToProcess.Status.GetValueOrDefault() == JobStatus.ReadyForJobProcessing)
                                {
                                    await this.ProcessItem(itemToProcess).ConfigureAwait(false);
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"Failed to process pin: {pin}");
                                Console.WriteLine(e.Message);
                            }
                        });

                        Console.WriteLine($"Found ({missingBatchPins.Count}) missing pins. Adding them and calculating result");
                        foreach (var mPin in missingBatchPins.Keys.ToList())
                        {
                            row++;
                            RuleContext context;
                            var legacyTickFlags = FlagEnums.ParseFlags<ClockTypes>(legacyTicks[mPin].CLOCK_TYPE_CD).CombineFlags(ClockTypes.State);
                            legacyTickFlags = legacyTicks[mPin].FED_CLOCK_IND.Trim() == "Y" ? legacyTickFlags.CombineFlags(ClockTypes.Federal) : legacyTickFlags;
                            worksheet.Cells[row, 1].Value = mPin;
                            worksheet.Cells[row, 2].Value = FlagEnums.FormatFlags(legacyTickFlags);

                            ClockTypes clockTypesResultFlags = ClockTypes.None;

                            if (this.pinOutput.TryGetValue(mPin, out context))
                            {
                                clockTypesResultFlags = context.TimelimitType.GetValueOrDefault();
                                worksheet.Cells[row, 3].Value = FlagEnums.FormatFlags(clockTypesResultFlags);
                            }
                            else
                            {
                                worksheet.Cells[row, 3].Value = "None";
                            }

                            worksheet.Cells[row, 4].Value = (clockTypesResultFlags == legacyTickFlags) ? "P" : "F";
                        }

                        filePath = Path.Combine(outputPath, "all-batch-results.xlsx");

                        package.SaveAs(new FileInfo(filePath));
                        Console.WriteLine("Created corrected all-batch-results.xlsx");
                        Console.WriteLine("Using file: " + filePath);

                    }
                    else
                    {
                        Console.WriteLine("No Missing records found!");
                    }

                }
                #endregion

                #region Create outputPackage Header
                var outputRow = 2;
                var outputWorksheet = outputPackage.Workbook.Worksheets.Add("results");

                outputWorksheet.Row(outputRow).Style.Fill.PatternType = ExcelFillStyle.Solid;
                outputWorksheet.Row(outputRow).Style.Fill.BackgroundColor.SetColor(Color.FromArgb(13, 215, 245));
                outputWorksheet.Row(outputRow).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                outputWorksheet.Row(outputRow).Style.Font.Bold = true;

                outputWorksheet.Cells[1, 1].Value = "Benefit month";
                outputWorksheet.Cells[1, 2].Value = this.Context.Date;
                outputWorksheet.Cells[outputRow, 1].Value = "Pin Number";
                outputWorksheet.Cells[outputRow, 2].Value = "Current Timelimit(s)";
                outputWorksheet.Cells[outputRow, 3].Value = "New Timelimit(s)";
                outputWorksheet.Cells[outputRow, 4].Value = "Corrections Count";

                outputWorksheet.Cells[outputRow, 5].Value = "State will need Ext";
                outputWorksheet.Cells[outputRow, 6].Value = "CSJ  will needs Ext";
                outputWorksheet.Cells[outputRow, 7].Value = "W2T will needs Ext";
                outputWorksheet.Cells[outputRow, 8].Value = "TEMP will needs Ext";

                outputWorksheet.Cells[outputRow, 9].Value = "State Used";
                outputWorksheet.Cells[outputRow, 10].Value = "State Used Correction";
                outputWorksheet.Cells[outputRow, 11].Value = "State Remaining";
                outputWorksheet.Cells[outputRow, 12].Value = "State Remaining Correction";
                outputWorksheet.Cells[outputRow, 13].Value = "State Max";
                outputWorksheet.Cells[outputRow, 14].Value = "State Max Correction";

                outputWorksheet.Cells[outputRow, 15].Value = "CSJ Used";
                outputWorksheet.Cells[outputRow, 16].Value = "CSJ Used Correction";
                outputWorksheet.Cells[outputRow, 17].Value = "CSJ Remaining";
                outputWorksheet.Cells[outputRow, 18].Value = "CSJ Remaining Correction";
                outputWorksheet.Cells[outputRow, 19].Value = "CSJ Max";
                outputWorksheet.Cells[outputRow, 20].Value = "CSJ Max Correction";

                outputWorksheet.Cells[outputRow, 21].Value = "W-2 T Used";
                outputWorksheet.Cells[outputRow, 22].Value = "W-2 T Used Correction";
                outputWorksheet.Cells[outputRow, 23].Value = "W-2 T Remaining";
                outputWorksheet.Cells[outputRow, 24].Value = "W-2 T Remaining Correction";
                outputWorksheet.Cells[outputRow, 25].Value = "W-2 T Max";
                outputWorksheet.Cells[outputRow, 26].Value = "W-2 T Max Correction";

                outputWorksheet.Cells[outputRow, 27].Value = "TEMP Used";
                outputWorksheet.Cells[outputRow, 28].Value = "TEMP Used Correction";
                outputWorksheet.Cells[outputRow, 29].Value = "TEMP Remaining";
                outputWorksheet.Cells[outputRow, 30].Value = "TEMP Remaining Correction";
                outputWorksheet.Cells[outputRow, 31].Value = "TEMP Max";
                outputWorksheet.Cells[outputRow, 32].Value = "TEMP Max Correction";

                //TODO: put in processing details (time limits, corrections, etc)
                //outputWorksheet.Cells[outputRow, 5].Value = "Expected Discrepency";
                //outputWorksheet.Cells[outputRow, 6].Value = "Batch Notes / Results:";
                //outputWorksheet.Cells[outputRow, 7].Value = "Last Employment Position";
                //outputWorksheet.Cells[outputRow, 8].Value = "Placements";
                //outputWorksheet.Cells[outputRow, 9].Value = "Previous Placement";
                //outputWorksheet.Cells[outputRow, 10].Value = "First Non Cmc Employment Position";
                //outputWorksheet.Cells[outputRow, 11].Value = "Had Previous Paid Placement In Month";
                //outputWorksheet.Cells[outputRow, 12].Value = "Moved Directly Into Cmc";
                //outputWorksheet.Cells[outputRow, 13].Value = "Has Child Born 10Mmonths After Paid w2Start";
                //outputWorksheet.Cells[outputRow, 14].Value = "Cmc Should Tick Previous Placement";
                //outputWorksheet.Cells[outputRow, 15].Value = "Is Alien";
                //outputWorksheet.Cells[outputRow, 16].Value = "Payments Are Fully Sanctioned";
                //outputWorksheet.Cells[outputRow, 17].Value = "Payments (data)";
                //outputWorksheet.Cells[outputRow, 18].Value = "Placments (data)";
                //outputWorksheet.Cells[outputRow, 19].Value = "Alien Statues (data)";
                //outputWorksheet.Cells[outputRow, 20].Value = "Assistance Group (data)";

                #endregion

                var newTickDictionary = new ConcurrentDictionary<Decimal, List<TimelineMonth>>();

                foreach (var worksheet in worksheets)
                {
                    Console.WriteLine($"Aggregating failures - Processing worksheet: {worksheet.Name}...");
                    this.Context.Date = worksheet.Cells[1, 2].GetValue<DateTime>().StartOf(DateTimeUnit.Month);

                    var row = 2;
                    string sPin = "";
                    do
                    {
                        row++;
                        sPin = worksheet.Cells[row, 1].GetValue<String>();
                        var isFailure = worksheet.Cells[row, 4]?.GetValue<String>()?.ToUpper() == "F";
                        if (isFailure)
                        {
                            
                            Decimal pin = Decimal.Parse(sPin);
                            var newClocktype = FlagEnums.ParseFlags<ClockTypes>(worksheet.Cells[row, 3].GetValue<string>());
                            var timelineMonth = new TimelineMonth(this.Context.Date, newClocktype.RemoveFlags(ClockTypes.State | ClockTypes.Federal), newClocktype.HasAnyFlags(ClockTypes.Federal), newClocktype.HasAnyFlags(ClockTypes.Federal), newClocktype.HasAnyFlags(ClockTypes.PlacementTypes));
                            newTickDictionary.AddOrUpdate(pin, new List<TimelineMonth> { timelineMonth }, (k, l) =>
                            {
                                l.Add(timelineMonth);
                                return l;
                            });
                        }
                    } while (!sPin.IsNullOrWhiteSpace());
                }

                Console.Write($"Found {newTickDictionary.Count} with corrections. Calculating changes");
                StringBuilder sb = new StringBuilder();
                StringBuilder sb2 = new StringBuilder();

                var correctionsNeedsStateExtensionCount = 0;
                var correctionsNeedsCSJExtensionCount = 0;
                var correctionsNeedsW2TExtensionCount = 0;
                var correctionsNeedsTEMPExtensionCount = 0;

                foreach (var change in newTickDictionary)
                {
                    sb.Clear();
                    sb2.Clear();
                    outputRow++;
                    using (var timelimitsService = this.Container.Resolve<ITimelimitService>())
                    {
                        var pin = change.Key;
                        var timeline = await timelimitsService.GetTimelineAsync(pin, this._token).ConfigureAwait(false);

                        // Write Current value(s)
                        foreach (var newMonth in change.Value)
                        {
                            var month = timeline.Months[newMonth.Date];
                            if (month != null)
                            {
                                sb.AppendLine($"{month.Date:Y} - {FlagEnums.FormatFlags(month.ClockTypes)}");
                            }
                            sb2.AppendLine($"{newMonth.Date:Y} - {FlagEnums.FormatFlags(newMonth.ClockTypes)}");

                        }

                        var currentSnap = timelimitsService.CreateTimelimitSummary(timeline, 1);

                        timeline.AddTimelineMonths(change.Value);

                        var newSnap = timelimitsService.CreateTimelimitSummary(timeline, 1);


                        outputWorksheet.Cells[outputRow, 1].Value  = pin;
                        outputWorksheet.Cells[outputRow, 2].Value  = sb.ToString();
                        outputWorksheet.Cells[outputRow, 3].Value  = sb2.ToString();
                        outputWorksheet.Cells[outputRow, 4].Value  = change.Value.Count;

                        outputWorksheet.Cells[outputRow, 5].Value  = !currentSnap.StateExtensionDue.GetValueOrDefault() && newSnap.StateExtensionDue.GetValueOrDefault();
                        outputWorksheet.Cells[outputRow, 6].Value  = !currentSnap.CSJExtensionDue.GetValueOrDefault() && newSnap.CSJExtensionDue.GetValueOrDefault();
                        outputWorksheet.Cells[outputRow, 7].Value  = !currentSnap.W2TExtensionDue.GetValueOrDefault() && newSnap.W2TExtensionDue.GetValueOrDefault();
                        outputWorksheet.Cells[outputRow, 8].Value  = !currentSnap.TempExtensionDue.GetValueOrDefault() && newSnap.TempExtensionDue.GetValueOrDefault();

                        outputWorksheet.Cells[outputRow, 9].Value  = currentSnap.StateUsed;
                        outputWorksheet.Cells[outputRow, 10].Value = newSnap.StateUsed;
                        outputWorksheet.Cells[outputRow, 11].Value = currentSnap.StateMax - currentSnap.StateUsed;
                        outputWorksheet.Cells[outputRow, 12].Value = newSnap.StateMax - newSnap.StateUsed;
                        outputWorksheet.Cells[outputRow, 13].Value = currentSnap.StateMax;
                        outputWorksheet.Cells[outputRow, 14].Value = newSnap.StateMax;

                        outputWorksheet.Cells[outputRow, 15].Value = currentSnap.CSJUsed;
                        outputWorksheet.Cells[outputRow, 16].Value = newSnap.CSJUsed;
                        outputWorksheet.Cells[outputRow, 17].Value = currentSnap.CSJMax - currentSnap.CSJMax;
                        outputWorksheet.Cells[outputRow, 18].Value = newSnap.CSJMax - newSnap.CSJMax;
                        outputWorksheet.Cells[outputRow, 19].Value = currentSnap.CSJMax;
                        outputWorksheet.Cells[outputRow, 20].Value = newSnap.CSJMax;

                        outputWorksheet.Cells[outputRow, 21].Value = currentSnap.W2TUsed;
                        outputWorksheet.Cells[outputRow, 22].Value = newSnap.W2TUsed;
                        outputWorksheet.Cells[outputRow, 23].Value = currentSnap.W2TMax - currentSnap.W2TMax;
                        outputWorksheet.Cells[outputRow, 24].Value = newSnap.W2TMax - newSnap.W2TMax;
                        outputWorksheet.Cells[outputRow, 25].Value = currentSnap.W2TMax;
                        outputWorksheet.Cells[outputRow, 26].Value = newSnap.W2TMax;


                        outputWorksheet.Cells[outputRow, 27].Value = currentSnap.TempUsed;
                        outputWorksheet.Cells[outputRow, 28].Value = newSnap.TempUsed;
                        outputWorksheet.Cells[outputRow, 29].Value = currentSnap.TempMax - currentSnap.TempMax;
                        outputWorksheet.Cells[outputRow, 30].Value = newSnap.TempMax - newSnap.TempMax;
                        outputWorksheet.Cells[outputRow, 31].Value = currentSnap.TempMax;
                        outputWorksheet.Cells[outputRow, 32].Value = newSnap.TempMax;

                        // Apply some highlighting
                        if (!currentSnap.StateExtensionDue.GetValueOrDefault() && newSnap.StateExtensionDue.GetValueOrDefault())
                        {
                            correctionsNeedsStateExtensionCount++;
                            outputWorksheet.Cells[outputRow, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            outputWorksheet.Cells[outputRow, 5].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 177, 0));
                        }

                        if (!currentSnap.CSJExtensionDue.GetValueOrDefault() && newSnap.CSJExtensionDue.GetValueOrDefault())
                        {
                            correctionsNeedsCSJExtensionCount++;
                            outputWorksheet.Cells[outputRow, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            outputWorksheet.Cells[outputRow, 6].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 177, 0));
                        }

                        if (!currentSnap.W2TExtensionDue.GetValueOrDefault() && newSnap.W2TExtensionDue.GetValueOrDefault())
                        {
                            correctionsNeedsW2TExtensionCount++;
                            outputWorksheet.Cells[outputRow, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            outputWorksheet.Cells[outputRow, 7].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 177, 0));
                        }

                        if (!currentSnap.TempExtensionDue.GetValueOrDefault() && newSnap.TempExtensionDue.GetValueOrDefault())
                        {
                            correctionsNeedsTEMPExtensionCount++;
                            outputWorksheet.Cells[outputRow, 8].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            outputWorksheet.Cells[outputRow, 8].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 177, 0));
                        }
                    }
                }

                var resultsSheet = package.Workbook.Worksheets.Add("Totals");
                resultsSheet.Cells[2, 1].Value = "State";
                resultsSheet.Cells[2, 2].Value = correctionsNeedsStateExtensionCount;

                resultsSheet.Cells[3, 1].Value = "CSJ";
                resultsSheet.Cells[3, 2].Value = correctionsNeedsCSJExtensionCount;

                resultsSheet.Cells[4, 1].Value = "W2T";
                resultsSheet.Cells[4, 2].Value = correctionsNeedsW2TExtensionCount;

                resultsSheet.Cells[5, 1].Value = "TEMP";
                resultsSheet.Cells[5, 2].Value = correctionsNeedsTEMPExtensionCount;

                Console.WriteLine("processing complete. Writing Output");

                outputPackage.SaveAs(new FileInfo(Path.Combine(outputPath,"corrections_counts.xlsx")));
            }
        }

        private async Task ProcessPlacementClosures()
        {
            var passed = 0;
            var failed = 0;
            var skipped = 0;

            var pinsToProcess = new List<Decimal>();
            List<TimeLimit> timelimits;
            Int32 taskId;
            do
            {
                Console.WriteLine(@"What type of run would you like make?
                                                --------------------------
                                                1. Full
                                                2. File / Enter pins

                            ");
            } while (!Int32.TryParse(Console.ReadLine(), out taskId));


            using (var db = this.Container.Resolve<WwpEntities>())
            {
                var placementTypes = FlagEnums.GetFlags(ClockTypes.PlacementTypes).Select(x => (Int32)x);
                IQueryable<TimeLimit> timelimitsQuery = db.TimeLimits.Where(x => !x.IsDeleted && x.TimeLimitTypeId != null && placementTypes.Contains(x.TimeLimitTypeId.Value) && x.EffectiveMonth != null && x.EffectiveMonth.Value.Year == this.Context.Date.Year && x.EffectiveMonth.Value.Month == this.Context.Date.Month);

                switch (taskId)
                {
                    case 1:

                        break;
                    case 2:
                        pinsToProcess = this.GetIds<Decimal>();
                        timelimitsQuery = timelimitsQuery.Where(x => pinsToProcess.Contains(x.Participant.PinNumber.Value));
                        break;
                }


                timelimits = await timelimitsQuery.Include(x => x.Participant).ToListAsync(this._token).ConfigureAwait(false);
            }

            if (!pinsToProcess.Any())
                pinsToProcess.AddRange(timelimits.Select(x => x.Participant.PinNumber.GetValueOrDefault()));

            //timelineMonths = timelimits.ToDictionary(x => x.Participant.PinNumber.GetValueOrDefault(),x=>new TimelineMonth(x.EffectiveMonth.GetValueOrDefault(),(ClockTypes)x.TimeLimitTypeId,x.FederalTimeLimit.GetValueOrDefault(),x.StateTimelimit.GetValueOrDefault(),x.TwentyFourMonthLimit.GetValueOrDefault()));

            Console.WriteLine($"Selected ( {pinsToProcess.Count} ) pins to process...");


            Console.WriteLine(@"Run in simulation mode?(y/n)");
            var isSimulation = Console.ReadLine().Trim().ToLower() != "n";

            //Dictionary<Decimal, Boolean> processResults = new Dictionary<Decimal, Boolean>();

            #region create workbook

            ExcelPackage outputPackage = new ExcelPackage();
            var worksheet = outputPackage.Workbook.Worksheets.Add("Imported");
            worksheet.Row(1).Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Row(1).Style.Fill.BackgroundColor.SetColor(Color.FromArgb(13, 215, 245));
            worksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Row(1).Style.Font.Bold = true;

            worksheet.Cells[1, 1].Value = "Pin Number";
            worksheet.Cells[1, 2].Value = "Timelimit Type";
            worksheet.Cells[1, 3].Value = "Used";
            worksheet.Cells[1, 4].Value = "Max";
            worksheet.Cells[1, 5].Value = "Remaining";
            worksheet.Cells[1, 6].Value = "Clock Max";
            worksheet.Cells[1, 7].Value = "OP State Remaining";
            worksheet.Cells[1, 8].Value = "Switched";
            worksheet.Cells[1, 9].Value = "Primary Out Of Ticks";
            worksheet.Cells[1, 10].Value = "Primary Out Of State Ticks";
            worksheet.Cells[1, 11].Value = "Secondary Out Of State Ticks";
            worksheet.Cells[1, 12].Value = "Should Close?";
            worksheet.Cells[1, 13].Value = "Closed";
            worksheet.Cells[1, 14].Value = "Error";

            #endregion


            var count = 1;
            await this.RunActionByPartitions(pinsToProcess, async (pin) =>
            {
                using (var db = this.Container.Resolve<WwpEntities>())
                {
                    var outputRow = Interlocked.Increment(ref count);
                    try
                    {
                        var placementData = await db.SpTimelimitPlacementSummaryAsync(pin.ToString(), this._token).ConfigureAwait(false);
                        var primaryPlacements = placementData.Where(x => x.W2_EPISODE_BEGIN_DATE < this.Context.Date.EndOf(DateTimeUnit.Month)).Select(x => new Placement(x.PLACEMENT_TYPE, x.PLACEMENT_BEGIN_DATE, x.PLACEMENT_END_MONTH) { PinNumber = x.PARTICIPANT });

                        var allPlacements = new List<Placement>();
                        allPlacements.AddRange(primaryPlacements);

                        using (var timelimitService = this.Container.Resolve<ITimelimitService>())
                        {
                            //var assistancGroup = await timelimitService.GetOtherAGMembersAsync(id, monthRange.Start, monthRange.End, this._token);
                            var assistancGroup = timelimitService.GetOtherAGMembers(pin, this.Context.Date.StartOf(DateTimeUnit.Month), this.Context.Date.EndOf(DateTimeUnit.Month));

                            // we aren't going to switch, sincei this is only based on the old batch results. even if it's wrong

                            //foreach (var agMember in assistancGroup)
                            //{
                            //    if (!agMember.IsChild() && agMember.ELIGIBILITY_PART_STATUS_CODE != "XA")
                            //    {
                            //        var agMemberPlacements = agMember.Timeline.Placements.SelectMany(x => x.Value);
                            //        //allPlacements.AddRange(agMemberPlacements); 
                            //    }
                            //}

                            var monthlyPlacements = allPlacements.Where(c => c.PlacementType.GetValueOrDefault().HasAnyFlags(ClockTypes.PlacementTypes) && c.DateRange.Overlaps(new DateTimeRange(this.Context.Date.StartOf(DateTimeUnit.Month), this.Context.Date.EndOf(DateTimeUnit.Month))));
                            var lastPlacement = monthlyPlacements.GetMax(x => x.DateRange.End);

                            var pinNumber = pin;
                            if (lastPlacement != null)
                            {
                                //Boolean switched = false;
                                //if (lastPlacement.PinNumber != pin)
                                //{
                                //    pinNumber = lastPlacement.PinNumber;
                                //    switched = true;
                                //}


                                var timeline = await timelimitService.GetTimelineAsync(pinNumber, this._token).ConfigureAwait(false);
                                timeline.TimelineDate = this.Context.Date;
                                var timelineMonth = timeline.Months[this.Context.Date];

                                if (timelineMonth == null)
                                {
                                    throw new Exception("TimelineMonth not found for pin: " + pinNumber);
                                }

                                var clockType = timelineMonth.ClockTypes;
                                var tickType = clockType.RemoveFlags(ClockTypes.Federal | ClockTypes.State | ClockTypes.NoPlacementLimit | ClockTypes.None);

                                // Normalize Temp
                                if (tickType.HasAnyFlags(ClockTypes.TEMP))
                                {
                                    tickType = ClockTypes.TEMP;
                                }

                                var clockMax = timeline.GetClockMax(tickType);

                                Boolean primaryOutOfTicks = false;
                                Boolean secondaryOutOfStateTicks = false;
                                var primaryOutOfStateTicks = false;
                                Boolean primaryOutOfClockTicks = false;

                                Int32? otherStateParentRemaining = null;

                                if (!clockType.HasAnyFlags(ClockTypes.None))
                                {
                                    // 1. First check state clock

                                    primaryOutOfStateTicks = clockType.HasAnyFlags(ClockTypes.State) && timeline.GetRemainingMonths(ClockTypes.State) == 0;

                                    var otherParents = assistancGroup.Where(agMember => !agMember.IsChild() && agMember.ELIGIBILITY_PART_STATUS_CODE != "XA");
                                    foreach (var parent in otherParents)
                                    {
                                        parent.Timeline.TimelineDate = this.Context.Date;
                                        otherStateParentRemaining = clockType.HasAnyFlags(ClockTypes.State) ? parent.Timeline.GetRemainingMonths(ClockTypes.State) : null;
                                        secondaryOutOfStateTicks = clockType.HasAnyFlags(ClockTypes.State) && otherStateParentRemaining == 0;
                                        if (secondaryOutOfStateTicks)
                                        {
                                            break;
                                        }
                                    }

                                    // check the clock we are ticking
                                    primaryOutOfClockTicks = timeline.GetRemainingMonths(tickType) == 0;
                                    primaryOutOfTicks = primaryOutOfStateTicks || primaryOutOfClockTicks;
                                }

                                Boolean? closedSuccesfully = null;
                                var shouldClose = primaryOutOfTicks || secondaryOutOfStateTicks;
                                if (!isSimulation)
                                {
                                    var lastPlacementSummry = placementData.GetMax(x => (Int32)x.PLACEMENT_SEQUENCE_NUMBER); //TODO filter out future placements
                                    String existingFepId = lastPlacementSummry.MFWorkerId;
                                    var newFepId = ""; //TODO: What should this be?? 

                                    try
                                    {
                                        await db.SpTimeLimitPlacementClosureAsync(
                                            lastPlacementSummry.CASE_NUMBER.GetValueOrDefault(),
                                            this.Context.Date,
                                            existingFepId,
                                            lastPlacementSummry.W2_EPISODE_BEGIN_DATE.GetValueOrDefault(),
                                            pin,
                                            existingFepId,
                                            lastPlacementSummry.W2_EPISODE_END_DATE.GetValueOrDefault(),
                                            lastPlacementSummry.PLACEMENT_TYPE,
                                            lastPlacementSummry.PLACEMENT_BEGIN_DATE,
                                            newFepId,
                                            this.Context.Date,
                                            lastPlacementSummry.PLACEMENT_TYPE, this._token).ConfigureAwait(false);
                                        closedSuccesfully = true;
                                    }
                                    catch (Exception e)
                                    {
                                        closedSuccesfully = false;
                                    }
                                }


                                worksheet.Cells[outputRow, 1].Value = pinNumber;
                                worksheet.Cells[outputRow, 2].Value = FlagEnums.FormatFlags(timelineMonth.ClockTypes);
                                worksheet.Cells[outputRow, 3].Value = timeline.GetUsedMonths(tickType);
                                worksheet.Cells[outputRow, 4].Value = timeline.GetMaxMonths(tickType);
                                worksheet.Cells[outputRow, 5].Value = timeline.GetRemainingMonths(tickType);
                                worksheet.Cells[outputRow, 6].Value = clockMax;
                                worksheet.Cells[outputRow, 7].Value = otherStateParentRemaining;
                                worksheet.Cells[outputRow, 8].Value = "-";//switched;
                                worksheet.Cells[outputRow, 9].Value = primaryOutOfClockTicks;
                                worksheet.Cells[outputRow, 10].Value = primaryOutOfStateTicks;
                                worksheet.Cells[outputRow, 11].Value = secondaryOutOfStateTicks;
                                worksheet.Cells[outputRow, 12].Value = shouldClose;
                                worksheet.Cells[outputRow, 13].Value = closedSuccesfully.HasValue ? closedSuccesfully.ToString() : "-";
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        worksheet.Cells[1, 14].Value = e.Message;
                    }
                }
            }).ConfigureAwait(false);


            Console.WriteLine(@"Write output (y/n)?");
            var writeOutput = Console.ReadLine().Trim().ToLower() != "n";
            if (writeOutput)
            {
                var outputPath = this.GetOutputPath();
                outputPackage.SaveAs(new FileInfo(Path.Combine(outputPath, $"placement-closures.{this.Context.Date.Date:yyyy-MM}")));

            }

        }

        private async Task WriteTicksBackToDb2()
        {
            var ids = this.GetIds<Int32>();
            using (var dbContext = this.Container.Resolve<WwpEntities>())
            {
                var sqlRecords = await dbContext.T0459_IN_W2_LIMITS.Where(x => ids.Contains(x.Id)).ToListAsync().ConfigureAwait(false);
                if (sqlRecords.Select(x => x.PIN_NUM).Distinct().Count() > 1)
                {
                    Console.WriteLine("too many pins found, only enter records for 1 pin");
                }
                else
                {
                    var db2Records = await dbContext.Database.SqlQuery<T0459_IN_W2_LIMITS>($"SELECT * from NETWINFO_D00A_DB2.NETWINFO_D00A.PWA639TC.T0459_IN_W2_LIMITS where pin_num = '';").ToListAsync().ConfigureAwait(false);

                    // set the id's to the same thing that we already have
                    foreach (var tick in db2Records)
                    {
                        var sqlRecord = sqlRecords.FirstOrDefault(x => x.HISTORY_SEQ_NUM == tick.HISTORY_SEQ_NUM && x.BENEFIT_MM == tick.BENEFIT_MM);
                        if (sqlRecord != null)
                        {
                            tick.Id = sqlRecord.Id;
                        }
                    }

                }
            }
        }

        private async Task RunActionByPartitions(List<Decimal> pins, Func<decimal, Task> action)
        {
            var count = pins.Count;
            var partition1 = new Stack<Decimal>(pins.Where(x => x <= 0999999999));
            var partition2 = new Stack<Decimal>(pins.Where(x => x < 1999999999 && x >= 1000000000));
            var partition3 = new Stack<Decimal>(pins.Where(x => x < 2999999999 && x >= 2000000000));
            var partition4 = new Stack<Decimal>(pins.Where(x => x < 3999999999 && x >= 3000000000));
            var partition5 = new Stack<Decimal>(pins.Where(x => x < 4999999999 && x >= 4000000000));
            var partition6 = new Stack<Decimal>(pins.Where(x => x < 5999999999 && x >= 5000000000));
            var partition7 = new Stack<Decimal>(pins.Where(x => x < 6999999999 && x >= 6000000000));
            var partition8 = new Stack<Decimal>(pins.Where(x => x < 7999999999 && x >= 7000000000));
            var partition9 = new Stack<Decimal>(pins.Where(x => x < 8999999999 && x >= 8000000000));
            var partition10 = new Stack<Decimal>(pins.Where(x => x >= 9000000000));

            while (true)
            {
                var tasks = new List<Task>();
                if (partition1.Any()) tasks.Add(action(partition1.Pop()));
                if (partition2.Any()) tasks.Add(action(partition2.Pop()));
                if (partition3.Any()) tasks.Add(action(partition3.Pop()));
                if (partition4.Any()) tasks.Add(action(partition4.Pop()));
                if (partition5.Any()) tasks.Add(action(partition5.Pop()));
                if (partition6.Any()) tasks.Add(action(partition6.Pop()));
                if (partition7.Any()) tasks.Add(action(partition7.Pop()));
                if (partition8.Any()) tasks.Add(action(partition8.Pop()));
                if (partition9.Any()) tasks.Add(action(partition9.Pop()));
                if (partition10.Any()) tasks.Add(action(partition10.Pop()));
                if (!tasks.Any())
                    break;
                await Task.WhenAll(tasks).ConfigureAwait(false);

                var remaining = partition1.Count + partition2.Count + partition3.Count + partition4.Count + partition5.Count + partition6.Count + partition7.Count + partition8.Count + partition9.Count + partition10.Count;
                //var percent = remaining / count * 100;
                Console.WriteLine($" {(Double)(count - remaining) / count:P}% completed");
            }
        }

        private async Task ImportDb2Ticks()
        {
            Console.WriteLine("Do you want to overwrite previously imported ticks?");
            var overwrite = Console.ReadLine() == "y";

            var passed = 0;
            var failed = 0;
            var skipped = 0;

            var pinsToProcess = new List<Decimal>();
            Int32 taskId;
            do
            {
                Console.WriteLine(@"What type of run would you like make?
                                                --------------------------
                                                1. Full
                                                2. File / Enter pins

                            ");
            } while (!Int32.TryParse(Console.ReadLine(), out taskId));


            switch (taskId)
            {
                case 1:
                    using (var db = this.Container.Resolve<WwpEntities>())
                    {
                        var dmonth = Decimal.Parse(this.Context.Date.ToString("yyyyMM"));
                        pinsToProcess = await db.T0459_IN_W2_LIMITS.Where(x => x.BENEFIT_MM == dmonth).Select(x => x.PIN_NUM).Distinct().ToListAsync().ConfigureAwait(false);
                    }

                    break;
                case 2:
                    pinsToProcess = this.GetIds<Decimal>();
                    break;
            }

            Console.WriteLine($"Selected ( {pinsToProcess.Count} ) pins to process...");


            Console.WriteLine(@"Run in simulation mode?(y/n)");
            var isSimulation = Console.ReadLine().Trim().ToLower() != "n";
            var createdTimeLimits = new ConcurrentDictionary<Decimal, List<ITimeLimit>>();

            var importAllMonths = false;
            if (pinsToProcess.Count == 1)
            {
                Console.WriteLine("Single pin selected. Import all months?");
                importAllMonths = Console.ReadLine().Trim().ToLower() == "y";
            }


            var no24DateRange = new DateTimeRange(new DateTime(2009, 11, 01), new DateTime(2011, 12, 31));

            //foreach (var pin in pinsToProcess)
            //{
            await this.RunActionByPartitions(pinsToProcess, (pin) =>
            {
                using (var repo = this.Container.Resolve<IRepository>())
                {
                    using (LogContext.PushProperty("PinNumber", pin))
                    {

                        repo.ResetContext();

                        var monthsToProcess = new List<DateTime>();

                        if (importAllMonths)
                        {
                            monthsToProcess.AddRange(repo.GetW2LimitsByPin(pin).Select(x => DateTime.ParseExact(x.BENEFIT_MM.ToString(CultureInfo.InvariantCulture), "yyyyMM", CultureInfo.InvariantCulture)));
                        }
                        else
                        {
                            monthsToProcess.Add(this.Context.Date);
                        }

                        foreach (var month in monthsToProcess)
                        {
                            try
                            {
                                var dMonth = Decimal.Parse(month.ToStringMonthYearComposite());
                                var tick = repo.GetW2LimitByMonth(dMonth, pin);
                                var timelimit = repo.TimeLimitByDate(pin.ToString(), month, false);
                                IParticipantInfo participant = repo.GetParticipant(pin.ToString());
                                if (tick == null)
                                {
                                    this._logger.Information("skipping pin, No DB2 record found. ");
                                    skipped++;
                                    continue;
                                }

                                if (participant == null)
                                {

                                    try
                                    {
                                        participant = repo.GetRefreshedParticipant(pin.ToString());
                                    }
                                    catch (Exception)
                                    {
                                        this._logger.Information("Failed to refresh participant.");

                                    }
                                    if (participant == null)
                                    {
                                        this._logger.Information("skipping pin, No Participant record found. ");
                                        skipped++;
                                        continue;
                                    }
                                }

                                if (timelimit == null)
                                {
                                    this._logger.Information($"No Timelimit record found. Creating one! ");
                                    timelimit = repo.NewTimeLimit();
                                }
                                else if (timelimit.TimeLimitTypeId == (Int32)ClockTypes.None)
                                {

                                    this._logger.Information($"Edited \"None\" Timelimit record found. Overwriting! ");

                                }
                                else if (timelimit.ModifiedDate.HasValue)
                                {
                                    this._logger.Information($"Edited Timelimit record found with ClockType: \"{(ClockTypes)timelimit.TimeLimitTypeId.Value}\". skipping! ");
                                    skipped++;
                                    continue;
                                }
                                else if (!overwrite)
                                {
                                    this._logger.Information($"Batch Timelimit record found with ClockType: \"{(ClockTypes)timelimit.TimeLimitTypeId.Value}\". Skipping! ");
                                    skipped++;
                                    continue;
                                }
                                else
                                {
                                    this._logger.Information($"Batch Timelimit record found with ClockType: \"{(ClockTypes)timelimit.TimeLimitTypeId.Value}\". Overwriting! ");
                                }

                                ClockTypes clockType;
                                if (!Enum.TryParse(tick.CLOCK_TYPE_CD, out clockType))
                                {
                                    this._logger.Information($" Unable to parse clocktype with CLOCK_TYPE_CD: \"{tick.CLOCK_TYPE_CD}\". Skipping! ");
                                }

                                timelimit.ParticipantID = participant.Id;
                                timelimit.EffectiveMonth = month.StartOf(DateTimeUnit.Month);
                                timelimit.TimeLimitTypeId = (Int32)clockType;
                                timelimit.TwentyFourMonthLimit = clockType.HasAnyFlags(ClockTypes.PlacementTypes) && !no24DateRange.Contains(timelimit.EffectiveMonth.Value);
                                timelimit.StateTimelimit = true;
                                timelimit.FederalTimeLimit = tick.FED_CLOCK_IND == "Y";
                                timelimit.CreatedDate = tick.UPDATED_DT;
                                timelimit.ModifiedBy = "WWP Batch";
                                timelimit.Notes = $"Historical data from old WP application, Created transaction code:{tick.CRE_TRAN_CD}, Comments from old WP application: {tick.COMMENT_TXT}";
                                timelimit.IsDeleted = tick.OVERRIDE_REASON_CD?.ToUpper().StartsWith("S") ?? false;

                                if (!isSimulation)
                                {
                                    repo.Save();
                                }

                                createdTimeLimits.AddOrUpdate(pin, new List<ITimeLimit>() { timelimit }, (k, v) =>
                                {
                                    v.Add(timelimit);
                                    return v;
                                });
                                passed++;
                            }
                            catch (Exception e)
                            {
                                this._logger.Error(e, $"There was an error proceszsing this pin for month: {month:MM/yyyy} ");
                                failed++;
                            }
                        }
                    }
                }

                return Task.CompletedTask;
            }).ConfigureAwait(false);

            //}
            this._logger.Information($"Finished processing {pinsToProcess.Count}. Passed: {passed}. Failed: {failed}. Skipped: {skipped} ");


            Console.WriteLine(@"Write output (y/n)?");
            var writeOutput = Console.ReadLine().Trim().ToLower() != "n";
            if (writeOutput)
            {
                var outputPath = this.GetOutputPath();

                this._logger.Information("outputPath: {path}", outputPath);

                ExcelPackage outputPackage = new ExcelPackage();
                var worksheet = outputPackage.Workbook.Worksheets.Add("Imported");

                worksheet.Cells[1, 1].Value = "Pin Number";
                worksheet.Cells[1, 2].Value = "Id";
                worksheet.Cells[1, 3].Value = "ParticipantID";
                worksheet.Cells[1, 4].Value = "Effective Month";
                worksheet.Cells[1, 5].Value = "TimeLimit Type";
                worksheet.Cells[1, 6].Value = "State Timelimit";
                worksheet.Cells[1, 7].Value = "Federal TimeLimit";
                worksheet.Cells[1, 8].Value = "Created Date";
                worksheet.Cells[1, 9].Value = "Modified Date";

                Int32 row = 1;
                foreach (var pin in pinsToProcess)
                {
                    var timelimits = createdTimeLimits.ContainsKey(pin) ? createdTimeLimits[pin] : null;
                    if (timelimits != null)
                    {
                        foreach (var timelimit in timelimits)
                        {

                            row++;
                            //var timelimit = createdTimeLimits.ContainsKey(pin) ? createdTimeLimits[pin] : null;

                            worksheet.Cells[row, 1].Value = pin;
                            if (timelimit == null)
                            {
                                worksheet.Cells[row, 2].Value = "Skipped or failed";
                                continue;
                            }

                            worksheet.Cells[row, 2].Value = timelimit.Id;
                            worksheet.Cells[row, 3].Value = timelimit.ParticipantID;
                            worksheet.Cells[row, 3].Value = timelimit.ParticipantID;
                            worksheet.Cells[row, 4].Style.Numberformat.Format = "yyyy-MM-dd";
                            worksheet.Cells[row, 4].Formula = $"=DATE({timelimit.EffectiveMonth.Value.Year},{timelimit.EffectiveMonth.Value.Month},{timelimit.EffectiveMonth.Value.Day})";
                            worksheet.Cells[row, 5].Value = ((ClockTypes)timelimit.TimeLimitTypeId).ToString();
                            worksheet.Cells[row, 6].Value = timelimit.StateTimelimit;
                            worksheet.Cells[row, 7].Value = timelimit.FederalTimeLimit;
                            worksheet.Cells[row, 8].Value = timelimit.CreatedDate;
                            worksheet.Cells[row, 9].Value = timelimit.ModifiedDate;
                        }
                    }
                }



                var filePath = Path.Combine(outputPath, $"imported_db2_ticks.{DateTime.Now:MMMM - yyyy-dd hh.mm.ss}.xlsx");
                var fileInfo = new FileInfo(filePath);
                outputPackage.SaveAs(fileInfo);
                this._logger.Information("Output file saved!");
            }
            // Write Output
        }

        private async Task FixDb2CountsAsync()
        {
            Int32 taskId = 0;


            var pinsToProcess = this.GetIds<Decimal>();




            Console.WriteLine(@"Run in simulation mode?(y/n)");
            var isSimulation = Console.ReadLine().Trim().ToLower() != "n";


            var oldPinDictionary = new ConcurrentDictionary<Decimal, List<IT0459_IN_W2_LIMITS>>();
            var newPinDictionary = new ConcurrentDictionary<Decimal, List<IT0459_IN_W2_LIMITS>>();
            var pinDictionary = new ConcurrentDictionary<Decimal, List<IT0459_IN_W2_LIMITS>>();

            //var queue = new ActionBlock<Decimal>(pin =>
            var queue = this.RunActionByPartitions(pinsToProcess, async pin =>
            {
                using (var repo = this.Container.Resolve<IRepository>())
                using (var timelimitService = this.Container.Resolve<ITimelimitService>())
                using (var db2TimelimitsService = this.Container.Resolve<IDb2TimelimitService>())
                using (this._logger.BeginTimedOperation("Job Proccesing", pin.ToString()))
                {
                    var stopWatch = new Stopwatch();
                    db2TimelimitsService.IsSimulated = isSimulation;

                    try
                    {
                        stopWatch.Start();
                        var participant = repo.GetParticipant(pin.ToString());
                        var ticksToUpdate = repo.GetLatestW2LimitsMonthsForEachClockType(pin);
                        var timeline = timelimitService.GetTimeline(participant.PinNumber.GetValueOrDefault());



                        var newTicks = db2TimelimitsService.UpdateTicks0459(ticksToUpdate, timeline, false, "Fixing counts for WWP DB2 Write back.");

                        if (!isSimulation)
                        {
                            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
                            {
                                try
                                {
                                    db2TimelimitsService.Save();
                                    scope.Complete();
                                }
                                catch (Exception e)
                                {
                                    scope.Dispose();
                                }
                            }
                        }
                        //Thread.Sleep(1000);

                        var outputTicks = new List<IT0459_IN_W2_LIMITS>();
                        outputTicks.AddRange(newTicks);
                        outputTicks.AddRange(ticksToUpdate);

                        oldPinDictionary.AddOrUpdate(pin, ticksToUpdate, (k, v) =>
                        {
                            v.AddRange(ticksToUpdate);
                            return v;
                        });

                        newPinDictionary.AddOrUpdate(pin, newTicks, (k, v) =>
                        {
                            v.AddRange(newTicks);
                            return v;
                        });

                        pinDictionary.AddOrUpdate(pin, outputTicks, (k, v) =>
                        {
                            v.AddRange(outputTicks);
                            return v;
                        });

                        stopWatch.Stop();
                    }
                    catch (Exception e)
                    {
                        this._logger.Error(e, $"Error processing job: {pin}");
                    }
                    finally
                    {
                        stopWatch.Stop();
                    }
                }
            });
            //}, new ExecutionDataflowBlockOptions { CancellationToken = this._token, MaxDegreeOfParallelism = 1, SingleProducerConstrained = true });

            //foreach (var pin in pinsToProcess)
            //{
            //    queue.Post(pin);
            //}

            this._logger.Information($"Stopping Producer queue: All processing Items generated. Generated [{pinsToProcess.Count}] items to process. ");
            //queue.Complete();

            await queue.ConfigureAwait(false);
            //await queue.Completion.ConfigureAwait(false);

            var outputPath = this.GetOutputPath();

            this._logger.Information("Processing complete, writing output file...");



            this._logger.Information("outputPath: {path}", outputPath);
            // Write Output

            do
            {
                Console.WriteLine(@"What type of run would you like?
                                    --------------------------
                                    1. Write SQL Files?
                                    2. Write Spreadsheet
                                    3. None
                ");
            } while (!Int32.TryParse(Console.ReadLine(), out taskId));

            if (taskId == 1)
            {

                var updateSql = @" DECLARE @PIN_NUM DECIMAL(10,0) = {0};
                                     Declare @BENEFIT_MM DECIMAL(6,0) = {1};
                                     DECLARE @HISTORY_SEQ_NUM smallint = {2}; 
                                     DECLARE @CLOCK_TYPE_CD char(4) =  '{3}';
                                     Declare @statement1 as varchar(max);

                                        set @statement1 = ' UPDATE PWP639TC.T0459_IN_W2_LIMITS 

                                        SET HISTORY_CD = 9 

                                        WHERE PIN_NUM = '+CONVERT(VARCHAR(10),@PIN_NUM)+'

                                        AND BENEFIT_MM = '+CONVERT(VARCHAR(10),@BENEFIT_MM)+'

                                        AND HISTORY_SEQ_NUM = '+CONVERT(VARCHAR(10),@HISTORY_SEQ_NUM)+' 

                                        AND CLOCK_TYPE_CD = '''+@CLOCK_TYPE_CD+''' 

                                        '

　

                                        print @statement1


                                        exec (@statement1) at NETWINFO_D40P_DB2;
GO";

                var insertSql = @" 
                                    declare @PIN_NUM decimal(10, 0) = {0};
                                    declare @BENEFIT_MM decimal(6, 0) = {1};
                                    declare @HISTORY_SEQ_NUM smallint = {2} ;
                                    declare @CLOCK_TYPE_CD char(4) = '{3}';
                                    declare @CRE_TRAN_CD char(8) = '{4}';
                                    declare @FED_CLOCK_IND char(1) = '{5}';
                                    declare @FED_CMP_MTH_NUM smallint = {6};
                                    declare @FED_MAX_MTH_NUM smallint = {7};
                                    declare @HISTORY_CD smallint = {8};
                                    declare @OT_CMP_MTH_NUM smallint = {9};
                                    declare @OVERRIDE_REASON_CD char(3) = '{10}';
                                    declare @TOT_CMP_MTH_NUM smallint = {11};
                                    declare @TOT_MAX_MTH_NUM smallint = {12};
                                    declare @UPDATED_DT date = '{13}';
                                    declare @USER_ID char(6) = '{14}';
                                    declare @WW_CMP_MTH_NUM smallint = {15};
                                    declare @WW_MAX_MTH_NUM smallint = {16};
                                    declare @COMMENT_TXT varchar(75) = '{17}';
                                    declare @statement1 as varchar(max);


                                    set @statement1 = ' INSERT INTO PWP639TC.T0459_IN_W2_LIMITS ( PIN_NUM , BENEFIT_MM , HISTORY_SEQ_NUM , CLOCK_TYPE_CD , CRE_TRAN_CD , 

                                    FED_CLOCK_IND , FED_CMP_MTH_NUM , FED_MAX_MTH_NUM , HISTORY_CD , 

                                    OT_CMP_MTH_NUM , OVERRIDE_REASON_CD , TOT_CMP_MTH_NUM , 

                                    TOT_MAX_MTH_NUM , UPDATED_DT , USER_ID , WW_CMP_MTH_NUM , 

                                    WW_MAX_MTH_NUM , COMMENT_TXT ) 

                                    VALUES ('+CONVERT(VARCHAR(10),@PIN_NUM)+','+CONVERT(VARCHAR(10),@BENEFIT_MM)+','+CONVERT(VARCHAR(10),@HISTORY_SEQ_NUM)+','''+@CLOCK_TYPE_CD+''','''+@CRE_TRAN_CD+''', 

                                    '''+CONVERT(VARCHAR(10),@FED_CLOCK_IND)+''' , '+CONVERT(VARCHAR(10),@FED_CMP_MTH_NUM)+' , '+CONVERT(VARCHAR(10),@FED_MAX_MTH_NUM)+' , '+CONVERT(VARCHAR(10),@HISTORY_CD)+' , 

                                    '+CONVERT(VARCHAR(10),@OT_CMP_MTH_NUM)+' , '''+CONVERT(VARCHAR(10),@OVERRIDE_REASON_CD)+''', '+CONVERT(VARCHAR(10),@TOT_CMP_MTH_NUM)+' , 

                                    '+CONVERT(VARCHAR(10),@TOT_MAX_MTH_NUM)+' , '''+CONVERT(VARCHAR(16),@UPDATED_DT)+''' , '''+@USER_ID+''' , '+CONVERT(VARCHAR(10),@WW_CMP_MTH_NUM)+' , 

                                    '+CONVERT(VARCHAR(10),@WW_MAX_MTH_NUM)+' , '''+@COMMENT_TXT+''' )

                                    '


                                    exec (@statement1) at NETWINFO_D40P_DB2;
GO";


                foreach (var pin in pinDictionary)
                {
                    StringBuilder sb = new StringBuilder();
                    var updateRecords = pin.Value.Where(x => x.HISTORY_CD == 9);
                    var insertRecords = pin.Value.Where(x => x.HISTORY_CD == 0);

                    foreach (var record in updateRecords)
                    {
                        var sql = String.Format(updateSql, record.PIN_NUM, record.BENEFIT_MM, record.HISTORY_SEQ_NUM, record.CLOCK_TYPE_CD);
                        sb.AppendLine(sql);
                        sb.AppendLine("----------");


                    }
                    foreach (var insert in insertRecords)
                    {
                        var sql = String.Format(insertSql,
                            insert.PIN_NUM,
                            insert.BENEFIT_MM,
                            insert.HISTORY_SEQ_NUM,
                            insert.CLOCK_TYPE_CD,
                            insert.CRE_TRAN_CD,
                            insert.FED_CLOCK_IND,
                            insert.FED_CMP_MTH_NUM,
                            insert.FED_MAX_MTH_NUM,
                            insert.HISTORY_CD,
                            insert.OT_CMP_MTH_NUM,
                            insert.OVERRIDE_REASON_CD,
                            insert.TOT_CMP_MTH_NUM,
                            insert.TOT_MAX_MTH_NUM,
                            insert.UPDATED_DT.ToString("yyyy-MM-dd"),
                            insert.USER_ID,
                            insert.WW_CMP_MTH_NUM,
                            insert.WW_MAX_MTH_NUM,
                            insert.COMMENT_TXT.Replace("'", "''''"));
                        sb.AppendLine(sql);
                        sb.AppendLine("----------");
                    }

                    File.WriteAllText(Path.Combine(outputPath, pin.Key + ".sql"), sb.ToString());
                    this._logger.Information($"Output file {pin}.sql saved!");

                }
            }
            else if (taskId == 2)
            {

                ExcelPackage outputPackage = new ExcelPackage();



                foreach (var kvp in pinDictionary.OrderBy(y => y.Key))
                {
                    var worksheet = outputPackage.Workbook.Worksheets.Add(kvp.Key.ToString().PadLeft(10, '0'));

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

                    var list = kvp.Value;
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
                }


                var filePath = Path.Combine(outputPath, $"corrected_t0459_records.{DateTime.Now:MM.dd.yyyy}.xlsx");
                var fileInfo = new FileInfo(filePath);
                outputPackage.SaveAs(fileInfo);
                this._logger.Information("Output file saved!");
            }
            else
            {
                this._logger.Information("Skipping file output...");

            }
        }

        //private async Task RunSimulatedBatchAgainstSpreadsheet()
        //{
        //    Boolean again = true;
        //    Int32 taskId = 0;

        //    while (again)
        //    {
        //        do
        //        {
        //            Console.WriteLine(@"What spreadsheet would you like to run?
        //                            --------------------------
        //                            1. Full (acc.xlsx)
        //                            2. Other
        //        ");
        //        } while (!Int32.TryParse(Console.ReadLine(), out taskId));

        //        List<Decimal> pinsToProcess = new List<Decimal>();

        //        var excelFile = "";

        //        // add pins to the queue
        //        if (taskId == 1)
        //        {
        //            excelFile = "acc.xlsx";
        //        }
        //        else if (taskId == 2)
        //        {
        //            do
        //            {
        //                Console.WriteLine("Enter spreadsheet Name");
        //                excelFile = Console.ReadLine();
        //            } while (!File.Exists(excelFile) || Path.GetExtension(excelFile) != ".xlsx");
        //        }

        //        this.Context.PinsToProcess.Clear();

        //        this.CreateProcessingQueue();
        //        foreach (var pin in pinsToProcess)
        //        {
        //            this.Context.PinsToProcess.Add(pin);
        //        }
        //        await this.GetPinsToProcess().ConfigureAwait(false);

        //        Console.WriteLine("Starting up producer/consumer queue");
        //        await this.StartProducerQueue().ConfigureAwait(false);
        //        await this._queue.Completion.ConfigureAwait(false);
        //        this.PinsToProcess = new AsyncCollection<Decimal>();//so we can run again!

        //        var path = DirectoryHelper.CreateIfNotExists($"SimulaterOuput/{this.Context.Date:MM.yyyy}");
        //        //Once complete, write the files output 
        //        foreach (var kvps in this.taskOutput)
        //        {
        //            var outputPath = Path.Combine(path, $"{String.Join(".", FlagEnums.GetFlagMembers(kvps.Key).Select(x => x.Name))}.txt");
        //            File.WriteAllText(
        //                outputPath
        //                , kvps.Value.ToString());
        //        }

        //        var fullOutput = this.pinOutput.OrderBy(kvp => kvp.Key).Select(kvp => { return $"{kvp.Key} - {FlagEnums.FormatFlags(kvp.Value)} "; });
        //        File.WriteAllLines(Path.Combine(path, "AllPins.txt"), fullOutput);

        //        Console.WriteLine("Run again(y/n):");
        //        again = Console.ReadLine()?.ToLower() != "n";
        //    }
        //}

        private async Task ParseBatchOutput()
        {
            var folder = "";

            DirectoryInfo directory;
            var again = false;
            var outputRow = 2;
            using (var outputPackage = new ExcelPackage())
            {
                var outputWorksheet = outputPackage.Workbook.Worksheets.Add("results");
                #region Create outputPackage Header
                outputWorksheet.Row(outputRow).Style.Fill.PatternType = ExcelFillStyle.Solid;
                outputWorksheet.Row(outputRow).Style.Fill.BackgroundColor.SetColor(Color.FromArgb(13, 215, 245));
                outputWorksheet.Row(outputRow).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                outputWorksheet.Row(outputRow).Style.Font.Bold = true;

                outputWorksheet.Cells[outputRow, 1].Value = "Pin Number";
                outputWorksheet.Cells[outputRow, 2].Value = "Legacy Batch Timelimit";
                outputWorksheet.Cells[outputRow, 3].Value = "New Batch Timelimit Type";
                outputWorksheet.Cells[outputRow, 4].Value = "Pass/Fail";
                outputWorksheet.Cells[outputRow, 5].Value = "Expected Discrepency";
                outputWorksheet.Cells[outputRow, 6].Value = "Batch Notes / Results:";
                outputWorksheet.Cells[outputRow, 7].Value = "Last Employment Position";
                outputWorksheet.Cells[outputRow, 8].Value = "Placements";
                outputWorksheet.Cells[outputRow, 9].Value = "Previous Placement";
                outputWorksheet.Cells[outputRow, 10].Value = "First Non Cmc Employment Position";
                outputWorksheet.Cells[outputRow, 11].Value = "Had Previous Paid Placement In Month";
                outputWorksheet.Cells[outputRow, 12].Value = "Moved Directly Into Cmc";
                outputWorksheet.Cells[outputRow, 13].Value = "Has Child Born 10Mmonths After Paid w2Start";
                outputWorksheet.Cells[outputRow, 14].Value = "Cmc Should Tick Previous Placement";
                outputWorksheet.Cells[outputRow, 15].Value = "Is Alien";
                outputWorksheet.Cells[outputRow, 16].Value = "Payments Are Fully Sanctioned";
                outputWorksheet.Cells[outputRow, 17].Value = "Payments (data)";
                outputWorksheet.Cells[outputRow, 18].Value = "Placments (data)";
                outputWorksheet.Cells[outputRow, 19].Value = "Alien Statues (data)";
                outputWorksheet.Cells[outputRow, 20].Value = "Assistance Group (data)";

                #endregion

                var dMonth = Decimal.Parse(this.Context.Date.ToString("yyyyMM"));
                do
                {
                    Console.WriteLine("What directory do you want to process?");
                    var path = Console.ReadLine().Trim('\"');

                    directory = new DirectoryInfo(path);
                    if (!directory.Exists)
                    {
                        Console.WriteLine("Doesn't exisist");
                    }
                    else
                    {
                        try
                        {
                            var files = directory.GetFiles("*.xlsx", SearchOption.AllDirectories).ToList();

                            Dictionary<Decimal, T0459_IN_W2_LIMITS> legacyTicks;
                            using (var dbContext = this.Container.Resolve<WwpEntities>())
                            {
                                legacyTicks = await dbContext.T0459_IN_W2_LIMITS.Where(x => x.BENEFIT_MM == dMonth && x.CRE_TRAN_CD == "PWCAEP11"
                                                                                                                   && x.HISTORY_SEQ_NUM == (dbContext.T0459_IN_W2_LIMITS.Where(y => x.BENEFIT_MM == y.BENEFIT_MM && x.PIN_NUM == y.PIN_NUM && y.CRE_TRAN_CD == "PWCAEP11")).Max(y => y.HISTORY_SEQ_NUM)
                                                                                                                   ).ToDictionaryAsync(x => x.PIN_NUM).ConfigureAwait(false);
                            }

                            var missingBatchPins = new Dictionary<Decimal, Decimal>();
                            var missingBatchFile = new FileInfo(Path.Combine(directory.FullName, "missing_batch_ticks.xlsx"));
                            if (!missingBatchFile.Exists)
                            {
                                // Add missing records where the legacy batch ticked and the new batch didn't process the pin
                                Console.WriteLine("Adding records where the legacy batch ticked and the new batch didn't process the pin...");
                                var batchPins = new HashSet<Decimal>();
                                for (var index = 0; index < files.Count; index++)
                                {
                                    var package = new ExcelPackage();
                                    var file = files[index];
                                    using (var fs = file.OpenRead())
                                    {
                                        package.Load(fs);
                                    }

                                    var row = 3;
                                    var worksheet = package.Workbook.Worksheets[1];
                                    string sPin;
                                    do
                                    {
                                        sPin = worksheet.Cells[row, 1].GetValue<String>();
                                        Decimal pin;
                                        if (!sPin.IsNullOrWhiteSpace() && Decimal.TryParse(sPin, out pin) && !batchPins.Contains(pin))
                                        {
                                            batchPins.Add(pin);
                                        }

                                        row++;
                                    } while (!sPin.IsNullOrWhiteSpace());

                                }

                                var package2 = new ExcelPackage();
                                var worksheet2 = package2.Workbook.Worksheets.Add("Results");
                                worksheet2.Row(outputRow).Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet2.Row(outputRow).Style.Fill.BackgroundColor.SetColor(Color.FromArgb(13, 215, 245));
                                worksheet2.Row(outputRow).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                worksheet2.Row(outputRow).Style.Font.Bold = true;

                                worksheet2.Cells[outputRow, 1].Value = "Pin Number";
                                worksheet2.Cells[outputRow, 2].Value = "Legacy Batch Timelimit";
                                worksheet2.Cells[outputRow, 3].Value = "New Batch Timelimit Type";

                                missingBatchPins = legacyTicks.Keys.Except(batchPins).ToDictionary(x => x);
                                foreach (var mPin in missingBatchPins.Keys.ToList())
                                {
                                    outputRow++;
                                    var legacyTickFlags = FlagEnums.ParseFlags<ClockTypes>(legacyTicks[mPin].CLOCK_TYPE_CD).CombineFlags(ClockTypes.State);
                                    legacyTickFlags = legacyTicks[mPin].FED_CLOCK_IND.Trim() == "Y" ? legacyTickFlags.CombineFlags(ClockTypes.Federal) : legacyTickFlags;
                                    worksheet2.Cells[outputRow, 1].Value = mPin;
                                    worksheet2.Cells[outputRow, 2].Value = FlagEnums.FormatFlags(legacyTickFlags);
                                    worksheet2.Cells[outputRow, 3].Value = "None";
                                    worksheet2.Cells[outputRow, 4].Value = "F";
                                }

                                package2.SaveAs(missingBatchFile);
                                Console.WriteLine("Created missing_batch_ticks.xlsx");
                                files.Add(missingBatchFile);
                            }

                            outputRow = 2;

                            for (var index = 0; index < files.Count; index++)
                            {
                                Console.Write($"Proccessing item {index + 1} / {files.Count}");

                                var package = new ExcelPackage();
                                var file = files[index];
                                using (var fs = file.Open(FileMode.Open,FileAccess.Read,FileShare.ReadWrite))
                                {
                                    package.Load(fs);
                                }

                                var row = 3;
                                String pin;
                                var worksheet = package.Workbook.Worksheets[1];

                                do
                                {
                                    pin = worksheet.Cells[row, 1].GetValue<String>();
                                    //string status = worksheet.Cells[row, 4].GetValue<String>();
                                    //if (!pin.IsNullOrWhiteSpace() && status == "F")
                                    if (!pin.IsNullOrWhiteSpace())
                                    {
                                        var dPin = Decimal.Parse(pin);

                                        if (file.Name == "Created missing_batch_ticks.xlsx" && missingBatchPins.ContainsKey(dPin))
                                        {
                                            Console.WriteLine($"skipping {dPin} because it is in the missing pins. Should Catch it later...");
                                            row++;
                                            continue;
                                        }

                                        outputRow++;
                                        ClockTypes expectedClockType = ClockTypes.None;

                                        var legacyTick = worksheet.Cells[row, 2].GetValue<String>();
                                        if (string.IsNullOrEmpty(legacyTick))
                                        {
                                            T0459_IN_W2_LIMITS tick;
                                            //(x => x.PIN_NUM == dPin);//dbContext.T0459_IN_W2_LIMITS.FirstOrDefault(x => x.BENEFIT_MM == dMonth && x.PIN_NUM == dPin);
                                            if (legacyTicks.TryGetValue(dPin, out tick) && tick != null)
                                            {
                                                Enum.TryParse(tick.CLOCK_TYPE_CD, out expectedClockType);
                                                expectedClockType = expectedClockType.CombineFlags(ClockTypes.State);
                                                if (tick.FED_CLOCK_IND == "Y")
                                                {
                                                    expectedClockType = expectedClockType.CombineFlags(ClockTypes.Federal);
                                                }
                                            }
                                            legacyTick = FlagEnums.FormatFlags(expectedClockType);
                                        }
                                        else
                                        {
                                            expectedClockType = FlagEnums.ParseFlags<ClockTypes>(legacyTick);
                                        }


                                        outputWorksheet.Cells[outputRow, 1].Value = worksheet.Cells[row, 1].Value;
                                        outputWorksheet.Cells[outputRow, 2].Value = legacyTick;
                                        outputWorksheet.Cells[outputRow, 3].Value = worksheet.Cells[row, 3].Value;

                                        ClockTypes actualResult = FlagEnums.ParseFlags<ClockTypes>(worksheet.Cells[row, 3].GetValue<String>());

                                        if (expectedClockType == actualResult)
                                        {
                                            outputWorksheet.Cells[outputRow, 4].Value = "P";
                                        }
                                        else
                                        {
                                            outputWorksheet.Cells[outputRow, 4].Value = "F";
                                        }

                                        //var itemToProcess = await this.GetQueueItemAsync(dPin).ConfigureAwait(false);
                                        //if (itemToProcess.Status.GetValueOrDefault() == JobStatus.ReadyForJobProcessing)
                                        //{
                                        //await this.ProcessItem(itemToProcess).ConfigureAwait(false);




                                        //outputRow++;
                                        //outputWorksheet.Cells[outputRow, 1].Value = worksheet.Cells[row, 1].Value;
                                        //outputWorksheet.Cells[outputRow, 2].Value = worksheet.Cells[row, 2].Value;
                                        //outputWorksheet.Cells[outputRow, 3].Value = worksheet.Cells[row, 3].Value;
                                        //outputWorksheet.Cells[outputRow, 4].Value = worksheet.Cells[row, 4].Value;

                                        //var timeline = itemToProcess.Timeline;
                                        //RuleContext ruleContext;
                                        //if (!this.pinOutput.TryGetValue(dPin, out ruleContext))
                                        //{
                                        //    outputWorksheet.Cells[outputRow, 6].Value = "Failed to evalute pin";
                                        //}
                                        //else
                                        //{
                                        //    outputWorksheet.Cells[outputRow, 5].Value = "";
                                        //    outputWorksheet.Cells[outputRow, 6].Value = "";
                                        //    outputWorksheet.Cells[outputRow, 7].Value = this.FormatObjectForOutput(ruleContext.LastEmploymentPosition);
                                        //    outputWorksheet.Cells[outputRow, 8].Value = "-";
                                        //    outputWorksheet.Cells[outputRow, 9].Value = this.FormatObjectForOutput(ruleContext.PreviousPlacement);
                                        //    outputWorksheet.Cells[outputRow, 10].Value = this.FormatObjectForOutput(ruleContext.FirstNonCmcEmploymentPosition);
                                        //    outputWorksheet.Cells[outputRow, 11].Value = this.FormatObjectForOutput(ruleContext.HadPreviousPaidPlacementInMonth);
                                        //    outputWorksheet.Cells[outputRow, 12].Value = this.FormatObjectForOutput(ruleContext.MovedDirectlyIntoCmc);
                                        //    outputWorksheet.Cells[outputRow, 13].Value = this.FormatObjectForOutput(ruleContext.HasChildBorn10monthsAfterPaidw2Start);
                                        //    outputWorksheet.Cells[outputRow, 14].Value = this.FormatObjectForOutput(ruleContext.CmcShouldTickPreviousPlacement);
                                        //    outputWorksheet.Cells[outputRow, 15].Value = this.FormatObjectForOutput(ruleContext.IsAlien);
                                        //    outputWorksheet.Cells[outputRow, 16].Value = this.FormatObjectForOutput(ruleContext.PaymentsAreFullySanctioned);
                                        //    outputWorksheet.Cells[outputRow, 17].Value = this.FormatObjectForOutput(itemToProcess.Payments);
                                        //    outputWorksheet.Cells[outputRow, 18].Value = this.FormatObjectForOutput(timeline.Placements.SelectMany(x => x.Value));
                                        //    outputWorksheet.Cells[outputRow, 19].Value = this.FormatObjectForOutput(itemToProcess.AlienStatus);
                                        //    outputWorksheet.Cells[outputRow, 20].Value = this.FormatObjectForOutput(itemToProcess.AssitanceGroupMembers);
                                        //}



                                        //}
                                    }
                                    row++;

                                } while (!pin.IsNullOrWhiteSpace());


                                Console.SetCursorPosition(0, Console.CursorTop);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }


                    Console.WriteLine("Keep Going?");
                    again = Console.ReadLine()?.ToLower() != "n";
                } while (again);
                var outputPath = this.GetOutputPath();
                outputWorksheet.Cells.AutoFitColumns();

                outputPackage.SaveAs(new FileInfo(Path.Combine(outputPath, "parsed_batch_output.xlsx")));
            }
        }

        private string FormatObjectForOutput<T>(T obj)
        {
            String output = null;
            var sb = new StringBuilder();

            if (obj is IEnumerable<Placement>)
            {
                var placements = ((IEnumerable<Placement>)obj).ToList().OrderByDescending(x => x.DateRange.End);
                sb.AppendLine("placements:");

                foreach (var placement in placements)
                {
                    sb.AppendLine(this.FormatObjectForOutput(placement));
                }
            }
            else if (obj is Placement)
            {
                var placement = obj as Placement;
                sb.AppendLine($"{placement.PlacementCode} : {placement.DateRange.Start:d}-{placement.DateRange.End:d} ");
            }
            else if (obj is IEnumerable<AlienStatus>)
            {
                var alienStatuses = ((IEnumerable<AlienStatus>)obj).ToList().OrderByDescending(x => x?.DateRange.End);
                sb.AppendLine("Alien status:");
                foreach (var alienStatus in alienStatuses)
                {
                    sb.AppendLine($"{alienStatus.AlienStatusCode} : {alienStatus.DateRange.Start:d}-{alienStatus.DateRange.End:d} ");
                }
            }
            else if (obj is IEnumerable<Payment>)
            {
                var payments = ((IEnumerable<Payment>)obj).OrderByDescending(x => x.EffectivePaymentMonth).ThenByDescending(x => x.PayPeriodEndDate);
                sb.AppendLine("payments:");
                foreach (var payment in payments)
                {
                    sb.AppendLine($"Payment Month:{payment.EffectivePaymentMonth.AddMonths(-1):MM-yyyy} Full Sanction: {payment.SanctionedToZero()}. AdjustedNetAmount: {payment.AdjustedNetAmount}. Pay Period Begin Date: {payment.PayPeriodBeginDate:d}. Pay Period End Date: {payment.PayPeriodEndDate:d} ");
                }
            }
            else if (obj is IEnumerable<AssistanceGroupMember>)
            {
                var agMembers = (obj as IEnumerable<AssistanceGroupMember>);
                sb.AppendLine("Parents:");
                foreach (var agMem in agMembers.Where(x => !x.IsChild()))
                {
                    sb.AppendLine($"Relationship: {agMem.RELATIONSHIP}. Age: {agMem.AGE}. pin: {agMem.PinNumber}. Spouse: {agMem.IsSpouse()}. Alien Statuses: {FormatObjectForOutput(agMem.AlienStatuses)}.");
                }

                sb.AppendLine("Children:");
                foreach (var agMem in agMembers.Where(x => x.IsChild()))
                {
                    sb.AppendLine($"Relationship: {agMem.RELATIONSHIP}. Age: {agMem.AGE}. pin: {agMem.PinNumber}. Spouse: {agMem.IsSpouse()}. Alien Statuses: {FormatObjectForOutput(agMem.AlienStatuses)}.");
                }
            }
            else if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                if (obj == null)
                {
                    sb.AppendLine($"[No Value]");
                }
                else
                {
                    var tail = this.FormatObjectForOutput(Convert.ChangeType(obj, Nullable.GetUnderlyingType(typeof(T))));
                    sb.AppendLine(tail);
                }
            }



            output = sb.ToString();
            return output.IsNullOrWhiteSpace() ? obj?.ToString() : output;
        }



        private async Task RunSimulatedBatchJob()
        {
            Boolean again = true;
            Int32 taskId = 0;

            while (again)
            {

                do
                {
                    Console.WriteLine(@"What type of run would you like make?
                                                --------------------------
                                                1. Full
                                                2. File / Enter pins

                            ");
                } while (!Int32.TryParse(Console.ReadLine(), out taskId));

                List<Decimal> pinsToProcess = new List<Decimal>();

                switch (taskId)
                {
                    case 1:
                        await this.GetPinsToProcess();
                        pinsToProcess = this.PinsToProcess.GetConsumingEnumerable().ToList();
                        break;
                    case 2:
                        pinsToProcess = this.GetIds<Decimal>();
                        break;
                }


                //var pinBatches = new Dictionary<Int32, List<Decimal>>();

                //if (pinsToProcess.Count > 500)
                //{
                //    Int32 batchSize;
                //    Console.WriteLine("Whoa! thats a lot! I'm gonna split these up into a couple runs for ya :)");

                //    Console.WriteLine("200?");
                //    if (!Int32.TryParse(Console.ReadLine(), out batchSize))
                //    {
                //        batchSize = 200;
                //    }


                //    pinBatches = pinsToProcess.Select((x, i) => new { Index = i, Value = x })
                //        .GroupBy(x => x.Index / batchSize)
                //        .ToDictionary(x => x.Key, x => x.Select(y => y.Value).ToList());
                //    Console.WriteLine($"created {pinBatches.Count} batches of size: {batchSize}?");

                //}
                //else
                //{
                //    pinBatches = new Dictionary<Int32, List<Decimal>>() { { 1, pinsToProcess } };
                //}

                var outputPath = this.GetOutputPath();

                //foreach (var pinBatch in pinBatches)
                //{
                //Console.WriteLine($"Processing batch {pinBatch.Key} / {pinBatches.Count}. Count: {pinBatch.Value.Count} ");
                //this.Context.inputPins.Clear();
                //foreach (var pin in pinBatch.Value)
                //{
                //    this.Context.inputPins.Add(pin);
                //}
                //await this.GetPinsToProcess().ConfigureAwait(false);

                //Console.WriteLine("Starting up producer/consumer queue");
                //var producerCompletedTask = this.StartProducerQueue();
                //await Task.WhenAll(producerCompletedTask, this._queue.Completion).ConfigureAwait(false);
                //this.PinsToProcess = new AsyncCollection<Decimal>();//so we can run again!

                ////Once complete, write the files output 
                //foreach (var kvps in this.taskOutput)
                //{
                //    var filePath = Path.Combine(path, $"{String.Join(".", FlagEnums.GetFlagMembers(kvps.Key).Select(x => x.Name))}.txt");
                //    File.WriteAllText(
                //        filePath
                //        , kvps.Value.ToString());
                //}

                this.CreateProcessingQueue();


                await this.RunActionByPartitions(pinsToProcess, async (pin) =>
                 {


                     //foreach (var pin in pinBatch.Value)
                     //{
                     try
                     {
                         var itemToProcess = await this.GetQueueItemAsync(pin).ConfigureAwait(false);
                         if (itemToProcess.Status.GetValueOrDefault() == JobStatus.ReadyForJobProcessing)
                         {
                             await this.ProcessItem(itemToProcess).ConfigureAwait(false);
                         }
                     }
                     catch (Exception e)
                     {
                         Console.WriteLine($"Failed to process pin: {pin}");
                         Console.WriteLine(e.Message);
                     }
                     //}
                 });

                var writeOutput = true;
                //Console.WriteLine("Write output file(s)? (y/n):");
                //writeOutput = Console.ReadLine().Trim().ToLower().Equals("y");
                if (writeOutput)
                {
                    Console.WriteLine("Writing Results.xlsx output...");

                    Console.WriteLine("Compare against legacy batch? (y/n):");
                    var generateCompare = false;// Console.ReadLine().Trim().ToLower().Equals("y");

                    String comparisionFile;

                    if (generateCompare)
                    {
                        comparisionFile = await this.CreateExpectedOutputFromLegacyBatch(pinsToProcess, null).ConfigureAwait(false);
                    }
                    else
                    {
                        Console.WriteLine("Expected results spreedsheet? (leave blank to skip comparing):");
                        comparisionFile = "";//Console.ReadLine();
                    }


                    var outputPackage = new ExcelPackage();
                    if (!String.IsNullOrEmpty(comparisionFile))
                    {
                        this.ParseExpectedOutput(comparisionFile, true);

                        using (var file = File.Open(comparisionFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            outputPackage.Load(file);
                        }
                    }

                    var rowNum = 2;

                    if (String.IsNullOrEmpty(comparisionFile))
                    {
                        var resultSheet = outputPackage.Workbook.Worksheets.Add("Results");
                        foreach (var kvp in this.pinOutput)
                        {
                            this.WriteOutputForPin(kvp.Key, kvp.Value, resultSheet, rowNum);
                            rowNum++;
                        }
                    }
                    else
                    {
                        var resultSheet = outputPackage.Workbook.Worksheets[1];
                        while (!resultSheet.Cells[rowNum, 1].GetValue<String>().IsNullOrWhiteSpace())
                        {

                            var pin = resultSheet.Cells[rowNum, 1].GetValue<String>();
                            if (pin.IsNullOrWhiteSpace())
                            {
                                continue;
                            }

                            Decimal dPin;
                            if (Decimal.TryParse(pin, out dPin))
                            {
                                RuleContext context;
                                if (this.pinOutput.TryGetValue(dPin, out context))
                                {
                                    this.WriteOutputForPin(dPin, context, resultSheet, rowNum);
                                }
                            }
                            rowNum++;
                        }
                    }
                    //get row with pin number

                    outputPackage.SaveAs(new FileInfo(Path.Combine(outputPath, ($"{ApplicationContext.AppEnvironment}_results_{this.Context.Date:MMMM-yyyy}.xlsx").ToLower())));
                }

                Console.WriteLine("Writing Results completed...");
                //Console.WriteLine("Keep Going?");
                //again = Console.ReadLine()?.ToLower() != "n";
                //if (!again)
                //{
                //    Console.WriteLine("Quitting...");
                //    break;
                //}







                Console.WriteLine("Run again(y/n):");
                again = Console.ReadLine()?.ToLower() != "n";
            }
        }

        private void WriteOutputForPin(Decimal dPin, RuleContext context, ExcelWorksheet resultSheet, Int32 rowNum)
        {

            ClockTypes actualResult;
            // Find the matching pin in the results
            this.pinOutput.TryGetValue(dPin, out context);
            actualResult = context == null ? ClockTypes.None : context.TimelimitType.GetValueOrDefault();

            ClockTypes expectedClockType = this.expectedPinOutput.Where(kvp => kvp.Value != null && kvp.Value.Contains(dPin)).Select(x => x.Key).FirstOrDefault();

            if (expectedClockType.CommonFlags(ClockTypes.TEMP) == ClockTypes.TEMP)
            {
                var hasTemp = actualResult.HasAnyFlags(ClockTypes.TEMP);
                var hasAllOtherFlags = expectedClockType.RemoveFlags(ClockTypes.TEMP).HasAllFlags(actualResult.RemoveFlags(ClockTypes.TEMP));
                var passed = hasTemp && hasAllOtherFlags;
                resultSheet.Cells[rowNum, 4].Value = passed ? "P" : "F";
            }
            else if (expectedClockType == actualResult)
            {
                resultSheet.Cells[rowNum, 4].Value = "P";
            }
            else
            {
                resultSheet.Cells[rowNum, 4].Value = "F";
            }

            resultSheet.Cells[rowNum, 3].Value = FlagEnums.FormatFlags(actualResult);

            resultSheet.Cells[rowNum, 1].Value = dPin;

            if (resultSheet.Cells[rowNum, 4].GetValue<String>() == "F")
            {
                resultSheet.Cells[rowNum, 4].Style.Font.Bold = true;
                resultSheet.Cells[rowNum, 4].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(100, 244, 67, 54));
                resultSheet.Cells[rowNum, 1].Style.Font.Bold = true;
                resultSheet.Cells[rowNum, 1].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(100, 244, 67, 54));
            }
            else
            {
                resultSheet.Cells[rowNum, 4].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(100, 3, 169, 244));
                resultSheet.Cells[rowNum, 1].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(100, 3, 169, 244));
            }
        }

        public async Task<String> CreateExpectedOutputFromLegacyBatch(List<Decimal> pinBatchValue = null, Int32? batchRun = null)
        {
            var dDate = Decimal.Parse(this.Context.Date.ToStringMonthYearComposite());

            this._logger.Information($"Getting legacy ticks for date: {this.Context.Date:MMMM - yyyy}");
            List<T0459_IN_W2_LIMITS> legacyticks;
            using (var dbContext = this.Container.Resolve<WwpEntities>())
            {
                var query = dbContext.T0459_IN_W2_LIMITS.Where(x => x.BENEFIT_MM == dDate && x.CRE_TRAN_CD.Trim() == "PWCAEP11");
                if (pinBatchValue?.Any() == true)
                {
                    query = query.Where(x => pinBatchValue.Contains(x.PIN_NUM));
                }
                legacyticks = await query.OrderBy(x => x.CLOCK_TYPE_CD).ThenBy(x => x.PIN_NUM).ToListAsync().ConfigureAwait(false);

            }
            this._logger.Information($"Found {legacyticks.Count} legacy ticks for date: {this.Context.Date:MMMM - yyyy}");



            var excelPackage = new ExcelPackage();
            var row = 3;

            using (var file = File.Open("results_spreadsheet_template.xlsx", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                excelPackage.Load(file);
            }
            var worksheet = excelPackage.Workbook.Worksheets[1];

            this._logger.Information("Processing...");

            for (var index = 0; index < legacyticks.Count; index++)
            {
                Console.Write($"Proccessing item {index + 1} / {legacyticks.Count}");
                var tick = legacyticks[index];

                worksheet.Cells[row, 1].Value = tick.PIN_NUM;
                ClockTypes clockType;
                Enum.TryParse(tick.CLOCK_TYPE_CD, out clockType);
                clockType = clockType.CombineFlags(ClockTypes.State);
                if (tick.FED_CLOCK_IND == "Y")
                {
                    clockType = clockType.CombineFlags(ClockTypes.Federal);
                }

                worksheet.Cells[row, 2].Value = FlagEnums.FormatFlags(clockType);
                Console.SetCursorPosition(0, Console.CursorTop);
                row++;
            }
            this._logger.Information("Writing ouptut...");

            //var outputPath = this.GetOutputPath();
            var fileName = "output\\Production_results_temp_spreadsheet_template.xlsx";//Path.Combine(outputPath, ApplicationContext.AppEnvironment + "_results_"+batchRun.GetValueOrDefault(1)+"_spreadsheet_template.xlsx");
            excelPackage.SaveAs(new FileInfo(fileName));
            this._logger.Information("Writing Complete!");
            return fileName;

        }

        public void ParseExpectedOutput(String comparisonFile, Boolean overwrite)
        {
            if (!overwrite && this.expectedPinOutput.Count > 0)
            {
                return;
            }
            // Load expected results
            ExcelPackage expcetedResultsExcelPackage = new ExcelPackage();
            using (var file = File.Open(comparisonFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                expcetedResultsExcelPackage.Load(file);
            }
            var worksheet = expcetedResultsExcelPackage.Workbook.Worksheets[1];
            var colNum = 1;
            var rowVal = 3;

            while (true)
            {
                var sPin = worksheet.Cells[rowVal, 1]?.Value?.ToString();
                var sClockTypes = worksheet.Cells[rowVal, 2]?.Value?.ToString();
                if (sPin.IsNullOrWhiteSpace() || sClockTypes.IsNullOrWhiteSpace())
                {
                    break;
                }

                Decimal pinNumber;
                ClockTypes clockTypes;
                try
                {
                    clockTypes = FlagEnums.ParseFlags<ClockTypes>(sClockTypes);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Couldn't clock type: {sPin}. Cell[{ worksheet.Cells[rowVal, 1].Address}]");
                    continue;
                }

                try
                {
                    pinNumber = Decimal.Parse(sPin);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Couldn't parse pin number: {sPin}. Cell[{ worksheet.Cells[rowVal, 1].Address}]");
                    continue;
                }

                this.expectedPinOutput.AddOrUpdate(clockTypes, new List<Decimal>() { pinNumber }, (k, v) =>
                                {
                                    v.Add(pinNumber);
                                    return v;
                                });
                rowVal++;
            }

            //while (true)
            //{
            //    var headerCell = worksheet.Cells[headerRow.Row, colNum];
            //    var sClockTypes = headerCell.GetValue<String>();
            //    if (sClockTypes.IsNullOrWhiteSpace())
            //    {
            //        break;
            //    }

            //    var clockTypes = FlagEnums.ParseFlags<ClockTypes>(sClockTypes);

            //    var rowVal = 2;
            //    while (true)
            //    {

            //        var cell = worksheet.Cells[rowVal, colNum];
            //        if (cell.GetValue<String>().IsNullOrWhiteSpace())
            //        {
            //            break;
            //        }
            //        var cellValues = (cell.GetValue<String>() ?? "").Split(',', ';', '/');
            //        foreach (var cellValue in cellValues)
            //        {

            //            decimal pinNumber;
            //            if (Decimal.TryParse(cellValue, out pinNumber))
            //            {
            //                this.expectedPinOutput.AddOrUpdate(clockTypes, new List<Decimal>() { pinNumber }, (k, v) =>
            //                {
            //                    v.Add(pinNumber);
            //                    return v;
            //                });
            //            }
            //            else
            //            {
            //                Console.WriteLine($"Couldn't parse pin number: {cellValue}. Cell[{cell.Address}]");
            //            }
            //        }

            //        rowVal++;
            //    }
            //    colNum++;
            //}
        }

        //private async Task CleanTimelimitsTask()
        //{
        //    List<TimeLimit> timelimitsToClean = new List<TimeLimit>();
        //    DateTime monthToClean;
        //    do
        //    {
        //        Console.WriteLine("What month would you like to clean? MM/yyyy:");
        //    } while (!DateTime.TryParseExact(Console.ReadLine(), "MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out monthToClean));


        //    Console.WriteLine("Clean All?");
        //    var cleanAll = Console.ReadLine()?.ToLower() != "n";

        //    using (var dbContext = this.dbContextFactory())
        //    {
        //        if (!cleanAll)
        //        {
        //            var pinsToClean = this.GetIds<Int32>();
        //            timelimitsToClean = await dbContext.TimeLimits.Where(a => pinsToClean.Contains(a.ParticipantID.GetValueOrDefault()) && a.EffectiveMonth.HasValue && DbFunctions.DiffMonths(a.EffectiveMonth.Value, monthToClean) == 0).ToListAsync(this._token);
        //        }
        //        else
        //        {
        //            timelimitsToClean = await dbContext.TimeLimits.Where(a => !a.IsDeleted && a.EffectiveMonth.HasValue && DbFunctions.DiffMonths(a.EffectiveMonth.Value, monthToClean) == 0).ToListAsync(this._token);
        //        }

        //        var pins = timelimitsToClean.Select(x => x.ParticipantID).Distinct();
        //        Console.WriteLine($"Found {pins.Count()} pins and {timelimitsToClean.Count}");

        //        Console.WriteLine($"Confirm Delete of {timelimitsToClean.Count} records?(y/n)");
        //        var shouldDelete = Console.ReadLine().Trim().ToLower() == "y";

        //        if (shouldDelete)
        //        {
        //            foreach (var model in timelimitsToClean)
        //            {
        //                model.IsDeleted = true;
        //            }

        //            await dbContext.SaveChangesAsync(this._token);
        //        }
        //    }
        //    Console.WriteLine($"Deleted of {timelimitsToClean.Count} records");
        //}


        private List<Int64> GetIdsFromFile()
        {
            Boolean again = false;
            List<Int64> ids = new List<Int64>();
            var filePath = "";

            do
            {
                // add pins to the queue
                do
                {
                    Console.WriteLine(@"What file would you like to run (pins.xlsx)?:");
                    filePath = Console.ReadLine().Trim('\"');
                    filePath = String.IsNullOrWhiteSpace(filePath) ? "pins.xlsx" : filePath;
                } while (!File.Exists(filePath) || (Path.GetExtension(filePath) != ".xlsx" && Path.GetExtension(filePath) != ".txt"));

                try
                {
                    if ((Path.GetExtension(filePath) == ".xlsx"))
                    {
                        ExcelPackage idsPackage = new ExcelPackage();
                        try
                        {
                            using (var file = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                            {
                                idsPackage.Load(file);
                            }
                        }
                        catch (System.IO.IOException)
                        {
                            Console.WriteLine(@"File locked....");
                            again = true;
                            continue;
                        }

                        var worksheet = idsPackage.Workbook.Worksheets[1];
                        var row = 1;

                        var parseErrorCount = 0;
                        string cellValue = null;
                        do
                        {

                            cellValue = worksheet.Cells[row, 1].GetValue<string>();
                            Int64 currentId = 0;
                            if (Int64.TryParse(cellValue, out currentId))
                            {
                                ids.Add(currentId);
                                parseErrorCount = 0;
                            }
                            else
                            {
                                parseErrorCount++;
                            }
                            row++;
                        } while (parseErrorCount < 5);


                    }
                    else
                    {
                        var lines = File.ReadAllLines(filePath);
                        foreach (var line in lines)
                        {
                            var sPins = line.Trim().Replace(" ", "").Split(',', ';', '/');
                            foreach (var sPin in sPins)
                            {
                                Int64 iPin;
                                if (Int64.TryParse(sPin, out iPin))
                                {
                                    ids.Add(iPin);
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    this._logger.Error(e, $"Error trying to process file: {filePath}");
                    Console.WriteLine("Run again(y/n):");
                    again = Console.ReadLine()?.ToLower() != "n";
                }
                this._logger.Information("Found {count} Ids", ids.Count);

            } while (again);
            return ids;

        }

        private List<Int64> GetIdsFromConsole()
        {
            var ids = new List<Int64>();
            Console.WriteLine("Enter Id's numbers(',' seperated)?:");
            var pinsStrings = Console.ReadLine().Replace(" ", "").Split(',');
            foreach (var sId in pinsStrings)
            {
                Int64 id;
                if (Int64.TryParse(sId, out id))
                {
                    ids.Add(id);
                }
            }
            return ids;
        }

        public List<T> GetIds<T>() where T : IConvertible
        {
            var taskId = 0;
            do
            {
                Console.WriteLine(@"What type of run would you like?
                                    --------------------------
                                    1. File
                                    2. Enter Pin(s)
                ");
            } while (!Int32.TryParse(Console.ReadLine(), out taskId));


            List<T> pinsToProcess;
            if (taskId == 1)
            {
                pinsToProcess = this.GetIdsFromFile().Select(x => (T)Convert.ChangeType(x, typeof(T))).ToList();

            }
            else
            {
                pinsToProcess = this.GetIdsFromConsole().Select(x => (T)Convert.ChangeType(x, typeof(T))).ToList();
            }
            return pinsToProcess;
        }

        protected override Task CreateJobAsync(IBatchTask task)
        {
            task.ExternalJobId = Guid.NewGuid().ToString("N");
            task.JobId = new Random().Next(100000, Int32.MaxValue);
            return Task.CompletedTask;
        }
        protected override Task UpdateJobStatus(EvaluateTimelimitsTaskContext JobId, JobStatus status)
        {
            //TODO: Just log to console if we a simulating
            this._logger.Debug($"Completed job [{JobId}] with status: {status.ToString()}");
            return Task.CompletedTask;
        }

        protected override void LogJobAsProccessed(IBatchTask queuedItem, IEnumerable<EvaluateTimelimitsTaskResult> results, TimeSpan stopWatchElapsed)
        {

            foreach (var result in results)
            {
                this.pinOutput.TryAdd(result.PinNumber, result.RuleContext);
                this.taskOutput.AddOrUpdate(result.RuleContext.TimelimitType.GetValueOrDefault(), new StringBuilder(result.PinNumber.ToString() + "\n"), (c, s) =>
                  {
                      s.AppendLine(result.PinNumber.ToString());
                      return s;
                  });

                foreach (var opcTick in result.OtherParentData)
                {
                    this.pinOutput.TryAdd(opcTick.parent.PinNumber.GetValueOrDefault(), new RuleContext() { TimelimitType = opcTick.TimelimitType, EvaluationMonth = result.RuleContext.EvaluationMonth, IsAlien = result.RuleContext.IsAlien, PaymentsAreFullySanctioned = result.RuleContext.PaymentsAreFullySanctioned, ShouldCreateOpcTicks = result.RuleContext.ShouldCreateOpcTicks });
                    this.taskOutput.AddOrUpdate(opcTick.TimelimitType, new StringBuilder(opcTick.parent.PinNumber.ToString() + Environment.NewLine), (c, s) =>
                      {
                          s.AppendLine(opcTick.parent.PinNumber.ToString());
                          return s;
                      });
                }
            }
        }

        private string GetOutputPath()
        {
            var index = 1;

            if (!Directory.Exists("output"))
            {
                Directory.CreateDirectory("output");
            }

            var outputDirList = Directory.GetDirectories("output", "*", SearchOption.TopDirectoryOnly);
            if (outputDirList.Any())
            {
                var outputDirIndexs = outputDirList.Select(x =>
                {
                    Int32 i = 0;
                    var sIndex = x.TrimEnd().Split('-').Last();
                    Int32.TryParse(sIndex, out i);
                    return i;
                }).ToList();

                var maxIndex = outputDirIndexs.Any() ? outputDirIndexs.Max() : 0;

                // TODO: Archive old one's

                index = maxIndex + 1;
            }

            var di = Directory.CreateDirectory($"output/run-{index}");
            return di.FullName;

        }
    }

    public class JsonDbConnection
    {
        public String server { get; set; }
        public String instance { get; set; }
        public String user { get; set; }
        public String auth { get; set; }
    }
}