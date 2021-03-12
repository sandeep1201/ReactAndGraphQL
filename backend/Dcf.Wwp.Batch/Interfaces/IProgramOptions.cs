namespace Dcf.Wwp.Batch.Interfaces
{
    public interface IProgramOptions
    {
        #region Properties

        string JobName      { get; set; }
        string ExportFormat { get; set; }
        bool   IsSimulation { get; set; }
        string ProgramCode  { get; set; }
        string Sproc        { get; set; }
        string Desc         { get; set; }
        string JobNumber    { get; set; }
        bool   IsRQJ        { get; set; }

        #endregion

        #region Methods

        #endregion
    }
}
