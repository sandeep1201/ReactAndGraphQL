using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
 //   [MetadataType(typeof(ModelExtension))]
 //   public partial class ChildCareActionBridge : BaseCommonModel, IChildCareActionBridge, IEquatable<ChildCareActionBridge>
	//{

	//	IChildCareSection IChildCareActionBridge.ChildCareSection
	//	{
	//		get { return ChildCareSection; }
	//		set { ChildCareSection = (ChildCareSection)value; }
	//	}
	//	IActionNeeded IChildCareActionBridge.ActionNeeded
	//	{
	//		get { return ActionNeeded; }
	//		set { ActionNeeded = (ActionNeeded)value; }
	//	}

	//	#region ICloneable

	//	public new object Clone()
	//	{
	//		var ccc = new ChildCareActionBridge();

	//		ccc.Id = this.Id;
	//		ccc.ChildCareSectionId = this.ChildCareSectionId;
	//		ccc.ActionNeededId = this.ActionNeededId;
			
			
	//		//Clone Child Object
	//		ccc.IsDeleted = this.IsDeleted;

	//		return ccc;
	//	}

	//	#endregion ICloneable

	//	#region IEquatable<T>

	//	public override bool Equals(object other)
	//	{
	//		if (other == null)
	//			return false;

	//		var obj = other as ChildCareActionBridge;
	//		return obj != null && Equals(obj);
	//	}

	//	public bool Equals(ChildCareActionBridge other)
	//	{
	//		//Check whether the compared object is null.
	//		if (Object.ReferenceEquals(other, null)) return false;

	//		//Check whether the compared object references the same data.
	//		if (Object.ReferenceEquals(this, other)) return true;

	//		//Check whether the products' properties are equal.

	//		if (!AdvEqual(Id, other.Id))
	//			return false;
	//		if (!AdvEqual(ChildCareSectionId, other.ChildCareSectionId))
	//			return false;
	//		if (!AdvEqual(ActionNeededId, other.ActionNeededId))
	//			return false;
	//		if (!AdvEqual(IsDeleted, other.IsDeleted))
	//			return false;

	//		return true;
	//	}

	//	#endregion IEquatable<T>
	//}
}



