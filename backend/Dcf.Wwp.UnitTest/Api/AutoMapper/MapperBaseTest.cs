using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Dcf.Wwp.Api.Library.AutoMapperConfigs.Profiles;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dcf.Wwp.UnitTest.Api.AutoMapper
{
    [TestClass]
    public class MapperBaseTest
    {
        #region Properties

        private MapperConfiguration _config;

        #endregion

        #region Methods

        [TestInitialize]
        public void Initialize()
        {
            var profiles = typeof(WeeklyHoursWorkedProfile).Assembly.GetTypes()
                                                           .Where(t => typeof(Profile).IsAssignableFrom(t))
                                                           .ToList();

            _config = new MapperConfiguration(cfg =>
                                              {
                                                  foreach (var profile in profiles)
                                                  {
                                                      cfg.AddProfile(Activator.CreateInstance(profile) as Profile);
                                                  }
                                              });
        }

        [TestMethod]
        public void AutoMapper_AllConfiguration_IsValid()
        {
            _config.AssertConfigurationIsValid();
        }

        public void AssertAllPropertiesMapped<TS, TD>(TS source, TD destination, List<string> ignoreProperties = null) where TS : class where TD : class
        {
            var sourceProperties      = source.GetType().GetProperties();
            var destinationProperties = destination.GetType().GetProperties();

            sourceProperties.Where(i => ignoreProperties == null || !ignoreProperties.Contains(i.Name))
                            .ForEach(sourceProperty =>
                                     {
                                         var destinationProperty = destinationProperties.FirstOrDefault(i => i.Name == sourceProperty.Name);

                                         if (destinationProperty != null)
                                             Assert.AreEqual(sourceProperty.GetValue(source), destinationProperty.GetValue(destination));
                                     });
        }

        #endregion
    }
}
