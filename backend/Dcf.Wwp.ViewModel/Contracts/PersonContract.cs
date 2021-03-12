using System;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class PersonContract
    {
        public decimal? Pin { get; set; }

        public string FirstName { get; set; }

        public string MiddleInitial { get; set; }

        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public int? Age { get; set; }

        public string Gender { get; set; }
    }

    public class RelatedPersonContract : PersonContract
    {
        public string Relationship { get; set; }
    }
}
