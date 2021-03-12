
namespace Dcf.Wwp.Model.Interface
{
    public interface IEmployerOfRecordInformation : ICommonDelModel
    {
        #region Properties

        int                    EmploymentInformationId { get; set; }
        string                 CompanyName             { get; set; }
        string                 Fein                    { get; set; }
        string                 StreetAddress           { get; set; }
        string                 ZipAddress              { get; set; }
        int?                   CityId                  { get; set; }
        int?                   JobSectorId             { get; set; }
        int?                   ContactId               { get; set; }
        
        #endregion

        #region Navigation Properties

        ICity                  City                    { get; set; }
        IContact               Contact                 { get; set; }
        IEmploymentInformation EmploymentInformation   { get; set; }
        IJobSector             JobSector               { get; set; }

        #endregion
    }
}
