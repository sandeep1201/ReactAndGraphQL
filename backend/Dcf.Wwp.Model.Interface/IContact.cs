using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface IContact : ICloneable, ICommonModel
    {
        Int32?    ParticipantId          { get; set; }
        Int32?    TitleId                { get; set; }
        String    CustomTitle            { get; set; }
        String    Name                   { get; set; }
        String    Email                  { get; set; }
        String    Phone                  { get; set; }
        String    ExtensionNo            { get; set; }
        String    FaxNo                  { get; set; }
        String    Address                { get; set; }
        String    Notes                  { get; set; }
        DateTime? ReleaseInformationDate { get; set; }
        Boolean   IsDeleted              { get; set; }

        IContactTitleType                        ContactTitleType            { get; set; }
        IParticipant                             Participant                 { get; set; }
        ICollection<IInvolvedWorkProgram>        InvolvedWorkPrograms        { get; set; }
        ICollection<ILegalIssuesSection>         LegalIssuesSections         { get; set; }
        ICollection<IBarrierDetailContactBridge> BarrierDetailContactBridges { get; set; }
        ICollection<IEmploymentInformation>      EmploymentInformations      { get; set; }
        ICollection<IActivityContactBridge>      ActivityContactBridges      { get; set; }
        ICollection<INonCustodialParentsSection> NonCustodialParentsSections { get; set; }
    }
}
