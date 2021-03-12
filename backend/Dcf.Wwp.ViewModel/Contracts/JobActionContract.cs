using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Api.Library.Extensions;

namespace Dcf.Wwp.Api.Library.Contracts
{
	public class JobActionContract
	{
		public JobActionContract()
		{
		}

		[DataMember(Name = "id")]
		public int Id { get; private set; }

		[DataMember(Name = "name")]
		public string Name { get; private set; }

		[DataMember(Name = "disablesOthers")]
		public bool? DisablesOthers { get; private set; }

		[DataMember(Name = "actionType")]
		public string ActionType { get; private set; }

		public static JobActionContract Create(int id, string name, bool? disablesOthers, string actionType)
		{
			return new JobActionContract { Id = id, Name = name.SafeTrim(), DisablesOthers = disablesOthers, ActionType = actionType.SafeTrim() };
		}
	}
}
