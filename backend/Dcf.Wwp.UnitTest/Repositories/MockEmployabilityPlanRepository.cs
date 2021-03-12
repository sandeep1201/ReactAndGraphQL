using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.UnitTest.Infrastructure;

namespace Dcf.Wwp.UnitTest.Repositories
{
    public class MockEmployabilityPlanRepository : MockRepositoryBase<EmployabilityPlan>, IEmployabilityPlanRepository
    {
        #region Properties

        public bool IsPOPClaim;
        public bool GetHasBeenCalled;

        public EmployabilityPlan EmployabilityPlan = new EmployabilityPlan
                                                     {
                                                         Id            = 3,
                                                         ParticipantId = 1,
                                                         BeginDate     = DateTime.Now,
                                                         Organization  = new Organization(),
                                                         Participant   = new Participant
                                                                         {
                                                                             PinNumber = 12345678
                                                                         },
                                                         ParticipantEnrolledProgram = new ParticipantEnrolledProgram
                                                                                      {
                                                                                          OfficeId        = 1,
                                                                                          EnrolledProgram = new EnrolledProgram
                                                                                                            {
                                                                                                                ProgramCode = "WW"
                                                                                                            }
                                                                                      }
                                                     };

        #endregion

        #region Methods

        public new IEnumerable<EmployabilityPlan> GetMany(Expression<Func<EmployabilityPlan, bool>> clause)
        {
            var eps = new List<EmployabilityPlan>();

            if (IsPOPClaim)
            {
                var epActivityBridges = new List<EmployabilityPlanActivityBridge>
                                        {
                                            new EmployabilityPlanActivityBridge
                                            {
                                                Activity = new Activity
                                                           {
                                                               StartDate    = new DateTime(2020, 08, 20),
                                                               EndDate      = new DateTime(2020, 09, 20),
                                                               ActivityType = new ActivityType
                                                                              {
                                                                                  EnrolledProgramEPActivityTypeBridges = new List<EnrolledProgramEPActivityTypeBridge>
                                                                                                                         {
                                                                                                                             new EnrolledProgramEPActivityTypeBridge
                                                                                                                             {
                                                                                                                                 IsUpfrontActivity = true,
                                                                                                                                 EnrolledProgramId = 11
                                                                                                                             }
                                                                                                                         }
                                                                              }
                                                           }
                                            }
                                        };
                var ep = new EmployabilityPlan
                         {
                             ParticipantId                   = 1,
                             IsDeleted                       = false,
                             EmploybilityPlanActivityBridges = epActivityBridges,
                             SubmitDate                      = new DateTime(2020, 08, 10),
                         };

                eps.Add(ep);
            }

            return eps;
        }

        public new EmployabilityPlan Get(Expression<Func<EmployabilityPlan, bool>> clause)
        {
            GetHasBeenCalled = true;
            return EmployabilityPlan;
        }

        #endregion
    }
}
