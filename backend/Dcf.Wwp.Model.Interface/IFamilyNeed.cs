//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Dcf.Wwp.Model.Interface
//{
//    public interface IFamilyNeed : ICommonModel,ICloneable
//    {
//        Nullable<bool> HasHealthProblem { get; set; }
//        int? HasHealthProblemDetailId { get; set; }
//        Nullable<bool> HasHealthConcerns { get; set; }
//        int? HasHealthConcernsDetailId { get; set; }
//        Nullable<bool> HasRiskBehavior { get; set; }
//        int? HasRiskBehaviorDetailId { get; set; }
//        Nullable<bool> HasChildrenBehaviorProblem { get; set; }
//        int? HasChildrenBehaviorProblemDetailId { get; set; }
//        Nullable<bool> HasChildrenSchoolExpulsionRisk { get; set; }
//        int? HasChildrenSchoolExpulsionRiskDetailId { get; set; }
//        Nullable<bool> HasFamilyIssuesInhibitWork { get; set; }
//        int? HasFamilyIssuesInhibitWorkDetailId { get; set; }
//        Nullable<bool> IsDeleted { get; set; }

//        ICollection<IFamilyBarriersSection> FamilyBarriersSections { get; set; }
//        IFamilyNeedDetail HasChildrenBehaviorProblemDetail { get; set; }
//        IFamilyNeedDetail HasChildrenSchoolExpulsionRiskDetail { get; set; }
//        IFamilyNeedDetail HasHealthConcernsDetail { get; set; }
//        IFamilyNeedDetail HasHealthProblemDetail { get; set; }
//        IFamilyNeedDetail HasRiskBehaviorDetail { get; set; }
//        IFamilyNeedDetail HasFamilyIssuesInhibitWorkDetail { get; set; }
//    }
//}
