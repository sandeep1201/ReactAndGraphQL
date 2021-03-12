﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dcf.Wwp.Api.Library.Model.Api.Google.Schools
{
		//------------------------------------------------------------------------------
		// <auto-generated>
		//     This code was generated by a tool.
		//     Runtime Version:4.0.30319.42000
		//
		//     Changes to this file may cause incorrect behavior and will be lost if
		//     the code is regenerated.
		// </auto-generated>
		//------------------------------------------------------------------------------

		// Type created for JSON at <<root>>
		[System.Runtime.Serialization.DataContractAttribute()]
		public partial class SchoolsFromGoogle
		{

			[System.Runtime.Serialization.DataMemberAttribute()]
			public object[] html_attributions;

			[System.Runtime.Serialization.DataMemberAttribute()]
			public Results[] results;

			[System.Runtime.Serialization.DataMemberAttribute()]
			public string status;
		}

		// Type created for JSON at <<root>> --> results
		[System.Runtime.Serialization.DataContractAttribute(Name = "results")]
		public partial class Results
		{

			[System.Runtime.Serialization.DataMemberAttribute()]
			public Geometry geometry;

			[System.Runtime.Serialization.DataMemberAttribute()]
			public string icon;

			[System.Runtime.Serialization.DataMemberAttribute()]
			public string id;

			[System.Runtime.Serialization.DataMemberAttribute()]
			public string name;

			[System.Runtime.Serialization.DataMemberAttribute()]
			public string place_id;

			[System.Runtime.Serialization.DataMemberAttribute()]
			public string reference;

			[System.Runtime.Serialization.DataMemberAttribute()]
			public string scope;

			[System.Runtime.Serialization.DataMemberAttribute()]
			public string[] types;

			[System.Runtime.Serialization.DataMemberAttribute()]
			public string vicinity;

			[System.Runtime.Serialization.DataMemberAttribute()]
			public Opening_hours opening_hours;

			[System.Runtime.Serialization.DataMemberAttribute()]
			public Photos[] photos;

			[System.Runtime.Serialization.DataMemberAttribute()]
			public double rating;
		}

		// Type created for JSON at <<root>> --> geometry
		[System.Runtime.Serialization.DataContractAttribute(Name = "geometry")]
		public partial class Geometry
		{

			[System.Runtime.Serialization.DataMemberAttribute()]
			public Location location;
		}

		// Type created for JSON at <<root>> --> geometry --> location
		[System.Runtime.Serialization.DataContractAttribute(Name = "location")]
		public partial class Location
		{

			[System.Runtime.Serialization.DataMemberAttribute()]
			public double lat;

			[System.Runtime.Serialization.DataMemberAttribute()]
			public double lng;
		}

		// Type created for JSON at <<root>> --> opening_hours
		[System.Runtime.Serialization.DataContractAttribute(Name = "opening_hours")]
		public partial class Opening_hours
		{

			[System.Runtime.Serialization.DataMemberAttribute()]
			public bool open_now;

			[System.Runtime.Serialization.DataMemberAttribute()]
			public object[] weekday_text;
		}

		// Type created for JSON at <<root>> --> photos
		[System.Runtime.Serialization.DataContractAttribute(Name = "photos")]
		public partial class Photos
		{

			[System.Runtime.Serialization.DataMemberAttribute()]
			public int height;

			[System.Runtime.Serialization.DataMemberAttribute()]
			public string[] html_attributions;

			[System.Runtime.Serialization.DataMemberAttribute()]
			public string photo_reference;

			[System.Runtime.Serialization.DataMemberAttribute()]
			public int width;
		}


	}

