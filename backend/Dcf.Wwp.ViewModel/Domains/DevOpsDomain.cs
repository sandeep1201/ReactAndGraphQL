using System;
using System.Dynamic;
using System.IO;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Api.Library.Model;
using Dcf.Wwp.ConnectedServices.Logging;
using Dcf.Wwp.Model.Interface.Core;
using DCF.Common;
using Newtonsoft.Json;
using Serilog.Events;

namespace Dcf.Wwp.Api.Library.Domains
{
    public class DevOpsDomain : IDevOpsDomain
    {
        #region Properties

        private readonly IAppVersion            _version;
        private readonly IDatabaseConfiguration _dbConfig;
        private readonly IAuthUser              _authUser;

        #endregion

        #region Methods

        public DevOpsDomain(IAuthUser authUser, IDatabaseConfiguration dbConfig, IAppVersion version)
        {
            _version  = version;
            _dbConfig = dbConfig;
            _authUser = authUser;
        }

        #endregion

        public dynamic GetStatus()
        {
            dynamic model = new ExpandoObject();

            model.AppVersion  = $"{_version.Major}.{_version.Minor}.{_version.Build}.{_version.Rev}";
            model.DbServer    = _dbConfig.Server;
            model.DbInstance  = _dbConfig.Catalog;
            model.DbUser      = _dbConfig.UserId;
            model.LogLevelApp = _version.LevelSwitch.MinimumLevel.ToString().ToUpper();
            model.LogLevelWcf = GetWcfLogLevel().WcfLogLevel;
            model.AuthUser    = _authUser.Username;

            return (model);
        }

        public string GetStatus2()
        {
            dynamic model = new ExpandoObject();

            model.AppVersion  = $"{_version.Major}.{_version.Minor}.{_version.Build}.{_version.Rev}";
            model.DbServer    = _dbConfig.Server;
            model.DbInstance  = _dbConfig.Catalog;
            model.DbUser      = _dbConfig.UserId;
            model.LogLevelApp = _version.LevelSwitch.MinimumLevel.ToString().ToUpper();
            model.LogLevelWcf = GetWcfLogLevel().WcfLogLevel;
            model.AuthUser    = _authUser.Username;
            model.MainframeId = _authUser.MainFrameId;
            model.WIUID       = _authUser.WIUID;

            var logFileName = $"{AppDomain.CurrentDomain.BaseDirectory}\\Logs\\Dcf.Wwp.Api.Startup.log";

            var fileStream = new FileStream(logFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            string fileContents;

            using (var reader = new StreamReader(fileStream))
            {
                fileContents = reader.ReadToEnd();
            }

            string[] separator = new string[] { "\r\n" };

            model.LogContents = fileContents.Split(separator, StringSplitOptions.None); 

            string json = JsonConvert.SerializeObject(model);

            return (json);
        }

        public dynamic GetDbInfo()
        {
            dynamic model = new ExpandoObject();

            model.DbServer    = _dbConfig.Server;
            model.DbDatabase  = _dbConfig.Catalog;
            model.DbUser      = _dbConfig.UserId;
            model.DbPass      = _dbConfig.Password;
            model.MaxPoolSize = _dbConfig.MaxPoolSize;
            model.Timeout     = _dbConfig.Timeout;

            return (model);
        }

        public LogEventLevel GetAppLogLevel()
        {
            var appLogLevel = _version.LevelSwitch.MinimumLevel;

            return (appLogLevel);
        }

        public dynamic SetAppLogLevel(LogEventLevel logEventLevel = LogEventLevel.Debug)
        {
            dynamic model = new ExpandoObject();

            model.AppLogLevel = _version.LevelSwitch.MinimumLevel;

            _version.LevelSwitch.MinimumLevel = logEventLevel;

            model.NewAppLogLevel = logEventLevel;

            return (model);
        }

        public void SetAppLogLevel(bool flush = false)
        {
            throw new NotImplementedException();
        }

        public dynamic GetWcfLogLevel()
        {
            var currentLevel = WebServicesLogger.GetLevel();

            dynamic model = new ExpandoObject();
            model.WcfLogLevel = currentLevel.ToUpper();

            return (model);
        }

        public dynamic SetWcfLogLevel(string newLevel)
        {
            dynamic model = new ExpandoObject();
            model.WcfLogLevel = WebServicesLogger.GetLevel().ToUpper();

            WebServicesLogger.SetLevel(newLevel);

            model.NewWcfLogLevel = newLevel.ToUpper();

            return (model);
        }

        public void ThrowExeption(string message = null)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                message = "Throwing a test exception.";
            }

            throw new Exception(message);
        }
    }
}
