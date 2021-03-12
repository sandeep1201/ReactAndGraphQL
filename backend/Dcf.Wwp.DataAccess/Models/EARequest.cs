using System;
using System.Collections.Generic;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class EARequest : BaseEntity
    {
        #region Properties

        public decimal  RequestNumber                   { get; set; }
        public DateTime ApplicationDate                 { get; set; }
        public bool     IsDeleted                       { get; set; }
        public string   ModifiedBy                      { get; set; }
        public DateTime ModifiedDate                    { get; set; }
        public bool?    DidApplicantTakeCareOfAnyChild  { get; set; }
        public bool?    WillTheChildStayInApplicantCare { get; set; }
        public string   EmergencyDetails                { get; set; }
        public bool?    HasNoIncome                     { get; set; }
        public bool?    HasNoAssets                     { get; set; }
        public bool?    HasNoVehicles                   { get; set; }
        public decimal? ApprovedPaymentAmount           { get; set; }
        public bool?    IsPreviousMemberClicked         { get; set; }
        public decimal? CaresCaseNumber                 { get; set; }
        public int?     OrganizationId                  { get; set; }
        public int?     ApplicationInitiatedMethodId    { get; set; }
        public decimal? AccessTrackingNumber            { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Organization                              Organization                        { get; set; }
        public virtual EAApplicationInitiationMethodLookUp       EaApplicationInitiationMethodLookUp { get; set; }
        public virtual ICollection<EAAssets>                     EaAssetses                          { get; set; } = new List<EAAssets>();
        public virtual ICollection<EAVehicles>                   EaVehicleses                        { get; set; } = new List<EAVehicles>();
        public virtual ICollection<EAImpendingHomelessness>      EaImpendingHomelessnesses           { get; set; } = new List<EAImpendingHomelessness>();
        public virtual ICollection<EAHomelessness>               EaHomelessnesses                    { get; set; } = new List<EAHomelessness>();
        public virtual ICollection<EAHouseHoldIncome>            EaHouseHoldIncomes                  { get; set; } = new List<EAHouseHoldIncome>();
        public virtual ICollection<EAEnergyCrisis>               EaEnergyCrises                      { get; set; } = new List<EAEnergyCrisis>();
        public virtual ICollection<EARequestContactInfo>         EaRequestContactInfos               { get; set; } = new List<EARequestContactInfo>();
        public virtual ICollection<EAComment>                    EaComments                          { get; set; } = new List<EAComment>();
        public virtual ICollection<EARequestParticipantBridge>   EaRequestParticipantBridges         { get; set; } = new List<EARequestParticipantBridge>();
        public virtual ICollection<EARequestEmergencyTypeBridge> EaRequestEmergencyTypeBridges       { get; set; } = new List<EARequestEmergencyTypeBridge>();
        public virtual ICollection<EARequestStatus>              EaRequestStatuses                   { get; set; } = new List<EARequestStatus>();
        public virtual ICollection<EAPayment>                    EaPayments                          { get; set; } = new List<EAPayment>();
        public virtual ICollection<EAFinancialNeed>              EaFinancialNeeds                    { get; set; } = new List<EAFinancialNeed>();

        #endregion
    }
}
