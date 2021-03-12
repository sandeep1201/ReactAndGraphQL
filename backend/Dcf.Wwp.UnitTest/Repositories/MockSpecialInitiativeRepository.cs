using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.Model.Interface.Constants;
using Dcf.Wwp.UnitTest.Infrastructure;

namespace Dcf.Wwp.UnitTest.Repositories
{
    public class MockSpecialInitiativeRepository : MockRepositoryBase<SpecialInitiative>, ISpecialInitiativeRepository
    {
        public  bool IsFeatureToggleDateOlderThanSixMonthsFromToday = true;
        private readonly SpecialInitiative _fakeValidFeatureDate = new SpecialInitiative
                                                                   {
                                                                       Id             = 19,
                                                                       ParameterName  = "WorkerTaskList",
                                                                       ParameterValue = "10-10-2020"
                                                                   };

        private readonly SpecialInitiative _fakeValidEPCutOverValidationDate = new SpecialInitiative
                                                                               {
                                                                                   Id             = 19,
                                                                                   ParameterName  = SpecialInitiatives.EPCutOverValidationDate,
                                                                                   ParameterValue =  DateTime.Now.AddMonths(-7).ToString("MM/dd/yyyy")
                                                                               };

        private readonly SpecialInitiative _fakeValidEPCutOverValidationDate2 = new SpecialInitiative
                                                                                {
                                                                                    Id             = 19,
                                                                                    ParameterName  = SpecialInitiatives.EPCutOverValidationDate,
                                                                                    ParameterValue = DateTime.Now.ToString("MM/dd/yyyy")
                                                                                };

        private readonly SpecialInitiative _fakeValidPOPClaimsDate = new SpecialInitiative
                                                                     {
                                                                         Id             = 19,
                                                                         ParameterName  = SpecialInitiatives.POPClaims,
                                                                         ParameterValue = DateTime.Now.ToString("MM/dd/yyyy")
                                                                     };


        public new SpecialInitiative Get(Expression<Func<SpecialInitiative, bool>> clause)
        {
            var fakeInValidFeatureDate = new SpecialInitiative
                                         {
                                             Id             = 40,
                                             ParameterName  = "TEST",
                                             ParameterValue = "10-10-2040"
                                         };
            var fakeData = new List<SpecialInitiative> { _fakeValidFeatureDate, fakeInValidFeatureDate, _fakeValidPOPClaimsDate , IsFeatureToggleDateOlderThanSixMonthsFromToday ? _fakeValidEPCutOverValidationDate : _fakeValidEPCutOverValidationDate2 };


            return fakeData.FirstOrDefault(clause.Compile());
        }
    }
}
