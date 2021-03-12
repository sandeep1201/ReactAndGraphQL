using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Dcf.Wwp.Api.Library.AutoMapperConfigs.Profiles;
using Dcf.Wwp.Api.Library.AutoMapperConfigs.Resolvers;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.UnitTest.Api.AutoMapper.Resolver;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dcf.Wwp.UnitTest.Api.AutoMapper
{
    [TestClass]
    public class WeeklyHoursWorkedProfileTest
    {
        #region Properties

        private MockWIUIDToFullNameModifiedByResolver _WIUIDToFullNameModifiedByResolver;
        private MapperConfiguration                   _config;
        private IMapper                               _mapper;
        private MapperBaseTest                        _mapperBaseTest;

        #endregion

        #region Methods

        [TestInitialize]
        public void Initialize()
        {
            _mapperBaseTest                    = new MapperBaseTest();
            _WIUIDToFullNameModifiedByResolver = new MockWIUIDToFullNameModifiedByResolver();
            _config = new MapperConfiguration(cfg =>
                                              {
                                                  cfg.AddProfile<WeeklyHoursWorkedProfile>();

                                                  cfg.ConstructServicesUsing(i =>
                                                                             {
                                                                                 if (i.Name.Contains(nameof(WIUIDToFullNameModifiedByResolver)))
                                                                                     return _WIUIDToFullNameModifiedByResolver;

                                                                                 return null;
                                                                             });
                                              });
            _mapper = _config.CreateMapper();
        }

        [TestMethod]
        public void AutoMapper_Configuration_IsValid()
        {
            _config.AssertConfigurationIsValid();
        }

        [TestMethod]
        public void WeeklyHoursWorkedProfileTestMappingEmptyEntityToContractForValueEquality()
        {
            var entity   = new WeeklyHoursWorked();
            var contract = _mapper.Map<WeeklyHoursWorkedContract>(entity);

            _mapperBaseTest.AssertAllPropertiesMapped(entity, contract, new List<string>
                                                                        {
                                                                            nameof(WeeklyHoursWorked.ModifiedBy),
                                                                            nameof(WeeklyHoursWorked.TotalWorkSiteAmount)
                                                                        });

            Assert.AreEqual(_WIUIDToFullNameModifiedByResolver.ResolveString, contract.ModifiedBy);
            Assert.AreEqual(entity.TotalWorkSiteAmount.GetValueOrDefault(),   contract.TotalWorkSiteAmount);
        }

        [TestMethod]
        public void WeeklyHoursWorkedProfileTestMappingNonEmptyEntityToContractForValueEquality()
        {
            var entity = new WeeklyHoursWorked
                         {
                             Id                      = 1,
                             StartDate               = new DateTime(2020, 10, 30),
                             Details                 = "DCF",
                             EmploymentInformationId = 100,
                             Hours                   = 10,
                             ModifiedBy              = "123456",
                             ModifiedDate            = DateTime.Now,
                             IsDeleted               = false,
                             TotalSubsidyAmount      = 200.50m,
                             TotalWorkSiteAmount     = 150.75m
                         };
            var contract = _mapper.Map<WeeklyHoursWorkedContract>(entity);

            _mapperBaseTest.AssertAllPropertiesMapped(entity, contract, new List<string> { nameof(WeeklyHoursWorked.ModifiedBy) });

            Assert.AreEqual(_WIUIDToFullNameModifiedByResolver.ResolveString, contract.ModifiedBy);
        }

        [TestMethod]
        public void WeeklyHoursWorkedProfileTestMappingEmptyContractToEntityForValueEquality()
        {
            var contract = new WeeklyHoursWorkedContract();
            var entity   = _mapper.Map<WeeklyHoursWorked>(contract);

            _mapperBaseTest.AssertAllPropertiesMapped(contract, entity, new List<string>());
        }

        [TestMethod]
        public void WeeklyHoursWorkedProfileTestMappingNonEmptyContractToEntityForValueEquality()
        {
            var contract = new WeeklyHoursWorkedContract
                           {
                               Id                      = 1,
                               StartDate               = new DateTime(2020, 10, 30),
                               Details                 = "DCF",
                               EmploymentInformationId = 100,
                               Hours                   = 10,
                               ModifiedBy              = "123456",
                               ModifiedDate            = DateTime.Now,
                               IsDeleted               = false,
                               TotalSubsidyAmount      = 200.50m,
                               TotalWorkSiteAmount     = 150.75m
                           };
            var entity = _mapper.Map<WeeklyHoursWorked>(contract);

            _mapperBaseTest.AssertAllPropertiesMapped(contract, entity, new List<string>
                                                                        {
                                                                            nameof(WeeklyHoursWorked.ModifiedBy),
                                                                            nameof(WeeklyHoursWorked.ModifiedDate),
                                                                            nameof(WeeklyHoursWorked.TotalSubsidyAmount),
                                                                            nameof(WeeklyHoursWorked.RowVersion)
                                                                        });

            Assert.AreEqual(default(string),   entity.ModifiedBy);
            Assert.AreEqual(default(DateTime), entity.ModifiedDate);
            Assert.AreEqual(default(decimal),  entity.TotalSubsidyAmount);
            Assert.AreEqual(0,                 entity.RowVersion.Select(i => (int) i).Sum());
        }

        #endregion
    }
}
