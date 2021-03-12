using DCF.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Reflection;
using DCF.Common;
using DCF.Common.Configuration;
using DCF.Common.Dates;
using DCF.Common.Exceptions;
using DCF.Common.Logging;
using DCF.Core;

namespace DCF.Timelimits.Rules.Domain
{
    public class ApplicationContext
    {
        public static ApplicationContext Current { get; }
        private static ApplicationEnvironment? _appEnvironment;
        public static bool IsInTransitionPeriod { get; private set; }

        public static ApplicationEnvironment AppEnvironment
        {
            get
            {
                if (!ApplicationContext._appEnvironment.HasValue)
                {
                    var dbName = Environment.GetEnvironmentVariable("WWP_DB_NAME");

                    if (dbName.IsNullOrEmpty() || Environment.UserInteractive || System.Diagnostics.Debugger.IsAttached)
                    {
                        ApplicationContext._appEnvironment = ApplicationEnvironment.Development;
                    }
                    else
                    {
                        // see what DB we are pointed at and guess the environment

                        switch (dbName.ToLower())
                        {
                            case "WWPDEV":
                                ApplicationContext._appEnvironment = ApplicationEnvironment.Development;
                                break;
                            case "WWPSYS":
                                ApplicationContext._appEnvironment = ApplicationEnvironment.Systems;
                                break;
                            case "WWPACC":
                                ApplicationContext._appEnvironment = ApplicationEnvironment.ACC;
                                break;
                            case "WWPTRN":
                                ApplicationContext._appEnvironment = ApplicationEnvironment.Training;
                                break;
                            default:
                                ApplicationContext._appEnvironment = ApplicationEnvironment.Production;
                                break;
                        }
                    }
                }

                return ApplicationContext._appEnvironment.Value;
            }
            set { ApplicationContext._appEnvironment = value; }
        }


        static ApplicationContext()
        {
            ApplicationContext.Current = new ApplicationContext();
        }

        private ApplicationContext()
            : this(DateTime.Now)
        {
        }

        /// <summary>
        /// Creates a new instance of an Application Context. Don't USE except for tests!
        /// </summary>
        /// <param name="currentDate"></param>
        internal ApplicationContext(DateTime currentDate)
        {
            this.Date = currentDate;
            try
            {
                this.ApplicationVersion = this.GetType().Assembly.GetName().Version;
            }
            catch (Exception e)
            {
            }
        }

        public DateTime _date;

        public DateTime Date
        {
            get { return this._date; }
            set
            {
                if (value.Day == 1)
                {
                    this._date = value.Date.AddMonths(-1).EndOf(DateTimeUnit.Month);
                }
                else
                {
                    this._date = value;
                }
                
            }
        }

        public List<Decimal> inputPins { get; set; } = new List<Decimal>();
        public IDatabaseConfiguration DatabaseConfig { get; set; } = new DatabaseConfiguration();
        public Int32 MaxDegreeOfParallelism { get; set; } = 1;
        public String ApplicationInstanceName { get; set; }

        public Version ApplicationVersion { get; } = Assembly.GetExecutingAssembly().GetName().Version;

        public String JobQueueName { get; set; }
        //public JobQueueType JobQueueType { get; set; }
        public Int32 JobQueuePartion { get; set; } = 0;
        public Boolean IsSimulation { get; set; } = true;
        public LogLevel LoggingLevel { get; private set; }
        public string LogPath { get; private set; }
        public string OutputPath { get; private set; }

        public void ApplyOptions(BatchProgramOptionsBase options)
        {
            if (options == null)
            {
                throw new DCFApplicationException("Unable to parse application options");
            }

            this.MaxDegreeOfParallelism = options.ItemsToProcessesConcurrently > 0 ? options.ItemsToProcessesConcurrently : 1;
            this.ApplicationInstanceName = options.ApplicationInstanceName;
            this.JobQueueName = options.JobQueueName;
            if (!options.DateString.IsNullOrEmpty())
            {
                this.Date = DateTime.ParseExact(options.DateString, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            }

            LogLevel logLevel;
            if (Enum.TryParse(options.LogLevel, true, out logLevel))
            {
                logLevel = LogLevel.Debug;
            }

            this.LoggingLevel = logLevel;
            this.IsSimulation = options.IsSimulationMode;
            ApplicationContext.AppEnvironment = options.Environment;
            this.OutputPath = Path.IsPathRooted(options.OutputPath) ? options.OutputPath : Path.Combine( Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), options.OutputPath);
            if (!Directory.Exists(this.OutputPath))
                Directory.CreateDirectory(this.OutputPath);
            this.LogPath = options.LoggingPath;
            if (!Directory.Exists(this.LogPath))
                Directory.CreateDirectory(this.LogPath);
            this.JobQueuePartion = options.Partition < 0 ? 0 : options.Partition;

        }

        public static void SetIsInTransitionPeriod()
        {
            var sql = @"SELECT CAST(ParameterValue AS DATE) 'ParameterValue'
                            FROM wwp.SpecialInitiative
                            WHERE ParameterName = '48MonthsStateMaxStartDate'";

            using (var cn = new SqlConnection(DatabaseConfiguration.ConnectionString))
            {
                var stateMax48MonthStartDate = (DateTime)cn.ExecSql(CommandType.Text, sql).Rows[0]["ParameterValue"];

                cn.Close();

                IsInTransitionPeriod = Current.Date.IsBefore(stateMax48MonthStartDate);
            }
        }
    }

    public enum ApplicationEnvironment
    {
        Development = 1,
        Systems = 2,
        ACC = 3,
        Training = 4,
        Production = 5
    }
}