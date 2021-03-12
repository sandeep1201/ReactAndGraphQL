
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Dcf.Wwp.Api.Library.Contracts
{
    [DataContract]
    public class PreSavingEPStatusContract
    {
        [DataMember(Name = "status")]
        public bool CanSaveEP { get; set; }

        [DataMember(Name = "errors")]
        public List<string> Errors { get; set; }

        [DataMember(Name = "warnings")]
        public List<string> Warnings { get; set; }

        [JsonIgnore]
        public List<string> ErrorCodes { get; set; }


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

        public PreSavingEPStatusContract()
        {
            Warnings = new List<string>();
            Errors = new List<string>();
        }
    }

}
