using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IPostSecondaryLicense : ICommonDelModel
    {
        string Name { get; set; }
        string Issuer { get; set; }
        DateTime? AttainedDate { get; set; }
        DateTime? ExpiredDate { get; set; }
        bool? IsInProgress { get; set; }
        bool? DoesNotExpire { get; set; }
        int? LicenseTypeId { get; set; }
        int? ValidInWIPolarLookupId { get; set; }
        int PostSecondaryEducationSectionId { get; set; }
        IPostSecondaryEducationSection PostSecondaryEducationSection { get; set; }
        ILicenseType LicenseType { get; set; }
        IPolarLookup PolarLookup { get; set; }
    }
}
