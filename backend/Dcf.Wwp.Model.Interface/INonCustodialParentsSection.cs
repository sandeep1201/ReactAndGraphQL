using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface INonCustodialParentsSection : ICommonModel, ICloneable, IComplexModel
    {
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        //public NonCustodialParentsSection()
        //{
        //    this.NonCustodialCaretakers = new HashSet<NonCustodialCaretaker>();
        //    this.NonCustodialChilds = new HashSet<NonCustodialChild>();
        //}
        int                                 ParticipantId                       { get; set; }
        Nullable<bool>                      HasChildren                         { get; set; }
        Nullable<decimal>                   ChildSupportPayment                 { get; set; }
        Nullable<bool>                      HasOwedChildSupport                 { get; set; }
        Nullable<bool>                      HasInterestInChildServices          { get; set; }
        Nullable<bool>                      IsInterestedInReferralServices      { get; set; }
        string                              InterestedInReferralServicesDetails { get; set; }
        string                              Notes                               { get; set; }
        int?                                ChildSupportContactId               { get; set; }
        ICollection<INonCustodialCaretaker> AllNonCustodialCaretakers           { get; set; }
        ICollection<INonCustodialCaretaker> NonCustodialCaretakers              { get; set; }
        IParticipant                        Participant                         { get; set; }
        IContact                            ChildSupportContact                 { get; set; }
    }
}
