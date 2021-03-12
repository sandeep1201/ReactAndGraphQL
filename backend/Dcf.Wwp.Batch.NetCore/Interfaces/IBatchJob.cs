using System.Data;

namespace Dcf.Wwp.Batch.Interfaces
{
    public interface IBatchJob
    {
        #region Properties

        string Name    { get; } // Control-M name, ie, 'JWWWP00'
        string Desc    { get; } // 'Long' or 'English' name, ie, 'Auto Disenrollment Update'
        string Sproc   { get; } // 'wwp.SP_DB2_Auto_Disenrollment_Update'
        int    NumRows { get; } // number of rows returned by sproc

        #endregion

        #region Methods

        DataTable Run(); // Invokes the batch job run

        #endregion
    }
}
