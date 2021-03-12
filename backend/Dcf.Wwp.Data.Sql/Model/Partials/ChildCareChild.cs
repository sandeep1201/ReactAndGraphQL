using Dcf.Wwp.Model.Interface;
using System;
using System.ComponentModel.DataAnnotations;


namespace Dcf.Wwp.Data.Sql.Model
{
 //   [MetadataType(typeof(ModelExtension))]
 //   public partial class ChildCareChild : BaseCommonModel, IChildCareChild,IEquatable<ChildCareChild>
 //   {
	//	IChildCareSection IChildCareChild.ChildCareSection
	//	{
	//		get { return ChildCareSection; }
	//		set { ChildCareSection = (ChildCareSection)value; }
	//	}

	//	IChildCareArrangement IChildCareChild.ChildCareArrangement
	//	{
	//		get { return ChildCareArrangement; }
	//		set { ChildCareArrangement = (ChildCareArrangement)value; }
	//	}

	//    IAgeCategory IChildCareChild.AgeCategory
	//    {
 //          get { return AgeCategory; }
 //          set { AgeCategory = (AgeCategory) value; }
	//    }
	//	#region ICloneable
	//	public new object Clone()
	//	{
	//		var ccc = new ChildCareChild();
	//		ccc.Id = this.Id;
	//		ccc.ChildCareSectionId = this.ChildCareSectionId;
	//		ccc.Name = this.Name;			
	//		ccc.IsSpecialNeeds = this.IsSpecialNeeds;
	//		ccc.DateOfBirth = this.DateOfBirth;
	//		ccc.ChildCareArrangementId = this.ChildCareArrangementId;
	//		ccc.Details = this.Details;
	//	    ccc.AgeCategoryId = this.AgeCategoryId;
	//	    ccc.IsDeleted = this.IsDeleted;
	//		return ccc;
	//	}
	//	#endregion ICloneable

	//	#region IEquatable<T>
	//	public override bool Equals(object other)
	//	{
	//		if (other == null)
	//			return false;
	//		var obj = other as ChildCareChild;
	//		return obj != null && Equals(obj);
	//	}

	//	public bool Equals(ChildCareChild other)
	//	{
	//		//Check whether the compared object is null.
	//		if (Object.ReferenceEquals(other, null)) return false;

	//		//Check whether the compared object references the same data.
	//		if (Object.ReferenceEquals(this, other)) return true;

 //           //Check whether the products' properties are equal.
 //           if (!AdvEqual(Id, other.Id))
 //               return false;
 //           if (!AdvEqual(ChildCareSectionId, other.ChildCareSectionId))
 //               return false;
 //           if (!AdvEqual(Name, other.Name))
 //               return false;
 //           if (!AdvEqual(DateOfBirth, other.DateOfBirth))
 //               return false;
 //           if (!AdvEqual(ChildCareArrangementId, other.ChildCareArrangementId))
 //               return false;
 //           if (!AdvEqual(Details, other.Details))
 //               return false;
	//		if (!AdvEqual(IsSpecialNeeds, other.IsSpecialNeeds))
	//			return false;
	//		if (!AdvEqual(IsDeleted, other.IsDeleted))
 //               return false;
 //           if (!AdvEqual(AgeCategoryId, other.AgeCategoryId))
 //               return false;
 //           return true;
	//	}
	//	#endregion IEquatable<T>
	//}
}
