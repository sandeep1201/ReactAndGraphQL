using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class PlanContract
    {
        public int                       Id                 { get; set; }
        public int                       PlanNumber         { get; set; }
        public int                       ParticipantId      { get; set; }
        public int?                      PlanTypeId         { get; set; }
        public string                    PlanTypeCode       { get; set; }
        public string                    PlanTypeName       { get; set; }
        public int?                      PlanStatusTypeId   { get; set; }
        public string                    PlanStatusTypeCode { get; set; }
        public string                    PlanStatusTypeName { get; set; }
        public int?                      OrganizationId     { get; set; }
        public string                    OrganizationCode   { get; set; }
        public string                    OrganizationName   { get; set; }
        public DateTime                  ModifiedDate       { get; set; }
        public string                    ModifiedBy         { get; set; }
        public List<PlanSectionContract> PlanSections       { get; set; }
    }

    public class PlanSectionContract
    {
        public int                               Id                    { get; set; }
        public int                               PlanTypeId            { get; set; }
        public int                               PlanId                { get; set; }
        public int                               PlanSectionTypeId     { get; set; }
        public string                            PlanSectionTypeCode   { get; set; }
        public string                            PlanSectionTypeName   { get; set; }
        public bool                              IsNotNeeded           { get; set; }
        public string                            ShortTermPlanOfAction { get; set; }
        public string                            LongTermPlanOfAction  { get; set; }
        public DateTime                          ModifiedDate          { get; set; }
        public string                            ModifiedBy            { get; set; }
        public List<PlanSectionResourceContract> PlanSectionResources  { get; set; }
    }

    public class PlanSectionResourceContract
    {
        public int      Id            { get; set; }
        public int?     PlanSectionId { get; set; }
        public string   Resource      { get; set; }
        public DateTime ModifiedDate  { get; set; }
        public string   ModifiedBy    { get; set; }
    }
}
