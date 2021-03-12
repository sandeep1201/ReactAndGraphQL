namespace Dcf.Wwp.BritsBatch.Interfaces
{
    public interface IProgramOptions
    {
        #region Properties

        string JobName      { get; set; }
        string ExportFormat { get; set; }
        bool   IsSimulation { get; set; }

        #endregion

        #region Methods

        #endregion
    }
}
