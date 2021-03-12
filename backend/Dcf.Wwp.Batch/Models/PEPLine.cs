namespace Dcf.Wwp.Batch.Models
{
    public class PEPLine
    {
        #region Properties

        public int    PEPId              { get; set; }
        public string CompletionReasonCd { get; set; }
        public int    EnrolledProgramId  { get; set; }
        public bool   CoEnrolled         { get; set; }

        #endregion

        #region Methods

        #endregion
    }
}
