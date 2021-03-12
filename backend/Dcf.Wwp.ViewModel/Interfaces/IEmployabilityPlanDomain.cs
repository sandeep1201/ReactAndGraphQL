using System;
using System.Collections.Generic;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Api.Library.Interfaces
{
    public interface IEmployabilityPlanDomain
    {
        #region Properties

        #endregion

        #region Methods

        List<EmployabilityPlanContract> GetEmployabilityPlans(string           pin);
        DocumentResponse                GetDocumentsForPin(string              pin);
        EmployabilityPlanContract       GetPlanById(int                        epId);
        bool                            DeletePlan(string                      pin,                       int      epId, bool isVoid, bool isAutoDeleted, bool epTransfer);
        EmployabilityPlanContract       SubmitPlan(string                      pin,                       int      epId);
        EndEPContract                   EndEP(EndEPContract                    contract,                  string   pin,          int                       epId);
        PreCheckEPContract              PreSaveCheck(int                       partId,                    bool     submittingEP, EmployabilityPlanContract contract);
        EmployabilityPlanContract       UpsertPlan(EmployabilityPlanContract   employabilityPlanContract, string   pin,          int                       subsequentEPId);
        void                            EPTransfer(IParticipantEnrolledProgram pep,                       DateTime modifiedDate);
        string                          GetChildCareAuthorizationsByPin(string pin);

        #endregion
    }
}
