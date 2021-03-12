using Dcf.Wwp.Batch.Interfaces;

namespace Dcf.Wwp.Batch.Infrastructure
{
    public class ProgramOptions : IProgramOptions
    {
        #region Properties

        public string JobName      { get; set; }
        public string ExportFormat { get; set; }
        public bool   IsSimulation { get; set; }
        public string ProgramCode  { get; set; }
        public string Sproc        { get; set; }
        public string Desc         { get; set; }
        public string JobNumber    { get; set; }
        public bool   IsRQJ        { get; set; }

        #endregion

        #region Methods

        public override string ToString() => ($"JobName: {JobName} / ExportFormat: {ExportFormat} / IsSimulation: {IsSimulation} / ProgramCode: {ProgramCode} / Sproc: {Sproc} / Desc: {Desc} / JobNumber: {JobNumber} / IsRQJ: {IsRQJ}");

        #endregion
    }
}
