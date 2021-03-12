using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Autofac;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Services;
using DCF.Common.Exceptions;
using DCF.Common.Extensions;
using DCF.Common.Tasks;
using DCF.Core.IO;
using DCF.Timelimits.Core.Processors;
using DCF.Timelimits.Core.Tasks;
using DCF.Timelimits.Rules.Definitions;
using DCF.Timelimits.Rules.Domain;
using DCF.Timelimits.Tasks;
using MediatR;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

namespace DCF.Timelimits
{
    public abstract class BatchApplication
    {
        public Int64 FailedProccessingJobsSinceStart { get; set; }
        public Int64 JobsProcessedSinceStart { get; set; }
        public TimeSpan AverageJobProcessingTime { get; set; }

        public IContainer Container { get; set; }
        protected Boolean _isRunning;
        protected Boolean _isPaused;
        protected CancellationToken _token;
        private CancellationTokenSource _tokenSource { get; }
        protected SemaphoreSlim GetQueueItemAsyncThrottle { get; set; }

        public ApplicationContext Context { get; }
        protected ILogger _logger;

        protected Int64 _sleepCount = 0;

        public Action<ContainerBuilder> OnContainerInitialized { get; set; }
        public Func<IContainer, Task> OnInitialized { get; set; }
        public Action OnPaused { get; set; }
        public Action OnStarted { get; set; }
        public Action OnStopped { get; set; }

        protected IJobQueue JobQueue { get; set; }
        protected IJobQueueService JobQueueService { get; set; }
        protected ITaskMediator mediator;
        private MemoryStream _loggingConfigstream;

        protected ExecutionDataflowBlockOptions DataFlowOptions { get; set; }

        protected BatchApplication(ApplicationContext context)
        {
            this.Context = context;
            if (this.Context.ApplicationInstanceName.IsNullOrWhiteSpace())
            {
                this.Context.ApplicationInstanceName = this.GetType().AssemblyQualifiedName;
            }
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            this._tokenSource = new CancellationTokenSource();
            this._token = this._tokenSource.Token;
            this._loggingConfigstream = new MemoryStream();
        }

        public abstract void CreateProcessingQueue();

        internal abstract Task StartProducerQueue();

        internal abstract void WriteJobOutput();

        protected ILogger ResolveLogger()
        {
            //var logPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, this.Context.LogPath, $"/{this.Context.ApplicationInstanceName}/"));
            //var logPath = Path.Combine(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, this.Context.LogPath)), $"{this.Context.ApplicationInstanceName}\\");
            //var logPath = Path.Combine(this.Context.LogPath, $"{this.Context.ApplicationInstanceName}\\");
            var logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this.Context.LogPath, $"{this.Context.ApplicationInstanceName}\\");
        
            var consoleLevel = System.Diagnostics.Debugger.IsAttached ? LogEventLevel.Verbose : LogEventLevel.Debug;
            const String outputTemplate = "{Timestamp} [{Level}] {ApplicationName}-v{Version}-{Partition} {Pin} {NewLine} {Message}{NewLine}{Exception}";
            var logPath1 = Path.GetFullPath(Path.Combine(logPath, "failures\\logs-{Date}-{Version}.txt"));
            var logPath2 = Path.GetFullPath(Path.Combine(logPath, "logs-{Date}-{Version}.txt"));
            try
            {
                DirectoryHelper.CreateIfNotExists(Path.GetDirectoryName(logPath1));
                DirectoryHelper.CreateIfNotExists(Path.GetDirectoryName(logPath2));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("[Error] - error creating log path for job(s).");
                Console.WriteLine("[path1] - ." + logPath1);
                Console.WriteLine("[path1] - ." + logPath2);

                logPath1 = "logs-failures-{Date}-{Version}.txt";
                logPath2 = "logs-{Date}-{Version}.txt";
            }

            Console.WriteLine($"logPath 1: {logPath1}");
            Console.WriteLine($"logPath 1: {logPath2}");
            //var file = File.CreateText("LoggingSetupConfigFile.txt");
            //Serilog.Debugging.SelfLog.Enable(TextWriter.Synchronized(file));

            var connectionString = WwpEntities.CreateSqlConnectionString(Context.DatabaseConfig.Server, Context.DatabaseConfig.Catalog, Context.DatabaseConfig.UserId, Context.DatabaseConfig.Password, "WWP-Logging", Context.DatabaseConfig.MaxPoolSize);

            var columnOptions = new ColumnOptions();
            columnOptions.Store.Add(StandardColumn.LogEvent);
            //columnOptions.LogEvent.ExcludeAdditionalProperties = true;//don't store redundant
            columnOptions.Properties.UsePropertyKeyAsElementName = true;
            columnOptions.Level.StoreAsEnum = true;

            return this._logger = new LoggerConfiguration()
                .WriteTo.Console(consoleLevel, outputTemplate: outputTemplate)
                .WriteTo.Async(a => a.RollingFile(logPath1, LogEventLevel.Warning, outputTemplate: outputTemplate))
                .WriteTo.Async(a => a.RollingFile(logPath2, LogEventLevel.Verbose, outputTemplate: outputTemplate))
                .WriteTo.MSSqlServer(connectionString, "LogEvent", columnOptions: columnOptions, schemaName: "wwp", restrictedToMinimumLevel: LogEventLevel.Error)
                //.WriteTo.MSSqlServer(connectionString, "LogEvent", columnOptions: columnOptions, schemaName: "wwp")
                .Destructure.ToMaximumDepth(15)
                .Enrich.WithMachineName()
                .Enrich.WithProcessName()
                .Enrich.WithProperty("ApplicationName", this.Context.ApplicationInstanceName)
                .Enrich.WithProperty("Partition", this.Context.JobQueuePartion)
                .Enrich.WithProperty("Version", this.Context.ApplicationVersion)
                .Enrich.FromLogContext()
                .CreateLogger();
        }

        /// <summary>
        /// Helper method to perform a controlled "sleep". allowing interruption 
        /// if the process is bieng shutdown or the task is cancelled to exit the sleep cycle.
        /// Will call the periodicCallback and block on the periodic callback every few seconds
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <param name="periodicCallback"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        protected async Task SleepAsync(Int64 milliseconds = 500, Func<CancellationToken, Task> periodicCallback = null, CancellationToken token = default(CancellationToken))
        {
            // For a long running sleep, we want to calculate approximitely when it will end,
            // because our callback may have some overhead
            DateTime waitUntil = DateTime.UtcNow - TimeSpan.FromMilliseconds(milliseconds);
            if (this._sleepCount >= Int32.MaxValue)
            {
                this._logger.Information("Sleep count reset.");
                this._sleepCount = 0;
            }


            Int64 sleepIterationCount = 0;

            this._logger.Debug($"Sleeping #{this._sleepCount:N} - Sleeping until: {waitUntil.ToLocalTime():F} ");
            while (!token.IsCancellationRequested && waitUntil > DateTime.UtcNow)
            {
                await Task.Delay(500, token).ConfigureAwait(false);
                sleepIterationCount++;

                if (!this._isRunning)
                {
                    return;
                }

                if (periodicCallback != null && sleepIterationCount % 10 == 0)
                {
                    try
                    {
                        await periodicCallback(token).ConfigureAwait(false); //TODO: Should we wait for this or fire and forget?
                    }
                    catch (Exception e)
                    {
                        this._logger.Error(e, "Periodic callback failed during sleep");
                    }
                }
            }
            this._logger.Debug($"Sleeping #{this._sleepCount:N} - Sleeping done: {DateTime.UtcNow.ToLocalTime():F} ");

        }

        public async Task InitializeAsync(ContainerBuilder containerBuilder)
        {
            this._logger = this.ResolveLogger();

            using (this._logger.BeginTimedOperation("Initializing Application"))
            {
                this._logger.BeginTimedOperation($"Running in simulation mode: {this.Context.IsSimulation} ");
                this._logger.BeginTimedOperation($"Running in with Context Date: {this.Context.Date:G}");
                containerBuilder.RegisterInstance(this._logger).As<ILogger>().SingleInstance();

                this._logger.Information("Building Container.");

                this.OnContainerInitialized?.Invoke(containerBuilder);

                this.Container = containerBuilder.Build();

                this.mediator = this.Container.Resolve<ITaskMediator>();

                this._logger.Information("Getting JobQueue.");

                this.JobQueueService = this.Container.Resolve<IJobQueueService>();

                this.JobQueue = await this.JobQueueService.GetJobQueueAsync(this.Context.JobQueueName, this._token).ConfigureAwait(false);
                
                if (JobQueue == null)
                {
                    throw new DCFApplicationInitializationException($"Application Initialization Error: Failed to get Job Queue with Queue Name: {this.Context.JobQueueName}.");
                }

                this._logger.Information("Getting JobQueue Complete!");

                if (this.OnInitialized != null)
                {
                    await this.OnInitialized(this.Container).ConfigureAwait(false);
                }

                this.GetQueueItemAsyncThrottle = new SemaphoreSlim(this.Context.MaxDegreeOfParallelism, this.Context.MaxDegreeOfParallelism);

                this.CreateProcessingQueue();

                this._logger.Information("Application initilization completed.");
            }

        }

        public async Task<Int32> RunAsync()
        {
            var exitCode = 0;
            try
            {
                await this.StartAsync();
                this.Stop();
                if (this.FailedProccessingJobsSinceStart == 0)
                {
                    exitCode = 0;
                }
                else
                {
                    exitCode = this.FailedProccessingJobsSinceStart > 100 ? 999 : (Int32)this.FailedProccessingJobsSinceStart;
                }
            }
            catch (Exception e)
            {
                exitCode = this.StopWithError(e);
            }
            this.CompleteBatchRun();
            return exitCode;
        }

        public abstract Task StartAsync();

        private Int32 StopWithError(Exception exception)
        {
            this.Stop(true);
            return 999;
        }

        public void Stop(Boolean throwOnFirstException = false)
        {
            try
            {
                if (this._token.CanBeCanceled)
                {
                    this._tokenSource.Cancel(throwOnFirstException);
                }
                Log.CloseAndFlush();
            }
            catch (Exception e)
            {
                //don't do anything, we can't throw an exception here
            }
            this._isRunning = false;
            this.OnStopped?.Invoke();
        }

        public void Pause()
        {
            this.OnPaused?.Invoke();
            this._isPaused = true;
        }

        private void TaskScheduler_UnobservedTaskException(Object sender, UnobservedTaskExceptionEventArgs e)
        {
            if (e == null) return;
            try
            {
                this._logger.Error("RTE-10: Unhandled exeption caught by TaskScheduler. Please notify engineering of any (RTE-100) following errors");

                if (e.Exception != null)
                    foreach (var exception in e.Exception.Flatten().InnerExceptions)
                    {
                        if (exception != null)
                        {
                            this._logger.Error(exception, "RTE-100: Unhandled exeption caught by TaskScheduler:");
                        }
                    }
            }
            catch (Exception)
            {
                //this is a terrible state, dunno what to do but we prevent crashing the service.        
            }

            e.SetObserved();
        }

        protected virtual void CompleteBatchRun()
        {
            try
            {
                this.WriteJobOutput();
            }
            catch (Exception e)
            {
                this._logger.Error(e,"Error Writing Job Output");
            }
        }

        //private string GetOutputPath()
        //{
        //    //  TODO: Archive old one's
        //    return Path.Combine(this.Context.OutputPath,this.Context.ApplicationInstanceName)

        //    //var index = 1;

        //    //if (!Directory.Exists("output"))
        //    //{
        //    //    Directory.CreateDirectory("output");
        //    //}

        //    //var outputDirList = Directory.GetDirectories("output", "*", SearchOption.TopDirectoryOnly);
        //    //if (outputDirList.Any())
        //    //{
        //    //    var outputDirIndexs = outputDirList.Select(x =>
        //    //    {
        //    //        Int32 i = 0;
        //    //        var sIndex = x.TrimEnd().Split('-').Last();
        //    //        Int32.TryParse(sIndex, out i);
        //    //        return i;
        //    //    }).ToList();

        //    //    var maxIndex = outputDirIndexs.Any() ? outputDirIndexs.Max() : 0;


        //    //    index = maxIndex + 1;
        //    //}

        //    //var di = Directory.CreateDirectory($"output/run-{index}");
        //    //return di.FullName;

        //}
    }

    public abstract class BatchApplication<T, TTask, TResult> : BatchApplication where TTask : IBatchTask<TResult> where TResult : IBatchTaskResult
    {

        protected ConcurrentDictionary<T, TResult> appOutput = new ConcurrentDictionary<T, TResult>();

        protected ActionBlock<TTask> _queue;
        protected Random Random { get; } = new Random();


        protected BatchApplication(ApplicationContext context) : base(context)
        {

        }

        protected virtual async Task<TResult> ProcessItemAsync(TTask queuedItem)
        {
            Boolean success = false;
            TResult jobResult = default(TResult);
            using (this._logger.BeginTimedOperation($"{this.JobQueue?.Name ?? "Unknown"} Job Proccesing", queuedItem.GetItemIdentifier()))
            {
                var stopWatch = new Stopwatch();

                try
                {
                    stopWatch.Start();
                    await this.UpdateJobStatusAsync(queuedItem, JobStatus.JobIsProcessing).ConfigureAwait(false);
                    jobResult = (await this.ProcessQueuedTaskAsync(queuedItem).ConfigureAwait(false));
                    queuedItem.Result = jobResult;
                    //queuedItem.Status = jobResult.Status;

                    stopWatch.Stop();
                    this.LogJobAsProccessed(queuedItem, jobResult, stopWatch.Elapsed);
                }
                catch (Exception e)
                {
                    this._logger.Error(e, $"Error processing job: {queuedItem.JobId}");

                    if (jobResult != null)
                    {
                        jobResult.Status = JobStatus.JobProcessingFailure;
                        jobResult.Errors.Add(e);
                    }

                    this.LogJobAsProccessed(queuedItem, jobResult, stopWatch.Elapsed);
                }
                finally
                {
                    stopWatch.Stop();
                }
                await this.UpdateJobStatusAsync(queuedItem, jobResult?.Status ?? JobStatus.JobProcessingFailure).ConfigureAwait(false);
                return jobResult;
            }
        }

        protected async Task<TResult> ProcessQueuedTaskAsync(TTask queuedItem)
        {
            var result = await this.mediator.Send<TResult>(queuedItem).ConfigureAwait(false);
            return result;
        }


        internal abstract Task<TTask> GetQueueItemAsync(T id);




        protected virtual void LogJobAsProccessed(TTask queuedItem, TResult results, TimeSpan stopWatchElapsed)
        {
            this.JobsProcessedSinceStart++;
            if (queuedItem.Status == JobStatus.JobProcessingFailure)
                this.FailedProccessingJobsSinceStart++;
            this.AverageJobProcessingTime = TimeSpan.FromTicks(this.AverageJobProcessingTime.Ticks / Math.Min(this.JobsProcessedSinceStart, 1));
            this._logger.Information("Job {jobId} process with status: {status} in {Elapsed:000} ms", queuedItem.JobId, queuedItem.Status, stopWatchElapsed.TotalMilliseconds);
        }

        internal abstract override void WriteJobOutput();

        public override async Task StartAsync()
        {
            this._isRunning = true;
            this._isPaused = false;
            using (this._logger.BeginTimedOperation("Total Processing Time", "TOT Time"))
            {
                var producerTask = this.StartProducerQueue();
                this.OnStarted?.Invoke();
                await Task.WhenAll(producerTask, this._queue.Completion);
            }
        }



        protected virtual async Task UpdateJobStatusAsync(TTask job, JobStatus status)
        {
            if (this.Context.IsSimulation)
            {
                job.Status = status;
            }
            else
            {
                using (var jobService = Container.Resolve<IJobQueueService>())
                {
                    await jobService.UpdateJobAsync(job, status, false, this._token);
                }
            }

        }


        protected virtual async Task CreateJobAsync(TTask job)
        {
            if (this.Context.IsSimulation)
            {
                job.JobId = this.Random.Next(1000, 99999999);
                job.Status = JobStatus.CreatingJobForProcessing;
            }
            else
            {
                using (var jobService = Container.Resolve<IJobQueueService>())
                {
                    await jobService.CreateJobAsync(job, this.JobQueue, this.Context.ApplicationInstanceName, this._token);
                }
            }
        }



    }
}
