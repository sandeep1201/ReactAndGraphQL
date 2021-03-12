using AutoMapper;
using Dcf.Wwp.Api.Library.AutoMapperConfigs.Resolvers;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.Api.Library.AutoMapperConfigs.Profiles
{
    public class ParticipantPaymentHistoryProfile : Profile
    {
        #region Properties

        #endregion

        #region Methods

        public ParticipantPaymentHistoryProfile()
        {
            CreateMap<ParticipantPaymentHistory, ParticipantPaymentHistoryContract>()
                .ForMember(dest => dest.IsDelayed, opt => opt.MapFrom(src => src.ParticipationEndDate.Day != 15))
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom<WIUIDToFullNameModifiedByResolver>());
        }

        #endregion
    }
}
