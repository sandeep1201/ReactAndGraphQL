using System;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IPostSecondaryDegreeRepository
    {
        public IPostSecondaryDegree NewDegree(IPostSecondaryEducationSection parentSection, string user)
        {
            IPostSecondaryDegree psd = new PostSecondaryDegree();
            psd.PostSecondaryEducationSection = parentSection;
            psd.IsDeleted = false;
            psd.ModifiedBy = user;
            psd.ModifiedDate = DateTime.Now;

            _db.PostSecondaryDegrees.Add((PostSecondaryDegree)psd);
            return psd;
        }
    }
}
