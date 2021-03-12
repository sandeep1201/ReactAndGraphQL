using AutoMapper;
using Dcf.Wwp.Api.Library.AutoMapperConfigs.Resolvers;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.Api.Library.AutoMapperConfigs.Profiles
{
    public class PlanProfile : Profile
    {
        public PlanProfile()
        {
            CreateMap<Plan, PlanContract>()
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom<WIUIDToFullNameModifiedByResolver>())
                .Ignore(dest => dest.OrganizationCode)
                .Ignore(dest => dest.OrganizationName)
                .Ignore(dest => dest.PlanSections)
                .AfterMap((src, dest) =>
                          {
                              var office            = src.Organization;
                              dest.OrganizationCode = office.EntsecAgencyCode;
                              dest.OrganizationName = office.AgencyName;
                          });
            CreateMap<PlanSection, PlanSectionContract>()
                .ForMember(dest => dest.ModifiedBy,            opt => opt.MapFrom<WIUIDToFullNameModifiedByResolver>())
                .ForMember(dest => dest.PlanSectionTypeId,     opt => opt.MapFrom(src => src.PlanSectionTypeId))
                .ForMember(dest => dest.PlanSectionTypeCode,   opt => opt.MapFrom(src => src.PlanSectionType.Code))
                .ForMember(dest => dest.PlanSectionTypeName,   opt => opt.MapFrom(src => src.PlanSectionType.Name))
                .ForMember(dest => dest.LongTermPlanOfAction,  opt => opt.MapFrom(src => src.LongTermPlanOfAction))
                .ForMember(dest => dest.ShortTermPlanOfAction, opt => opt.MapFrom(src => src.ShortTermPlanOfAction))
                .Ignore(dest => dest.PlanSectionResources)
                .AfterMap((src, dest) =>
                          {
                              dest.PlanId                = src.PlanId;
                              dest.PlanSectionTypeId     = src.PlanSectionTypeId;
                              dest.PlanSectionTypeCode   = src.PlanSectionType.Code;
                              dest.PlanSectionTypeName   = src.PlanSectionType.Name;
                              dest.IsNotNeeded           = src.IsNotNeeded;
                              dest.LongTermPlanOfAction  = src.LongTermPlanOfAction;
                              dest.ShortTermPlanOfAction = src.ShortTermPlanOfAction;
                              dest.ModifiedBy            = src.ModifiedBy;
                              dest.ModifiedDate          = src.ModifiedDate;
                          });
            CreateMap<PlanSectionResource, PlanSectionResourceContract>()
                .ForMember(dest => dest.Resource,     opt => opt.MapFrom(src => src.Resource))
                .ForMember(dest => dest.ModifiedBy,   opt => opt.MapFrom<WIUIDToFullNameModifiedByResolver>())
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => src.ModifiedDate))
                .AfterMap((src, dest) =>
                          {
                              dest.Resource      = src.Resource;
                              dest.ModifiedBy    = src.ModifiedBy;
                              dest.ModifiedDate  = src.ModifiedDate;
                              dest.Id            = src.Id;
                              dest.PlanSectionId = src.PlanSectionId;
                          });
        }
    }
}
