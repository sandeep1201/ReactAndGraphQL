using System;

namespace Dcf.Wwp.Batch.Infrastructure
{
    public class AppOptions
    {
        #region Properties

        public string JobName      { get; set; }
        public string ExportFormat { get; set; }
        public bool   IsSimulation { get; set; }
        public string Message      { get; set; }
        public Guid   Guid         { get; set; }
        public string LogLevel     { get; set; }
        public string LogPath      { get; set; }
        public string OutputPath   { get; set; }
        public string FielName     { get; set; }

        #endregion

        #region Methods

        public override string ToString()
        {
            return ($"JobName: {JobName} / ExportFormat: {ExportFormat} / IsSimulation: {IsSimulation}");
        }

        #endregion
    }
}
