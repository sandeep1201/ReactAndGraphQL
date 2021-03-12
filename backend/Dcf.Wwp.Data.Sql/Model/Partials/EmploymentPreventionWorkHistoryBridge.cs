//using System;
//using System.ComponentModel.DataAnnotations;
//using Dcf.Wwp.Model.Interface;

//namespace Dcf.Wwp.Data.Sql.Model
//{
//    [MetadataType(typeof(ModelExtension))]
//    public partial class EmploymentWorkHistoryBridge : BaseCommonModel, IEmploymentWorkHistoryBridge
//    {
//        IActionNeeded IEmploymentWorkHistoryBridge.ActionNeeded
//        {
//            get { return null; }
//            set { }
//            //get { return ActionNeeded; }
//            //set { ActionNeeded = (ActionNeeded)value; }
//        }

//        IWorkHistorySection IEmploymentWorkHistoryBridge.WorkHistorySection
//        {
//            get { return WorkHistorySection; }
//            set { WorkHistorySection = (WorkHistorySection)value; }
//        }

//        #region ICloneable

//        public new object Clone()
//        {
//            var epwb = new EmploymentWorkHistoryBridge();

//            epwb.Id = this.Id;
//            epwb.ActionNeededId = this.ActionNeededId;
//            epwb.WorkHistorySectionId = this.WorkHistorySectionId;
//            //epwb.ActionNeeded = (ActionNeeded)this.ActionNeeded?.Clone();
//            epwb.IsDeleted = this.IsDeleted;

//            return epwb;
//        }

//        #endregion ICloneable

//        #region IEquatable<T>

//        public override bool Equals(object other)
//        {
//            if (other == null)
//                return false;

//            var obj = other as EmploymentWorkHistoryBridge;
//            return obj != null && Equals(obj);
//        }

//        public bool Equals(EmploymentWorkHistoryBridge other)
//        {
//            //Check whether the compared object is null.
//            if (Object.ReferenceEquals(other, null)) return false;

//            //Check whether the compared object references the same data.
//            if (Object.ReferenceEquals(this, other)) return true;

//            //Check whether the products' properties are equal.
//            if (!AdvEqual(Id, other.Id))
//                return false;
//            if (!AdvEqual(ActionNeededId, other.ActionNeededId))
//                return false;
//            //if (!AdvEqual(ActionNeeded, other.ActionNeeded))
//            //    return false;
//            if (!AdvEqual(WorkHistorySectionId, other.WorkHistorySectionId))
//                return false;
//            if (!AdvEqual(IsDeleted, other.IsDeleted))
//                return false;

//            return true;
//        }

//        #endregion IEquatable<T>
//    }
//}
