using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class AssociatedOrganization
    {
        public int             Id              { get; set; }
        public int             ContractAreaId  { get; set; }
        public int             OrganizationId  { get; set; }
        public DateTime        ActivatedDate   { get; set; }
        public DateTime?       InactivatedDate { get; set; }
        public bool            IsDeleted       { get; set; }
        public string          ModifiedBy      { get; set; }
        public System.DateTime ModifiedDate    { get; set; }

        public virtual ContractArea ContractArea { get; set; }
        public virtual Organization Organization { get; set; }
    }
}
