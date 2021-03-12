using System;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository
    {
        public IPostSecondaryLicense NewLicense(IPostSecondaryEducationSection parentSection, string user)
        {
            IPostSecondaryLicense psl = new PostSecondaryLicense();
            psl.PostSecondaryEducationSection = parentSection;
            psl.ModifiedDate = DateTime.Now;
            psl.ModifiedBy = user;
            psl.IsDeleted = false;
            _db.PostSecondaryLicenses.Add((PostSecondaryLicense)psl);
            return psl;
        }
    }
}
