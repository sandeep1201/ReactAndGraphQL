using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Dcf.Wwp.Api.Library.Contracts
{
	[DataContract]
	public class ContactContract
	{
		[DataMember(Name = "rowVersion")]
		public byte[] RowVersion { get; set; }

		[DataMember(Name = "id")]
		public int Id { get; set; }

		[DataMember(Name = "titleTypeId")]
		public int? TitleTypeId { get; set; }

        [DataMember(Name = "titleTypeName")]
        public string TitleTypeName { get; set; }

        [DataMember(Name = "customTitle")]
		public string CustomTitle { get; set; }

		[DataMember(Name = "name")]
		public string Name { get; set; }

		[DataMember(Name = "email")]
		public string Email { get; set; }

		[DataMember(Name = "phoneNumber")]
		public string PhoneNumber { get; set; }

		[DataMember(Name = "faxNumber")]
		public string FaxNumber { get; set; }

		[DataMember(Name = "phoneExt")]
		public string PhoneExt { get; set; }

		[DataMember(Name = "address")]
		public string Address { get; set; }

		[DataMember(Name = "notes")]
		public string Notes { get; set; }

        [DataMember(Name = "roiSignedDate")]
        public string RoiSignedDate { get; set; }

		[DataMember(Name = "isRoiSigned")]
        public bool? IsRoiSigned { get; set; }

		[DataMember(Name = "modifiedBy")]
		public string ModifiedBy { get; set; }

		[DataMember(Name = "modifiedDate")]
		public DateTime? ModifiedDate { get; set; }

		[DataMember(Name = "modifiedByName")]
		public string ModifiedByName { get; set; }
	}
}
