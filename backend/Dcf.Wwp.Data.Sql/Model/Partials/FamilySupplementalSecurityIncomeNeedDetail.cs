//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Dcf.Wwp.Model.Interface;

//namespace Dcf.Wwp.Data.Sql.Model
//{
//    public partial class FamilySupplementalSecurityIncomeNeedDetail : BaseCommonModel, IFamilySupplementalSecurityIncomeNeedDetail, IEquatable<FamilySupplementalSecurityIncomeNeedDetail>
//    {
//        ICollection<IFamilySupplementalSecurityIncomeNeed> IFamilySupplementalSecurityIncomeNeedDetail.HasReceivingSSIDetails
//        {
//            get { return HasReceivingSSIDetails.Cast<IFamilySupplementalSecurityIncomeNeed>().ToList(); }
//            set { HasReceivingSSIDetails = (ICollection<FamilySupplementalSecurityIncomeNeed>)value; }
//        }
//        ICollection<IFamilySupplementalSecurityIncomeNeed> IFamilySupplementalSecurityIncomeNeedDetail.CaretakingResponsibilitiesDetails
//        {
//            get { return CaretakingResponsibilitiesDetails.Cast<IFamilySupplementalSecurityIncomeNeed>().ToList(); }
//            set { CaretakingResponsibilitiesDetails = (ICollection<FamilySupplementalSecurityIncomeNeed>)value; }
//        }
//        ICollection<IFamilySupplementalSecurityIncomeNeed> IFamilySupplementalSecurityIncomeNeedDetail.HasApplyingSSIDetails
//        {
//            get { return HasApplyingSSIDetails.Cast<IFamilySupplementalSecurityIncomeNeed>().ToList(); }
//            set { HasApplyingSSIDetails = (ICollection<FamilySupplementalSecurityIncomeNeed>)value; }
//        }
//        ICollection<IFamilySupplementalSecurityIncomeNeed> IFamilySupplementalSecurityIncomeNeedDetail.ApplicationStatusDetails
//        {
//            get { return ApplicationStatusDetails.Cast<IFamilySupplementalSecurityIncomeNeed>().ToList(); }
//            set { ApplicationStatusDetails = (ICollection<FamilySupplementalSecurityIncomeNeed>)value; }
//        }
//        ICollection<IFamilySupplementalSecurityIncomeNeed> IFamilySupplementalSecurityIncomeNeedDetail.HasOtherPersonsHelpDetails
//        {
//            get { return HasOtherPersonsHelpDetails.Cast<IFamilySupplementalSecurityIncomeNeed>().ToList(); }
//            set { HasOtherPersonsHelpDetails = (ICollection<FamilySupplementalSecurityIncomeNeed>)value; }
//        }
//        ICollection<IFamilySupplementalSecurityIncomeNeed> IFamilySupplementalSecurityIncomeNeedDetail.HasDeniedSSIDetails
//        {
//            get { return HasDeniedSSIDetails.Cast<IFamilySupplementalSecurityIncomeNeed>().ToList(); }
//            set { HasDeniedSSIDetails = (ICollection<FamilySupplementalSecurityIncomeNeed>)value; }
//        }
//        ICollection<IFamilySupplementalSecurityIncomeNeed> IFamilySupplementalSecurityIncomeNeedDetail.HasReceivedPastSSIDetails
//        {
//            get { return HasReceivedPastSSIDetails.Cast<IFamilySupplementalSecurityIncomeNeed>().ToList(); }
//            set { HasReceivedPastSSIDetails = (ICollection<FamilySupplementalSecurityIncomeNeed>)value; }
//        }
//        ICollection<IFamilySupplementalSecurityIncomeNeed> IFamilySupplementalSecurityIncomeNeedDetail.NoLongerReceiveSSIDetails
//        {
//            get { return NoLongerReceiveSSIDetails.Cast<IFamilySupplementalSecurityIncomeNeed>().ToList(); }
//            set { NoLongerReceiveSSIDetails = (ICollection<FamilySupplementalSecurityIncomeNeed>)value; }
//        }
//        ICollection<IFamilySupplementalSecurityIncomeNeed> IFamilySupplementalSecurityIncomeNeedDetail.HasInterestedInLearningDetails
//        {
//            get { return HasInterestedInLearningDetails.Cast<IFamilySupplementalSecurityIncomeNeed>().ToList(); }
//            set { HasInterestedInLearningDetails = (ICollection<FamilySupplementalSecurityIncomeNeed>)value; }
//        }

//        #region ICloneable

//        public new object Clone()
//        {
//            var fmd = new FamilySupplementalSecurityIncomeNeedDetail();

//            fmd.Id = this.Id;
//            fmd.Details = this.Details;
//            return fmd;
//        }

//        #endregion ICloneable

//        #region IEquatable<T>

//        public override bool Equals(object other)
//        {
//            if (other == null)
//                return false;

//            var obj = other as FamilySupplementalSecurityIncomeNeedDetail;
//            return obj != null && Equals(obj);
//        }

//        public bool Equals(FamilySupplementalSecurityIncomeNeedDetail other)
//        {
//            //Check whether the compared object is null.
//            if (Object.ReferenceEquals(other, null)) return false;

//            //Check whether the compared object references the same data.
//            if (Object.ReferenceEquals(this, other)) return true;

//            //Check whether the products' properties are equal.
//            return Id.Equals(other.Id) &&
//                   Details.Equals(other.Details);
//        }

//        #endregion IEquatable<T>
//    }
//}

