namespace Dcf.Wwp.Model.Interface
{
    public interface IContractor
    {
        #region Properties

        int Id                { get; set; }
        int EnrolledProgramId { get; set; }
        int OrganizationId    { get; set; }

        #endregion

        #region Navigation Props

        IEnrolledProgram EnrolledProgram { get; set; }
        IOrganization    Organization    { get; set; }

        #endregion
    }
}
