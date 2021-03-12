namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IPostSecondaryLicenseRepository
    {
        IPostSecondaryLicense NewLicense(IPostSecondaryEducationSection parentSection, string user);
    }
}
