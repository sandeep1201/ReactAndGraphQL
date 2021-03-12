using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class CareerAssessmentContract
    {
        public int          Id                          { get;  set; }
        public int          ParticipantId               { get;  set; }
        public DateTime     CompletionDate              { get;  set; }
        public string       AssessmentProvider          { get;  set; }
        public string       AssessmentToolUsed          { get;  set; }
        public string       AssessmentResults           { get;  set; }
        public int?         CareerAssessmentContactId   { get;  set; }
        public string       RelatedOccupation           { get;  set; }
        public string       AssessmentResultAppliedToEP { get;  set; }
        public bool         IsDeleted                   { get;  set; }
        public DateTime     CreatedDate                 { get;  set; }
        public string       ModifiedBy                  { get;  set; }
        public DateTime     ModifiedDate                { get;  set; }
        public List<int>    ElementIds                  { get;  set; }
        public List<string> ElementNames                {  get; set; }
    }
}
