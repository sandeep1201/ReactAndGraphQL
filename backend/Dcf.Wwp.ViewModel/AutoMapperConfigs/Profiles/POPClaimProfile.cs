using System.Linq;
using AutoMapper;
using Dcf.Wwp.Api.Library.AutoMapperConfigs.Resolvers;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.Api.Library.AutoMapperConfigs.Profiles
{
    public class POPClaimProfile : Profile
    {
        #region Properties

        #endregion

        #region Methods

        public POPClaimProfile()
        {
            CreateMap<POPClaim, POPClaimContract>()
                .ForMember(dest => dest.ModifiedBy,           opt => opt.MapFrom<WIUIDToFullNameModifiedByResolver>())
                .ForMember(dest => dest.POPClaimEmployments,  opt => opt.MapFrom(src => src.POPClaimEmploymentBridges))
                .ForMember(dest => dest.ClaimPeriodBeginDate, opt => opt.MapFrom(src => src.ClaimPeriodBeginDate))
                .Ignore(dest => dest.IsDeleted)
                .Ignore(dest => dest.ClaimStatusTypeId)
                .Ignore(dest => dest.ClaimStatusTypeCode)
                .Ignore(dest => dest.ClaimStatusTypeDisplayName)
                .Ignore(dest => dest.IsSubmit)
                .Ignore(dest => dest.IsWithdraw)
                .Ignore(dest => dest.Details)
                .Ignore(dest => dest.ClaimStatusTypeName)
                .Ignore(dest => dest.ClaimStatusDate)
                .Ignore(dest => dest.AgencyCode)
                .Ignore(dest => dest.AgencyName)
                .Ignore(dest => dest.PinNumber)
                .Ignore(dest => dest.ParticipantName)
                .Ignore(dest => dest.FirstName)
                .Ignore(dest => dest.MiddleInitial)
                .Ignore(dest => dest.LastName)
                .Ignore(dest => dest.SuffixName)
                .Ignore(dest => dest.POPClaimTypeName)
                .Ignore(dest => dest.ActivityId)
                .Ignore(dest => dest.ActivityCode)
                .Ignore(dest => dest.ActivityBeginDate)
                .Ignore(dest => dest.ActivityEndDate)
                .AfterMap((src, dest) =>
                          {
                              var participant  = src.Participant;
                              var organization = src.Organization;
                              var status       = src.POPClaimStatuses.OrderByDescending(i => i.Id).FirstOrDefault();
                              var activity     = src.POPClaimActivityBridges.FirstOrDefault();

                              dest.POPClaimTypeName = src.POPClaimType?.Description;

                              if (participant != null)
                              {
                                  if (participant.PinNumber != null)
                                      dest.PinNumber = (decimal) participant.PinNumber;
                                  dest.ParticipantName = participant.DisplayName;
                              }


                              if (organization != null)
                              {
                                  dest.AgencyCode = organization.EntsecAgencyCode;
                                  dest.AgencyName = organization.AgencyName;
                              }

                              if (status != null)
                              {
                                  var statusType = status.POPClaimStatusType;

                                  dest.ClaimStatusTypeDisplayName = statusType.DisplayName;
                                  dest.ClaimStatusDate            = status.POPClaimStatusDate;
                                  dest.ClaimStatusTypeCode        = statusType.Code;
                                  dest.ClaimStatusTypeId          = status.POPClaimStatusTypeId;
                              }

                              if (activity == null) return;
                              dest.ActivityId        = activity.ActivityId;
                              dest.ActivityCode      = activity.Activity.ActivityType.Code;
                              dest.ActivityBeginDate = activity.Activity.StartDate;
                              dest.ActivityEndDate   = activity.Activity.EndDate;
                          });

            CreateMap<POPClaimEmploymentBridge, POPClaimEmploymentContract>()
                .ForMember(dest => dest.IsSelected,              opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.IsPrimary,               opt => opt.MapFrom(src => src.IsPrimary))
                .ForMember(dest => dest.EmploymentInformationId, opt => opt.MapFrom(src => src.EmploymentInformationId))
                .ForMember(dest => dest.ModifiedBy,              opt => opt.MapFrom<WIUIDToFullNameModifiedByResolver>())
                .ForMember(dest => dest.Earnings,                opt => opt.MapFrom(src => src.Earnings))
                .ForMember(dest => dest.HoursWorked,             opt => opt.MapFrom(src => src.HoursWorked))
                .Ignore(dest => dest.JobTypeId)
                .Ignore(dest => dest.JobTypeName)
                .Ignore(dest => dest.JobBeginDate)
                .Ignore(dest => dest.JobEndDate)
                .Ignore(dest => dest.JobPosition)
                .Ignore(dest => dest.CompanyName)
                .Ignore(dest => dest.DeletedReasonId)
                .Ignore(dest => dest.StartingWage)
                .Ignore(dest => dest.StartingWageUnit)
                .AfterMap((src, dest) =>
                          {
                              var employmentInformation = src.EmploymentInformation;

                              dest.JobTypeName  = employmentInformation.JobType.Name;
                              dest.JobTypeId    = employmentInformation.JobType.Id;
                              dest.CompanyName  = employmentInformation.CompanyName;
                              dest.JobBeginDate = employmentInformation.JobBeginDate?.ToString("MM/dd/yyyy");
                              dest.JobEndDate   = employmentInformation.JobEndDate?.ToString("MM/dd/yyyy");
                              dest.JobPosition  = employmentInformation.JobPosition;
                              dest.IsPrimary    = src.IsPrimary;
                          });

            CreateMap<POPClaimStatus, POPClaimStatusContract>()
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom<WIUIDToFullNameModifiedByResolver>())
                .Ignore(dest => dest.POPClaimStatusName)
                .Ignore(dest => dest.POPClaimStatusDisplayName)
                .AfterMap((src, dest) =>
                          {
                              var popClaimStatusType = src.POPClaimStatusType;

                              dest.POPClaimStatusDisplayName = popClaimStatusType.DisplayName;
                              dest.POPClaimStatusName        = popClaimStatusType.Name;
                          });
        }

        #endregion
    }
}
