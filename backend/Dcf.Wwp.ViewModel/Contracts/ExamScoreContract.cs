using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Dcf.Wwp.Api.Library.Contracts
{
	[DataContract]
	public class ExamScoreContract 
    {
	
		[DataMember(Name = "id")]
		public int Id { get; set; }

		[DataMember(Name = "typeId")]
		public int? ExamTypeId { get; set; }

		[DataMember(Name = "name")]
		public string ExamName { get; set; }

		[DataMember(Name = "dateTaken")]
		public string DateTaken { get; set; }

		[DataMember(Name = "examResults")]
		public List<SubjectContract> ExamResults { get; set; }

        [DataMember(Name = "details")]
        public string Details { get; set; }

        [DataMember(Name = "rowVersion")]
        public byte[] RowVersion { get; set; }

		[DataMember(Name = "modifiedBy")]
		public string ModifiedBy { get; set; }

	    [DataMember(Name = "modifiedDate")]
	    public DateTime? ModifiedDate { get; set; }

	    [DataMember(Name = "modifiedByName")]
	    public string ModifiedByName { get; set; }
	}
}
