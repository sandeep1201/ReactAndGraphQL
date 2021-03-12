 
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Contracts.InformalAssessment;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Api.Library.Utils
{
    public static class NCRHelper
    {
        public static List<NonCustodialReferralChildContract> GetNCRChildren(INonCustodialReferralParent parent)
        {
            return parent.NonCustodialReferralChilds.Where(x => x.DeleteReasonId == null)
                .Select(child => new NonCustodialReferralChildContract
                {
                    Id = child.Id,
                    FirstName = child.FirstName,
                    LastName = child.LastName,
                    HasChildSupportOrder = child.HasChildSupportOrder,
                    ChildSupportOrderDetails = child.ChildSupportOrderDetails,
                    ContactIntervalId = child.ReferralContactIntervalId,
                    ContactIntervalName = child.ReferralContactInterval?.Name,
                    ContactIntervalDetails = child.ContactIntervalDetails,
                    RowVersion = child.RowVersion
                })
                .ToList();
        }

        public static NonCustodialReferralParentContract GetNCRParent(INonCustodialReferralParent parent)
        {
            return new NonCustodialReferralParentContract()
            {
                Id = parent.Id,
                FirstName = parent.FirstName,
                LastName = parent.LastName,
                IsAvailableOrWorking = parent.IsAvailableOrWorking,
                AvailableOrWorkingDetails = parent.AvailableOrWorkingDetails,
                IsInterestedInWorkProgram = parent.IsInterestedInWorkProgram,
                InterestedInWorkProgramDetails = parent.InterestedInWorkProgramDetails,
                IsContactKnownWithParent = parent.IsContactKnownWithParent,
                ContactId = parent.ContactId,
                RowVersion = parent.RowVersion
            };
        }
    }
}
