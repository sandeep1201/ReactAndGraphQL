using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.UnitTest.Infrastructure;

namespace Dcf.Wwp.UnitTest.Repositories
{
    public class MockEmploymentInformationRepository: MockRepositoryBase<EmploymentInformation>, IEmploymentInformationRepository
    {
        #region Properties

        public bool IsPOPClaim = false;
        public bool addMultipleAbsences = false;
        public bool lengthOfEmploymentMoreThan93 = false;
        public bool addMultipleEmployments = false;

        #endregion

        #region Methods

        public new IEnumerable<EmploymentInformation> GetMany(Expression<Func<EmploymentInformation, bool>> clause)
        {
            var employments = new List<EmploymentInformation>();

            if (IsPOPClaim)
            {
                var absences = new List<Absence>
                               {
                                   new Absence
                                   {
                                       AbsenceReasonId         = 1,
                                       EmploymentInformationId = 1,
                                       BeginDate               = new DateTime(2020, 08, 20),
                                       EndDate                 = new DateTime(2020, 08, 31),
                                       IsDeleted               = false
                                   }
                               };
                if (addMultipleAbsences)
                {
                    var anotherAbsence = new Absence
                                         {
                                             AbsenceReasonId         = 2,
                                             EmploymentInformationId = 1,
                                             BeginDate               = new DateTime(2020, 09, 05),
                                             EndDate                 = new DateTime(2020, 09, 10),
                                             IsDeleted               = false

                    };
                    absences.Add(anotherAbsence);
                }

              
                var employment = new EmploymentInformation
                                 {
                                     Absences = absences,
                                     Id = 1,
                                     ParticipantId = 123,
                                     JobBeginDate = new DateTime(2020, 07, 20),
                                     
                                     JobEndDate   = lengthOfEmploymentMoreThan93 ? new DateTime(2020, 12, 20) :  new DateTime(2020, 08, 20),
                };
                var employment2 = new EmploymentInformation
                                 {
                                     Id            = 1,
                                     ParticipantId = 123,
                                     JobBeginDate  = new DateTime(2020, 07, 25),
                                     JobEndDate = lengthOfEmploymentMoreThan93 ? new DateTime(2020, 12, 20) : new DateTime(2020, 08, 20),
                                 };

                employments.Add(employment);
                if (addMultipleEmployments)
                {
                    employments.Add(employment2);
                }
            }

            return employments;
        }

        public new EmploymentInformation Get(Expression<Func<EmploymentInformation, bool>> clause)
        {
            var employmentInformation = new EmploymentInformation()
                                   {
                                       Id = 1,
                                       WageHour = new WageHour
                                                  {
                                                      Id=1,
                                                      WageHourHistories = new List<WageHourHistory>
                                                                          {
                                                                              new WageHourHistory
                                                                              {
                                                                                  WageHourId = 1,
                                                                                  ComputedWageRateUnit = "Hour",
                                                                                  ComputedWageRateValue = 15.0M,
                                                                              }
                                                                          }
                                                  }
                                   };
            return employmentInformation;
        }
        #endregion
    }
}
