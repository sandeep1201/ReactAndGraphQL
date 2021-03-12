namespace Dcf.Wwp.Model.Interface
{
    public interface IPostSecondaryDegree : ICommonDelModel
    {
        int PostSecondaryEducationSectionId { get; set; }
        string Name { get; set; }
        string College { get; set; }
        int? DegreeTypeId { get; set; }
        int? YearAttained { get; set; }
        IDegreeType DegreeType { get; set; }
        IPostSecondaryEducationSection PostSecondaryEducationSection { get; set; }
    }
}