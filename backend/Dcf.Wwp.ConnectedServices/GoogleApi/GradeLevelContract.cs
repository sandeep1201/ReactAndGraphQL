﻿using System.Runtime.Serialization;

namespace Dcf.Wwp.ConnectedServices.GoogleApi
{

	[DataContract]
    public class SchoolGradeContract
    {
		[DataMember(Name = "sortOrder")]
		public int? SortOrder { get; set; }

		[DataMember(Name = "grade")]
		public int? Grade { get; set; }

		[DataMember(Name = "name")]
		public string Name { get; set; }

	}
}