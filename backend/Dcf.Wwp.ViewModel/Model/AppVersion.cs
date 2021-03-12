using Serilog.Core;

namespace Dcf.Wwp.Api.Library.Model
{
    public class AppVersion : IAppVersion
    {
        #region Properties

        public int Major { get; }
        public int Minor { get; }
        public int Build { get; }
        public int Rev   { get; }

        public LoggingLevelSwitch LevelSwitch { get; }

        #endregion

        #region Methods

        public AppVersion (int major, int minor, int build, int revision)
        {
            Major = major;
            Minor = minor;
            Build = build;
            Rev   = revision;
        }

        public AppVersion(int major, int minor, int build, int revision, LoggingLevelSwitch levelSwitch)
        {
            Major       = major;
            Minor       = minor;
            Build       = build;
            Rev         = revision;
            LevelSwitch = levelSwitch;
        }

        #endregion
    }
}
