using Serilog.Events;

namespace Dcf.Wwp.Api.Library.Interfaces
{
    public interface IDevOpsDomain
    {
        #region Properties

        #endregion

        #region Methods

        dynamic GetStatus();
        string  GetStatus2();
        dynamic GetDbInfo();
        LogEventLevel GetAppLogLevel();
        dynamic SetAppLogLevel(LogEventLevel logEventLevel = LogEventLevel.Debug);
        void    SetAppLogLevel(bool flush = false);
        dynamic GetWcfLogLevel();
        dynamic SetWcfLogLevel(string newLevel);
        void    ThrowExeption(string message = null);

        #endregion
    }
}
