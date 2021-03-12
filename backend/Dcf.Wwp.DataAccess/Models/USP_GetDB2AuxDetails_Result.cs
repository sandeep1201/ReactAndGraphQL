namespace Dcf.Wwp.DataAccess.Models
{
    public class USP_GetDB2AuxDetails_Result
    {
        #region Properties

        public decimal? CaseNumber       { get; set; }
        public decimal? PaymentAmount    { get; set; }
        public decimal? RecoupmentAmount { get; set; }
        public short?   OfficeNumber     { get; set; }
        public string   OfficeName       { get; set; }
        public short?   CountyNumber     { get; set; }
        public string   CountyName       { get; set; }
        public string   ProgramCd        { get; set; }
        public string   SubProgramCd     { get; set; }
        public short?   AGSequenceNumber { get; set; }
        public bool?    IsAllowed        { get; set; }
        public decimal? OverPymtAmt      { get; set; }

        #endregion
    }
}
