using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : ICertificateIssuingAuthorityRepository
    {
        public ICertificateIssuingAuthority CertificateIssuerById(int? id)
        {
            return (from x in _db.CertificateIssuingAuthorities where x.Id == id select x).FirstOrDefault();
        }

        public IEnumerable<ICertificateIssuingAuthority> CertificateIssuersIssuingAuthorities()
        {
            return _db.CertificateIssuingAuthorities.Where(x => !x.IsDeleted).OrderBy(x => x.SortOrder);
        }

        public IEnumerable<ICertificateIssuingAuthority> AllCertificateIssuersIssuingAuthorities()
        {
            return _db.CertificateIssuingAuthorities.OrderBy(x => x.SortOrder);
        }
    }
}