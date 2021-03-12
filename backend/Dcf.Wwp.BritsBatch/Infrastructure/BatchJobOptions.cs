using System;
using Dcf.Wwp.BritsBatch.Interfaces;

namespace Dcf.Wwp.BritsBatch.Infrastructure
{
    public class BatchJobOptions : ProgramOptions, IBatchJobOptions
    {
        #region Properties

        public string   OutPath      { get; set; }  
        public string   SubGuid      { get; set; } // used to build filename and prevent name collisions
        public DateTime RunTime      { get; set; } // used to build filename and prevent name collisions

        #endregion

        #region Methods

        public override string ToString() => ($"JobName: {JobName} / ExportFormat: {ExportFormat} / IsSimulation: {IsSimulation}");

        #endregion
    }
}
