using System.Data;
using System.IO;

namespace Dcf.Wwp.Batch.Interfaces
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
