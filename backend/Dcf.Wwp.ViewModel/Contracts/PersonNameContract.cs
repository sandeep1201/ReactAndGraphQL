using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class PersonNameContract
    {
        public int?   Id            { get; set; }
        public string FirstName     { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName      { get; set; }
        public string Suffix        { get; set; }
        public int?   SuffixTypeId  { get; set; }
        public string Alias { get; set; }


       
        public bool IsNew()
        {
            return Id == 0;
        }

        public bool IsEmpty()
        {
            return FirstName.IsNullOrEmpty() && MiddleInitial.IsNullOrEmpty() && LastName.IsNullOrEmpty() && Suffix.IsNullOrEmpty() && Alias.IsNullOrEmpty();
        }

        [JsonIgnore]
        public bool IsMaiden => Alias == "Maiden";

        [JsonIgnore]
        public bool IsAlias => Alias == "Other";

        [JsonIgnore]
        public bool IsNameEmpty => !FirstName.IsNullOrEmpty() || !MiddleInitial.IsNullOrEmpty() || !LastName.IsNullOrEmpty();
    }
}
