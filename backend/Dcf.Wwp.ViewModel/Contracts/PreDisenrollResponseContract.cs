using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Dcf.Wwp.Api.Library.Contracts
{
    [DataContract]
    public class DisenrollCheckContract
    {
        public DisenrollCheckContract()
        {
            Errors   = new List<string>();
            Warnings = new List<string>();
        }

        [DataMember(Name = "status")]
        public bool CanDisenroll { get; set; }

        [DataMember(Name = "errors")]
        public List<string> Errors { get; set; }

        [DataMember(Name = "warnings")]
        public List<string> Warnings { get; set; }

        [JsonIgnore]
        public List<string> ErrorCodes { get; set; }

        [DataMember(Name = "activityEndDate")]
        public DateTime? ActivityEndDate { get; set; }


        /// <remarks>
        /// This is for use by the NRules engine 
        /// </remarks>
        /// <param name="message">Error Code</param>
        public void AddErrorCodes(string message)
        {
            if (this.ErrorCodes == null)
                this.ErrorCodes = new List<string>();

            ErrorCodes.Add(message);
        }
    }
}
