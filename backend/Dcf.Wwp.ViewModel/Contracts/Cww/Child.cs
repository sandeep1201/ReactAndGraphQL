using System.Runtime.Serialization;

namespace Dcf.Wwp.Api.Library.Contracts.Cww
{
	[DataContract]
	public class Child
	{
		[DataMember(Name = "firstName")]
		public string FirstName { get; set; }

		[DataMember(Name = "middleInitial")]
		public string MiddleInitial { get; set; }

		[DataMember(Name = "lastName")]
		public string LastName { get; set; }

		[DataMember(Name = "dateOfBirth")]
		public string BirthDate { get; set; }

		[DataMember(Name = "age")]
		public int? Age { get; set; }

		[DataMember(Name = "gender")]
		public string Gender { get; set; }

		[DataMember(Name = "relationship")]
		public string Relationship { get; set; }
	}
}