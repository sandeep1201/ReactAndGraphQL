using System;
using AutoMapper;
using Dcf.Wwp.Api.Library.AutoMapperConfigs.Resolvers;
using Dcf.Wwp.Api.Library.Contracts;
using Phase1Transaction = Dcf.Wwp.Data.Sql.Model.Transaction;
using Transaction = Dcf.Wwp.DataAccess.Models.Transaction;

namespace Dcf.Wwp.Api.Library.AutoMapperConfigs.Profiles
{
    public class TransactionProfile : Profile
    {
        #region Properties

        #endregion

        #region Methods

        public TransactionProfile()
        {
            CreateMap<Transaction, TransactionContract>()
                .Ignore(dest => dest.AgencyId)
                .Ignore(dest => dest.AgencyName)
                .Ignore(dest => dest.CountyName)
                .Ignore(dest => dest.StatusCode)
                .ForMember(dest => dest.WorkerName, opt => opt.MapFrom<SetWorkerNameFromWorkerIdResolver>())
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom<WIUIDToFullNameModifiedByResolver>())
                .AfterMap((src, dest) =>
                          {
                              var office = src.Office;

                              if (office != null)
                              {
                                  var organization = office.ContractArea.Organization;

                                  dest.AgencyId   = organization.Id;
                                  dest.AgencyName = organization.AgencyName;
                                  dest.CountyName = office.CountyAndTribe.CountyName;
                              }
                          });

            CreateMap<TransactionContract, Phase1Transaction>()
                .ForMember(dest => dest.ModifiedBy,   opt => opt.MapFrom<SetModifiedDetailsResolver>())
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.Now))
                .Ignore(dest => dest.Participant)
                .Ignore(dest => dest.Worker)
                .Ignore(dest => dest.Office)
                .Ignore(dest => dest.RowVersion)
                .Ignore(dest => dest.TransactionTypeId)
                .Ignore(dest => dest.Description);

            CreateMap<TransactionContract, Transaction>()
                .ForMember(dest => dest.ModifiedBy,   opt => opt.MapFrom<SetModifiedDetailsResolver>())
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.Now))
                .Ignore(dest => dest.Participant)
                .Ignore(dest => dest.Worker)
                .Ignore(dest => dest.Office)
                .Ignore(dest => dest.RowVersion)
                .Ignore(dest => dest.TransactionType)
                .Ignore(dest => dest.TransactionTypeId)
                .Ignore(dest => dest.Description);
        }

        #endregion
    }
}
