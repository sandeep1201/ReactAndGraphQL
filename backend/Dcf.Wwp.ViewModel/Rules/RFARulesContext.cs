using System;
using System.Collections.Generic;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Api.Library.Contracts;

namespace Dcf.Wwp.Api.Library.Rules
{
    public class RFARulesContext
    {
        #region Properties

        public bool         IsEligible  { get; set; }
        public List<string> Reasons     { get; set; }
        public Int32[]      TJCounties  { get; set; }
        public IParticipant Participant { get; set; }
        public IEnumerable<IFieldData> FieldData { get; set; }

        #endregion

        #region Methods

        public RFARulesContext ()
        {
            Reasons = new List<string>();
        }

        public void AddReason(string reason)
        {
            Reasons.Add(reason);
        }

        #endregion
    }
}
