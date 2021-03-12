using System.Linq;
using AutoMapper;
using Dcf.Wwp.Api.Library.AutoMapperConfigs.Resolvers;
using Dcf.Wwp.Api.Library.Contracts.EmergencyAssistance;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.Api.Library.AutoMapperConfigs.Profiles
{
    public class EmergencyAssistanceProfile : Profile
    {
        #region Properties

        #endregion

        #region Methods

        public EmergencyAssistanceProfile()
        {
            CreateMap<EARequest, EARequestContract>()
                .Ignore(src => src.EaDemographics)
                .Ignore(src => src.EaEmergencyType)
                .Ignore(src => src.EaGroupMembers)
                .Ignore(src => src.EaComments)
                .Ignore(src => src.EaPayments)
                .Ignore(src => src.EaHouseHoldFinancials)
                .Ignore(src => src.EaAgencySummary)
                .Ignore(dest => dest.StatusId)
                .Ignore(dest => dest.StatusCode)
                .Ignore(dest => dest.StatusName)
                .Ignore(dest => dest.StatusReasonIds)
                .Ignore(dest => dest.StatusReasonCodes)
                .Ignore(dest => dest.StatusReasonNames)
                .Ignore(dest => dest.StatusLastUpdated)
                .Ignore(dest => dest.StatusDeadLine)
                .Ignore(dest => dest.OrganizationCode)
                .Ignore(dest => dest.OrganizationName);

            CreateMap<EAIPV, EAIPVContract>()
                .Ignore(src => src.MailingAddress)
                .Ignore(dest => dest.OrganizationCode)
                .Ignore(dest => dest.ReasonIds)
                .Ignore(dest => dest.ReasonCodes)
                .Ignore(dest => dest.ReasonNames)
                .Ignore(dest => dest.CountyName)
                .Ignore(dest => dest.OverTurnedDate)
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom<WIUIDToFullNameModifiedByResolver>())
                .AfterMap((src, dest) =>
                          {
                              var eaIpvReasonBridges       = src.EaIpvReasonBridges.Where(i => !i.IsDeleted).ToList();
                              var eaIpvReasonBridgeReasons = eaIpvReasonBridges.Select(i => i.Reason).ToList();

                              dest.ReasonIds   = eaIpvReasonBridges.Select(i => i.ReasonId).ToList();
                              dest.ReasonCodes = eaIpvReasonBridgeReasons.Select(i => i.Code).ToList();
                              dest.ReasonNames = eaIpvReasonBridgeReasons.Select(i => i.Name).ToList();
                          });

            CreateMap<EAIPVContract, EAIPV>()
                .Ignore(src => src.Participant)
                .Ignore(src => src.Organization)
                .Ignore(src => src.Occurrence)
                .Ignore(src => src.Status)
                .Ignore(src => src.County)
                .Ignore(src => src.EaIpvReasonBridges)
                .Ignore(src => src.EaAlternateMailingAddress)
                .Ignore(src => src.RowVersion)
                .Ignore(src => src.ParticipantId)
                .Ignore(src => src.MailingAddressId)
                .Ignore(src => src.OrganizationId)
                .Ignore(src => src.IPVNumber);

            CreateMap<EAPayment, EAPaymentContract>()
                .Ignore(src => src.MailingAddress)
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom<WIUIDToFullNameModifiedByResolver>());

            CreateMap<EAPaymentContract, EAPayment>()
                .Ignore(src => src.EaAlternateMailingAddress)
                .Ignore(src => src.EaRequest)
                .Ignore(src => src.RowVersion)
                .Ignore(src => src.MailingAddressId);
        }

        #endregion
    }
}
