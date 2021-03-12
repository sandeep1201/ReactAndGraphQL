using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
	public interface IMilitaryRank : ICloneable,ICommonModel
	{
		String Name { get; set; }

		Int32? SortOrder { get; set; }
		Boolean IsDeleted { get; set; }
		ICollection<IMilitaryTrainingSection> IMilitaryTrainingSections { get; set; }
	}
}
