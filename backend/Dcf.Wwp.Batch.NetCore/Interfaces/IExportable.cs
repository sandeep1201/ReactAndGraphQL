
using System.Data;

namespace Dcf.Wwp.Batch.Interfaces
{
    public interface IExportable // Probably should rename this to 'IExportable' because it's a behavior
    {
        #region Properties
    
        string FileName    { get; }
        //byte[] FileData    { get; }
        string ContentType { get; }

        #endregion

        #region Methods

        void WriteOutput(DataTable dataTable);

        #endregion
    }
}
