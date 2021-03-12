using AutoMapper;
using Dcf.Wwp.Api.Library.AutoMapperConfigs.Resolvers;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.Api.Library.AutoMapperConfigs.Profiles
{
    public class WeeklyHoursWorkedProfile : Profile
    {
        #region Properties

        #endregion

        #region Methods

        public WeeklyHoursWorkedProfile()
        {
            CreateMap<WeeklyHoursWorked, WeeklyHoursWorkedContract>()
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom<WIUIDToFullNameModifiedByResolver>());

            CreateMap<WeeklyHoursWorkedContract, WeeklyHoursWorked>()
                .Ignore(dest => dest.TotalSubsidyAmount)
                .Ignore(dest => dest.EmploymentInformation)
                .Ignore(dest => dest.ModifiedBy)
                .Ignore(dest => dest.ModifiedDate)
                .Ignore(dest => dest.RowVersion);
        }

        #endregion
    }
}
