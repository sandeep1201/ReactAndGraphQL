using System;
using Dcf.Wwp.Model.Interface.Cww;


namespace Dcf.Wwp.Model.Cww
{
	public class CurrentChild : ICurrentChild
	{
		public string Pin { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Middle { get; set; }
		public DateTime? BirthDate { get; set; }
		public DateTime? DeathDate { get; set; }
		public string Gender { get; set; }
		public string Relationship { get; set; }
		public int? Age { get; set; }
	}
}