using System.Data;

namespace Dcf.Wwp.BritsBatch.Interfaces
{
    public interface IExportable
    {
        #region Properties
    
        string FileName    { get; }
        string ContentType { get; }

        #endregion

        #region Methods

        void Export(DataTable dataTable);

        #endregion
    }
}
