//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Dcf.Wwp.Model.Interface;

//namespace Dcf.Wwp.Data.Sql.Model
//{
//	public partial class EmploymentPreventionFactor : BaseCommonModel, IEmploymentPreventionFactor
//	{
//		ICollection<IEmploymentPreventionWorkHistoryBridge> IEmploymentPreventionFactor.EmploymentPreventionWorkHistoryBridges
//		{
//			get
//			{ return EmploymentPreventionWorkHistoryBridges.Cast<IEmploymentPreventionWorkHistoryBridge>().ToList(); }
//			set { EmploymentPreventionWorkHistoryBridges = (ICollection<EmploymentPreventionWorkHistoryBridge>)value; }
//		}

//		#region ICloneable

//		public new object Clone()
//		{
//			var epf = new EmploymentPreventionFactor();

//			epf.Id = this.Id;
//			epf.SortOrder = this.SortOrder;
//			epf.Name = this.Name;
//			return epf;
//		}

//		#endregion ICloneable

//		#region IEquatable<T>

//		public override bool Equals(object other)
//		{
//			if (other == null)
//				return false;

//			var obj = other as EmploymentPreventionFactor;
//			return obj != null && Equals(obj);
//		}

//		public bool Equals(EmploymentPreventionFactor other)
//		{
//			//Check whether the compared object is null.
//			if (Object.ReferenceEquals(other, null)) return false;

//			//Check whether the compared object references the same data.
//			if (Object.ReferenceEquals(this, other)) return true;

//			// Check whether the products' properties are equal.
//			// We have to be careful doing comparisons on null object properties.

//			if (AreBothNotNull(Id, other.Id) && (!Id.Equals(other.Id)) || EitherNotNull(Id, other.Id) && (!Id.Equals(other.Id)))
//				return false;
//			if (AreBothNotNull(SortOrder, other.SortOrder) && (!SortOrder.Equals(other.SortOrder)) || EitherNotNull(SortOrder, other.SortOrder) && (!SortOrder.Equals(other.SortOrder)))
//				return false;
//			if (AreBothNotNull(Name, other.Name) && (!Name.Equals(other.Name)) || EitherNotNull(Name, other.Name) && (!Name.Equals(other.Name)))
//				return false;

//			return true;
//		}

//		#endregion IEquatable<T>
//	}
//}
