using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.Model.Interface.Core;

namespace Dcf.Wwp.Api.Library.Domains
{
    public class PlanDomain : IPlanDomain
    {
        #region Properties

        private readonly IPlanRepository _planRepository;
        private readonly IPlanSectionRepository _planSectionRepository;
        private readonly IPlanStatusTypeRepository _planStatusTypeRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IAuthUser       _authUser;
        private readonly IUnitOfWork     _unitOfWork;
        private readonly IMapper         _mapper;

        #endregion

        #region Methods

        public PlanDomain
        (
            IPlanRepository planRepository,
            IPlanSectionRepository planSectionRepository,
            IPlanStatusTypeRepository planStatusTypeRepository,
            IOrganizationRepository organizationRepository,
            IAuthUser       authUser,
            IUnitOfWork     unitOfWork,
            IMapper         mapper
        )
        {
            _planRepository = planRepository;
            _planSectionRepository = planSectionRepository;
            _planStatusTypeRepository = planStatusTypeRepository;
            _organizationRepository = organizationRepository;
            _authUser       = authUser;
            _unitOfWork     = unitOfWork;
            _mapper         = mapper;
        }

        public List<PlanContract> GetW2PlansByParticipantId(int id)
        {
            return _mapper.Map<List<PlanContract>>(_planRepository.GetMany(i => i.ParticipantId == id && !i.IsDeleted)?.OrderByDescending(i => i.PlanNumber));
        }

        public PlanSectionContract UpsertPlanSection(PlanSectionContract contract, int participantId)
        {
            if (contract == null)
                throw new ArgumentNullException(nameof(contract));
            var modifiedBy   = _authUser.WIUID;
            var modifiedDate = _authUser.CDODate ?? DateTime.Now;
            if (contract.PlanId == 0)
            {
                UpsertW2Plan(participantId ,contract,modifiedBy,modifiedDate);
            }
            
            var w2PlansSection = contract.Id == 0 ? _planSectionRepository.New() : _planSectionRepository.Get(i => i.Id == contract.Id && !i.IsDeleted);
            
            

            w2PlansSection.PlanSectionTypeId = contract.PlanSectionTypeId;
            w2PlansSection.ShortTermPlanOfAction = contract.ShortTermPlanOfAction;
            w2PlansSection.LongTermPlanOfAction = contract.LongTermPlanOfAction;
            w2PlansSection.IsNotNeeded = contract.IsNotNeeded;
            w2PlansSection.ModifiedBy = modifiedBy;
            w2PlansSection.ModifiedDate = modifiedDate;
            w2PlansSection.IsDeleted = false;
           

            if (w2PlansSection.Id == 0)
            {
                _planSectionRepository.Add(w2PlansSection);
            }

            _unitOfWork.Commit();

            return _mapper.Map<PlanSectionContract>(w2PlansSection);
        }

        private void UpsertW2Plan(int participantId, PlanSectionContract contract,string modifiedBy, DateTime modifiedDate)
        {
            var w2Plan = _planRepository.New();
            w2Plan.ParticipantId = participantId;
            w2Plan.PlanTypeid = contract.PlanTypeId;
            w2Plan.PlanStatusTypeid = _planStatusTypeRepository.Get(i => i.Name == "In Progress").Id;
            w2Plan.OrganizationId = _organizationRepository.Get(i => i.EntsecAgencyCode.ToLower().Trim() == _authUser.AgencyCode.ToLower().Trim()).Id;
            w2Plan.ModifiedBy = modifiedBy;
            w2Plan.IsDeleted = false;
            w2Plan.CreatedDate = modifiedDate;
            w2Plan.ModifiedDate = modifiedDate;
            _planRepository.Add(w2Plan);
        }

        #endregion
    }
}
