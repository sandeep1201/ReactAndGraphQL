using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.Model.Interface.Constants;
using Dcf.Wwp.UnitTest.Infrastructure;
using ContractArea = Dcf.Wwp.DataAccess.Models.ContractArea;
using EnrolledProgram = Dcf.Wwp.DataAccess.Models.EnrolledProgram;

namespace Dcf.Wwp.UnitTest.Repositories
{
    public class MockParticipantRepository : MockRepositoryBase<Participant>, IParticipantRepository
    {
        #region Properties

        public       bool        GetHasBeenCalled;
        public const int         ParticipantEnrolledProgramId = 1;
        public const int         ParticipantEnrolledOfficeId  = 101;
        public       Participant Participant;
        public       bool        AddCutOverEntry = false;
        public       int         GetCount;

        #endregion

        #region Methods

        public new Participant Get(Expression<Func<Participant, bool>> clause)
        {
            Participant = new Participant
                          {
                              Id                          = 1,
                              PinNumber                   = 1234567890,
                              ParticipantEnrolledPrograms = new List<ParticipantEnrolledProgram>
                                                            {
                                                                new ParticipantEnrolledProgram
                                                                {
                                                                    EnrolledProgramId           = ParticipantEnrolledProgramId,
                                                                    OfficeId                    = ParticipantEnrolledOfficeId,
                                                                    EnrolledProgram             = new EnrolledProgram { ProgramCode = Model.Interface.Constants.EnrolledProgram.W2ProgramCode.Trim() },
                                                                    EnrolledProgramStatusCodeId = Model.Interface.Constants.EnrolledProgramStatusCode.EnrolledId,
                                                                    Office                      = new Office
                                                                                                  {
                                                                                                      ContractArea = new ContractArea
                                                                                                                     {
                                                                                                                         Organization = new Organization
                                                                                                                                        {
                                                                                                                                            EntsecAgencyCode =  AgencyCode.FSC
                                                                                                                                        }
                                                                                                                     }
                                                                                                  },
                                                                    Worker = new Worker
                                                                             {
                                                                                 Id = 0
                                                                             }
                                                                },
                                                                new ParticipantEnrolledProgram
                                                                {
                                                                    EnrolledProgramId           = ParticipantEnrolledProgramId,
                                                                    OfficeId                    = ParticipantEnrolledOfficeId,
                                                                    EnrollmentDate              = DateTime.Today.AddDays(-100),
                                                                    EnrolledProgram             = new EnrolledProgram { ProgramCode = Model.Interface.Constants.EnrolledProgram.TjProgramCode.Trim() },
                                                                    EnrolledProgramStatusCodeId = Model.Interface.Constants.EnrolledProgramStatusCode.EnrolledId,
                                                                    Office                      = new Office
                                                                                                  {
                                                                                                      ContractArea = new ContractArea
                                                                                                                     {
                                                                                                                         Organization = new Organization
                                                                                                                                        {
                                                                                                                                            EntsecAgencyCode = AgencyCode.FSC
                                                                                                                                        }
                                                                                                                     }
                                                                                                  },
                                                                    Worker = new Worker
                                                                             {
                                                                                 Id = 0
                                                                             }
                                                                },
                                                                new ParticipantEnrolledProgram
                                                                {
                                                                    EnrolledProgramId           = ParticipantEnrolledProgramId,
                                                                    OfficeId                    = ParticipantEnrolledOfficeId,
                                                                    EnrollmentDate              = DateTime.Today.AddDays(-10),
                                                                    EnrolledProgram             = new EnrolledProgram { ProgramCode = Model.Interface.Constants.EnrolledProgram.TmjProgramCode.Trim() },
                                                                    EnrolledProgramStatusCodeId = Model.Interface.Constants.EnrolledProgramStatusCode.EnrolledId,
                                                                    Office                      = new Office
                                                                                                  {
                                                                                                      ContractArea = new ContractArea
                                                                                                                     {
                                                                                                                         Organization = new Organization
                                                                                                                                        {
                                                                                                                                            EntsecAgencyCode =  AgencyCode.FSC
                                                                                                                                        }
                                                                                                                     }
                                                                                                  },
                                                                    Worker = new Worker
                                                                             {
                                                                                 Id = 0
                                                                             }
                                                                }
                                                            },
                              TimeLimits = new List<TimeLimit>
                                           {
                                               new TimeLimit
                                               {
                                                   ParticipantId  = 1,
                                                   EffectiveMonth = new DateTime(2020, 11, 20),
                                                   StateTimelimit = false,
                                               }
                                           },
                              ParticipantEnrolledProgramCutOverBridges = AddCutOverEntry
                                                                             ? new List<ParticipantEnrolledProgramCutOverBridge>
                                                                               {
                                                                                   new ParticipantEnrolledProgramCutOverBridge
                                                                                   {
                                                                                       CutOverDate       =  new DateTime(2020, 01, 20),
                                                                                       EnrolledProgramId = 11,
                                                                                       EnrolledProgram   = new EnrolledProgram
                                                                                                           {
                                                                                                               ProgramCode = Model.Interface.Constants.EnrolledProgram.W2ProgramCode.Trim()
                                                                                                           },
                                                                                       IsDeleted     = false,
                                                                                       ModifiedBy    = "Mock",
                                                                                       ModifiedDate  = DateTime.Now,
                                                                                       ParticipantId = 1
                                                                                   }
                                                                               }
                                                                             : null,
                              Is60DaysVerified = false,
                              ModifiedBy       = "Mock"
                          };

            GetHasBeenCalled = true;
            GetCount++;
            return new List<Participant> { Participant }.FirstOrDefault(clause.Compile());
        }

        public new IList<TC> ExecStoredProc<TC>(string storedProcName, Dictionary<string, object> parms, string schemaName = "wwp")
        {
            var result = new List<TC>();
            return result;
        }

        #endregion
    }
}
