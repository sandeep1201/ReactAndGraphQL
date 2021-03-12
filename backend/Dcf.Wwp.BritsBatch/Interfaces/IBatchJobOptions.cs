using System;

namespace Dcf.Wwp.BritsBatch.Interfaces
{
    public interface IBatchJobOptions : IProgramOptions
    {
        #region Properties

        string   OutPath      { get; set; }
        string   SubGuid      { get; set; } // used to build filename and prevent name collisions
        DateTime RunTime      { get; set; } // used to build filename and prevent name collisions

        #endregion

        #region Methods

        #endregion
    }
}
