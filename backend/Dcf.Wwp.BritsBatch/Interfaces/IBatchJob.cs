namespace Dcf.Wwp.BritsBatch.Interfaces
{
    public interface IBatchJob
    {
        #region Properties

        string Name    { get; } // 'Short' name or Control-M name, ie, 'JWBWP00'
        string Desc    { get; } // 'Long' name, ie, 'Get Recoupment Amount'
        string Sproc   { get; } // 'wwp.USP_GetWWRecoupment_Amount'
        int    NumRows { get; } // number of rows returned by sproc

        #endregion

        #region Methods

        string Run(); // Invokes the batch job run

        #endregion
    }
}
