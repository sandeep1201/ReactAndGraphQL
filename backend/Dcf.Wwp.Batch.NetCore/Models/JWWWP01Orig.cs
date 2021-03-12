using System.Data;
using Dcf.Wwp.Batch.Interfaces;

namespace Dcf.Wwp.Batch.Models
{
    public class JWWWP01Orig : IJWWWP01
    {
        #region Properties

        //private static readonly ILog _log = LogManager.GetLogger(typeof(JWWWP00));
        //private static readonly ILog _log = LogManager.GetLogger("log4net-default-repository", "Dcf.Wwp.Batch.Models");

        public string Name    => GetType().Name;
        public string Desc    => "SP_DB2_Auto_Disenrollment_Update";
        public string Sproc   { get; }
        public int    NumRows { get; }

        #endregion

        #region Methods

        //public JWWWP00 (IDbConfig dbConfig, string sprocName)
        // public JWWWP01(string cs, string sprocName)
        // {
        //
        // }

        public DataTable Run()
        {
            //var retVal = 0;
            //var t = new Test();
            //return (retVal);
            return (new DataTable());
        }

        #endregion
    }
}
