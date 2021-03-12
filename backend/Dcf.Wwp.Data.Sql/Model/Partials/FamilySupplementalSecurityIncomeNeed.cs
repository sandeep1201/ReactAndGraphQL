//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Dcf.Wwp.Model.Interface;

//namespace Dcf.Wwp.Data.Sql.Model
//{
//    public partial class FamilySupplementalSecurityIncomeNeed : BaseCommonModel, 
//        IFamilySupplementalSecurityIncomeNeed, IEquatable<FamilySupplementalSecurityIncomeNeed>
//    {
//        ICollection<IFamilyBarriersSection> IFamilySupplementalSecurityIncomeNeed.FamilyBarriersSections
//        {
//            get { return FamilyBarriersSections.Cast<IFamilyBarriersSection>().ToList(); }
//            set { FamilyBarriersSections = (ICollection<FamilyBarriersSection>)value; }
//        }

//        IApplicationStatusType IFamilySupplementalSecurityIncomeNeed.ApplicationStatusType
//        {
//            get { return ApplicationStatusType; }
//            set { ApplicationStatusType = (ApplicationStatusType)value; }
//        }

//        IContact IFamilySupplementalSecurityIncomeNeed.Contact
//        {
//            get { return Contact; }
//            set { Contact = (Contact)value; }
//        }

//        IFamilySupplementalSecurityIncomeNeedDetail IFamilySupplementalSecurityIncomeNeed.HasReceivingSSIDetail
//        {
//            get { return HasReceivingSSIDetail; }
//            set { HasReceivingSSIDetail = (FamilySupplementalSecurityIncomeNeedDetail)value; }
//        }
//        IFamilySupplementalSecurityIncomeNeedDetail IFamilySupplementalSecurityIncomeNeed.CaretakingResponsibilitiesDetail
//        {
//            get { return CaretakingResponsibilitiesDetail; }
//            set { CaretakingResponsibilitiesDetail = (FamilySupplementalSecurityIncomeNeedDetail)value; }
//        }
//        IFamilySupplementalSecurityIncomeNeedDetail IFamilySupplementalSecurityIncomeNeed.ApplicationStatusDetail
//        {
//            get { return ApplicationStatusDetail; }
//            set { ApplicationStatusDetail = (FamilySupplementalSecurityIncomeNeedDetail)value; }
//        }
//        IFamilySupplementalSecurityIncomeNeedDetail IFamilySupplementalSecurityIncomeNeed.HasOtherPersonsHelpDetail
//        {
//            get { return HasOtherPersonsHelpDetail; }
//            set { HasOtherPersonsHelpDetail = (FamilySupplementalSecurityIncomeNeedDetail)value; }
//        }
//        IFamilySupplementalSecurityIncomeNeedDetail IFamilySupplementalSecurityIncomeNeed.HasDeniedSSIDetail
//        {
//            get { return HasDeniedSSIDetail; }
//            set { HasDeniedSSIDetail = (FamilySupplementalSecurityIncomeNeedDetail)value; }
//        }
//        IFamilySupplementalSecurityIncomeNeedDetail IFamilySupplementalSecurityIncomeNeed.HasReceivedPastSSIDetail
//        {
//            get { return HasReceivedPastSSIDetail; }
//            set { HasReceivedPastSSIDetail = (FamilySupplementalSecurityIncomeNeedDetail)value; }
//        }
//        IFamilySupplementalSecurityIncomeNeedDetail IFamilySupplementalSecurityIncomeNeed.NoLongerReceiveSSIDetail
//        {
//            get { return NoLongerReceiveSSIDetail; }
//            set { NoLongerReceiveSSIDetail = (FamilySupplementalSecurityIncomeNeedDetail)value; }
//        }
//        IFamilySupplementalSecurityIncomeNeedDetail IFamilySupplementalSecurityIncomeNeed.HasInterestedInLearningDetail
//        {
//            get { return HasInterestedInLearningDetail; }
//            set { HasInterestedInLearningDetail = (FamilySupplementalSecurityIncomeNeedDetail)value; }
//        }
//        IFamilySupplementalSecurityIncomeNeedDetail IFamilySupplementalSecurityIncomeNeed.HasApplyingSSIDetail
//        {
//            get { return HasApplyingSSIDetail; }
//            set { HasApplyingSSIDetail = (FamilySupplementalSecurityIncomeNeedDetail)value; }
//        }
//        #region ICloneable

//        public object Clone()
//        {
//            var fn = new FamilySupplementalSecurityIncomeNeed();

//            fn.Id = this.Id;
//            fn.HasReceivingSSI = this.HasReceivingSSI;
//            fn.HasReceivingSSIDetailId = this.HasReceivingSSIDetailId;
//            fn.HasApplyingSSI = this.HasApplyingSSI;
//            fn.HasApplyingSSIDetailId = this.HasApplyingSSIDetailId;
//            fn.HasProcessApplyingSSI = this.HasProcessApplyingSSI;
//            fn.ApplicationStatusTypeId = this.ApplicationStatusTypeId;
//            fn.ApplicationStatusDetailId = this.ApplicationStatusDetailId;
//            fn.AppliedDate = this.AppliedDate;
//            fn.HasOtherPersonsHelp = this.HasOtherPersonsHelp;
//            fn.HasOtherPersonsHelpDetailId = this.HasOtherPersonsHelpDetailId;
//            fn.HasDeniedSSI = this.HasDeniedSSI;
//            fn.DenialDate = this.DenialDate;
//            fn.HasDeniedSSIDetailId = this.HasDeniedSSIDetailId;
//            fn.HasReceivedPastSSI = this.HasReceivedPastSSI;
//            fn.HasReceivedPastSSIDetailId = this.HasReceivedPastSSIDetailId;
//            fn.NoLongerReceiveSSIDetailId = this.NoLongerReceiveSSIDetailId;
//            fn.HasInterestedInLearningDetailId = this.HasInterestedInLearningDetailId;
//            fn.HasInterestedInLearning = this.HasInterestedInLearning;
//            fn.HasReceivingSSIDetail = (FamilySupplementalSecurityIncomeNeedDetail)this.HasReceivingSSIDetail?.Clone();
//            fn.CaretakingResponsibilitiesDetail = (FamilySupplementalSecurityIncomeNeedDetail)this.CaretakingResponsibilitiesDetail?.Clone();
//            fn.HasApplyingSSIDetail = (FamilySupplementalSecurityIncomeNeedDetail)this.HasApplyingSSIDetail?.Clone();
//            fn.ApplicationStatusDetail = (FamilySupplementalSecurityIncomeNeedDetail)this.ApplicationStatusDetail?.Clone();
//            fn.HasOtherPersonsHelpDetail = (FamilySupplementalSecurityIncomeNeedDetail)this.HasOtherPersonsHelpDetail?.Clone();
//            fn.HasDeniedSSIDetail = (FamilySupplementalSecurityIncomeNeedDetail)this.HasDeniedSSIDetail?.Clone();
//            fn.HasReceivedPastSSIDetail = (FamilySupplementalSecurityIncomeNeedDetail)this.HasReceivedPastSSIDetail?.Clone();
//            fn.NoLongerReceiveSSIDetail = (FamilySupplementalSecurityIncomeNeedDetail)this.NoLongerReceiveSSIDetail?.Clone();
//            fn.HasInterestedInLearningDetail = (FamilySupplementalSecurityIncomeNeedDetail)this.HasInterestedInLearningDetail?.Clone();
//            return fn;
//        }

//        #endregion ICloneable

//        #region IEquatable<T>

//        public override bool Equals(object other)
//        {
//            if (other == null)
//                return false;

//            var obj = other as FamilySupplementalSecurityIncomeNeed;
//            return obj != null && Equals(obj);
//        }

//        public bool Equals(FamilySupplementalSecurityIncomeNeed other)
//        {
//            //Check whether the compared object is null.
//            if (Object.ReferenceEquals(other, null)) return false;

//            //Check whether the compared object references the same data.
//            if (Object.ReferenceEquals(this, other)) return true;

//            //Check whether the products' properties are equal.
//            if (!AdvEqual(Id, other.Id))
//                return false;
//            if (!AdvEqual(HasReceivingSSI, other.HasReceivingSSI))
//                return false;
//            if (!AdvEqual(HasReceivingSSIDetailId, other.HasReceivingSSIDetailId))
//                return false;
//            if (!AdvEqual(HasReceivingSSIDetail, other.HasReceivingSSIDetail))
//                return false;
//            if (!AdvEqual(HasApplyingSSI, other.HasApplyingSSI))
//                return false;
//            if (!AdvEqual(HasApplyingSSIDetailId, other.HasApplyingSSIDetailId))
//                return false;
//            if (!AdvEqual(HasApplyingSSIDetail, other.HasApplyingSSIDetail))
//                return false;
//            if (!AdvEqual(HasProcessApplyingSSI, other.HasProcessApplyingSSI))
//                return false;
//            if (!AdvEqual(ApplicationStatusTypeId, other.ApplicationStatusTypeId))
//                return false;
//            if (!AdvEqual(ApplicationStatusDetailId, other.ApplicationStatusDetailId))
//                return false;
//            if (!AdvEqual(ApplicationStatusDetail, other.ApplicationStatusDetail))
//                return false;
//            if (!AdvEqual(AppliedDate, other.AppliedDate))
//                return false;
//            if (!AdvEqual(HasOtherPersonsHelp, other.HasOtherPersonsHelp))
//                return false;
//            if (!AdvEqual(HasOtherPersonsHelpDetailId, other.HasOtherPersonsHelpDetailId))
//                return false;
//            if (!AdvEqual(HasOtherPersonsHelpDetail, other.HasOtherPersonsHelpDetail))
//                return false;
//            if (!AdvEqual(HasDeniedSSI, other.HasDeniedSSI))
//                return false;
//            if (!AdvEqual(DenialDate, other.DenialDate))
//                return false;
//            if (!AdvEqual(HasDeniedSSIDetailId, other.HasDeniedSSIDetailId))
//                return false;
//            if (!AdvEqual(HasDeniedSSIDetail, other.HasDeniedSSIDetail))
//                return false;
//            if (!AdvEqual(HasReceivedPastSSI, other.HasReceivedPastSSI))
//                return false;
//            if (!AdvEqual(HasReceivedPastSSIDetailId, other.HasReceivedPastSSIDetailId))
//                return false;
//            if (!AdvEqual(HasReceivedPastSSIDetail, other.HasReceivedPastSSIDetail))
//                return false;
//            if (!AdvEqual(NoLongerReceiveSSIDetailId, other.NoLongerReceiveSSIDetailId))
//                return false;
//            if (!AdvEqual(NoLongerReceiveSSIDetail, other.NoLongerReceiveSSIDetail))
//                return false;
//            if (!AdvEqual(HasInterestedInLearningDetailId, other.HasInterestedInLearningDetailId))
//                return false;
//            if (!AdvEqual(HasInterestedInLearningDetail, other.HasInterestedInLearningDetail))
//                return false;
//            if (!AdvEqual(HasInterestedInLearning, other.HasInterestedInLearning))
//                return false;
          

//            return true;
//        }

//        #endregion IEquatable<T>
//    }
//}
