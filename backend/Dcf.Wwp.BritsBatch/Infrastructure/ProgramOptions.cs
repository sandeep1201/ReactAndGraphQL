using Dcf.Wwp.BritsBatch.Interfaces;

namespace Dcf.Wwp.BritsBatch.Infrastructure
{
    public class ProgramOptions : IProgramOptions
    {
        #region Properties

        public string JobName      { get; set; }
        public string ExportFormat { get; set; }
        public bool   IsSimulation { get; set; }

        #endregion

        #region Methods

        public override string ToString() => ($"JobName: {JobName} / ExportFormat: {ExportFormat} / IsSimulation: {IsSimulation}");

        #endregion
    }
}
