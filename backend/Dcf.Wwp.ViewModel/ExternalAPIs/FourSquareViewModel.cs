
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace Dcf.Wwp.Api.Library.ExternalAPIs
{

	//Decommissioned Api; Foursquare api returned incorrect place data.
	[DataContract]
	public class FourSquareViewModel
	{
		[DataMember(Name = "query")]

		public string name { get; set; }

		//43 for madison
		[DataMember(Name = "lat")]

		public string Latitude { get; set; }

		//-89 for madison
		[DataMember(Name = "long")]
		public string Longitude { get; set; }

		private string BaseURL = "https://api.foursquare.com/v2/venues/search?&categoryId=4bf58dd8d48988d13d941735&oauth_token=FIW5VJWXXIMAM0VO2ODKZYVPJUO5P5WB1UZEMR4GM1TQPMVL&v=20160122";

		public async Task<string> MakeURL()
		{

			BaseURL += "&ll=" + Latitude + "," + Longitude + "&query=" + name;


			HttpClient a = new HttpClient();

			var result = await a.GetStringAsync(BaseURL);

			GJSON m = JsonConvert.DeserializeObject<GJSON>(result);

			return result;
		}
	}
}
