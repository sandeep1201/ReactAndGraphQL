namespace Dcf.Wwp.Model.Interface
{
    public interface IPostSecondaryCollege : ICommonDelModel
    {
        int PostSecondaryEducationSectionId { get; set; }
        int? SchoolCollegeEstablishmentId { get; set; }
        bool? HasGraduated { get; set; }
        int? LastYearAttended { get; set; }
        int? Semesters { get; set; }
        decimal? Credits { get; set; }
        string Details { get; set; }
        bool? CurrentlyAttending { get; set; }
        int? SortOrder { get; set; }

        IPostSecondaryEducationSection PostSecondaryEducationSection { get; set; }
        ISchoolCollegeEstablishment SchoolCollegeEstablishment { get; set; }
    }
}