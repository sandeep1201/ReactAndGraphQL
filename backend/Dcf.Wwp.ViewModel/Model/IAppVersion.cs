using Serilog.Core;

namespace Dcf.Wwp.Api.Library.Model
{
    public interface IAppVersion
    {
        #region Properties

        int Major { get; }
        int Minor { get; }
        int Build { get; }
        int Rev   { get; }

        LoggingLevelSwitch LevelSwitch { get; }

        #endregion

        #region Methods

        #endregion
    }
}
