using AutoMapper;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.Api.Library.AutoMapperConfigs.Profiles
{
    public class EmploymentVerificationProfile : Profile
    {
        public EmploymentVerificationProfile()
        {
            CreateMap<EmploymentInformation, EmploymentVerificationContract>()
                .ForMember(dest => dest.AvgWeeklyHours, opt => opt.MapFrom(src => src.WageHour.CurrentAverageWeeklyHours))
                .Ignore(dest => dest.CreatedDate)
                .Ignore(dest => dest.NumberOfDaysAtVerification)
                .Ignore(dest => dest.EmploymentVerificationId)
                .Ignore(dest => dest.IsVerified)
                .Ignore(dest => dest.ModifiedBy)
                .Ignore(dest => dest.Location)
                .AfterMap((src, dest) => dest.Location = new FinalistAddressContract
                                                         {
                                                             AddressLine1 = src.StreetAddress,
                                                             City         = src.City?.Name,
                                                             State        = src.City?.State?.Code,
                                                             Zip          = src.ZipAddress
                                                         });
        }
    }
}
