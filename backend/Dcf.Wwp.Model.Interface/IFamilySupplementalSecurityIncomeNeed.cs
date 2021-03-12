//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Dcf.Wwp.Model.Interface
//{
//    public interface IFamilySupplementalSecurityIncomeNeed : ICommonModel, ICloneable
//    {
//         Nullable<bool> HasReceivingSSI { get; set; }
//         Nullable<int> HasReceivingSSIDetailId { get; set; }
//         Nullable<int> CaretakingResponsibilitiesDetailId { get; set; }
//         Nullable<bool> HasApplyingSSI { get; set; }
//         Nullable<int> HasApplyingSSIDetailId { get; set; }
//         Nullable<bool> HasProcessApplyingSSI { get; set; }
//         Nullable<int> ApplicationStatusTypeId { get; set; }
//         Nullable<int> ApplicationStatusDetailId { get; set; }
//         Nullable<System.DateTime> AppliedDate { get; set; }
//         Nullable<int> ContactId { get; set; }
//         Nullable<bool> HasOtherPersonsHelp { get; set; }
//         Nullable<int> HasOtherPersonsHelpDetailId { get; set; }
//         Nullable<bool> HasDeniedSSI { get; set; }
//         Nullable<System.DateTime> DenialDate { get; set; }
//         Nullable<int> HasDeniedSSIDetailId { get; set; }
//         Nullable<bool> HasReceivedPastSSI { get; set; }
//         Nullable<int> HasReceivedPastSSIDetailId { get; set; }
//         Nullable<int> NoLongerReceiveSSIDetailId { get; set; }
//         Nullable<bool> HasInterestedInLearning { get; set; }
//         Nullable<int> HasInterestedInLearningDetailId { get; set; }
//        Nullable<bool> IsDeleted { get; set; }
//        ICollection<IFamilyBarriersSection> FamilyBarriersSections { get; set; }
//        IFamilySupplementalSecurityIncomeNeedDetail ApplicationStatusDetail { get; set; }
//        IFamilySupplementalSecurityIncomeNeedDetail CaretakingResponsibilitiesDetail { get; set; }
//        IFamilySupplementalSecurityIncomeNeedDetail HasApplyingSSIDetail { get; set; }
//        IFamilySupplementalSecurityIncomeNeedDetail HasDeniedSSIDetail { get; set; }
//        IFamilySupplementalSecurityIncomeNeedDetail HasInterestedInLearningDetail { get; set; }
//        IFamilySupplementalSecurityIncomeNeedDetail HasOtherPersonsHelpDetail { get; set; }
//        IFamilySupplementalSecurityIncomeNeedDetail HasReceivedPastSSIDetail { get; set; }
//        IFamilySupplementalSecurityIncomeNeedDetail HasReceivingSSIDetail { get; set; }
//        IFamilySupplementalSecurityIncomeNeedDetail NoLongerReceiveSSIDetail { get; set; }
//        IApplicationStatusType ApplicationStatusType { get; set; }
//        IContact Contact { get; set; }

//    }
//}
