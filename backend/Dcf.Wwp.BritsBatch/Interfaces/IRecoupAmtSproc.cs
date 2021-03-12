using System.Data;
using System.Xml.Linq;

namespace Dcf.Wwp.BritsBatch.Interfaces
{
    public interface IRecoupAmtSproc
    {
        #region Properties

        #endregion

        #region Methods

        string ExecGetSproc();
        string ExecPostSproc();
        void ExecResponseSproc(XDocument xml);

        #endregion
    }
}
