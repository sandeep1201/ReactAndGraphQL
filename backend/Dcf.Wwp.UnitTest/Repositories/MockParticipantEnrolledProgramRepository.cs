using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.UnitTest.Infrastructure;
using EnrolledProgramStatusCode = Dcf.Wwp.Model.Interface.Constants.EnrolledProgramStatusCode;

namespace Dcf.Wwp.UnitTest.Repositories
{
    public class MockParticipantEnrolledProgramRepository : MockRepositoryBase<ParticipantEnrolledProgram>, IParticipantEnrolledProgramRepository
    {
        #region Properties

        public new bool IsPOPClaim      = false;
        public     bool AddCutOverEntry = false;

        #endregion

        #region Methods

        public new IEnumerable<ParticipantEnrolledProgram> GetMany(Expression<Func<ParticipantEnrolledProgram, bool>> clause)
        {
            var peps = new List<ParticipantEnrolledProgram>();

            if (IsPOPClaim && AddCutOverEntry)
            {
                var pep = new ParticipantEnrolledProgram
                          {
                              ParticipantId  = 1,
                              EnrollmentDate = new DateTime(2020, 08, 20),
                              EnrolledProgram = new EnrolledProgram
                                                {
                                                    ProgramCode = Model.Interface.Constants.EnrolledProgram.W2ProgramCode.Trim()
                                                },
                              Office = new Office
                                       {
                                           ContractArea = new ContractArea
                                                          {
                                                              OrganizationId = 1
                                                          }
                                       },
                              Participant = new Participant
                                            {
                                                Id = 1,
                                                TimeLimits = new List<TimeLimit>
                                                             {
                                                                 new TimeLimit
                                                                 {
                                                                     ParticipantId  = 1,
                                                                     EffectiveMonth = new DateTime(2020, 11, 20),
                                                                     StateTimelimit = false,
                                                                 }
                                                             },
                                                ParticipantEnrolledProgramCutOverBridges = new List<ParticipantEnrolledProgramCutOverBridge>
                                                                                           {
                                                                                               new ParticipantEnrolledProgramCutOverBridge
                                                                                               {
                                                                                                   CutOverDate       =  new DateTime(2020, 01, 20),
                                                                                                   EnrolledProgramId = 11,
                                                                                                   EnrolledProgram = new EnrolledProgram
                                                                                                                     {
                                                                                                                         ProgramCode = Model.Interface.Constants.EnrolledProgram.W2ProgramCode.Trim()
                                                                                                                     },
                                                                                                   IsDeleted     = false,
                                                                                                   ModifiedBy    = "Mock",
                                                                                                   ModifiedDate  = DateTime.Now,
                                                                                                   ParticipantId = 1
                                                                                               }
                                                                                           }
                                            }
                          };
                peps.Add(pep);
            }
            else
                if (IsPOPClaim && !AddCutOverEntry)
                {
                    var pep = new ParticipantEnrolledProgram
                              {
                                  ParticipantId  = 1,
                                  EnrollmentDate = new DateTime(2020, 08, 20),
                                  EnrolledProgram = new EnrolledProgram
                                                    {
                                                        ProgramCode = Model.Interface.Constants.EnrolledProgram.W2ProgramCode.Trim()
                                                    },
                                  Office = new Office
                                           {
                                               ContractArea = new ContractArea
                                                              {
                                                                  OrganizationId = 1
                                                              }
                                           },
                                  Participant = new Participant
                                                {
                                                    Id = 1,
                                                    TimeLimits = new List<TimeLimit>
                                                                 {
                                                                     new TimeLimit
                                                                     {
                                                                         ParticipantId  = 1,
                                                                         EffectiveMonth = new DateTime(2020, 11, 20),
                                                                         StateTimelimit = false,
                                                                     }
                                                                 },
                                                }
                              };
                    peps.Add(pep);
                }


            return peps;
        }


        public new ParticipantEnrolledProgram Get(Expression<Func<ParticipantEnrolledProgram, bool>> clause)
        {
            var participantEnrolledProgram = new ParticipantEnrolledProgram
                                             {
                                                 ParticipantId  = 1,
                                                 EnrollmentDate = new DateTime(2020, 08, 20),
                                                 EnrolledProgram = new EnrolledProgram
                                                                   {
                                                                       ProgramCode = Model.Interface.Constants.EnrolledProgram.W2ProgramCode.Trim()
                                                                   },
                                                 Office = new Office
                                                          {
                                                              ContractArea = new ContractArea
                                                                             {
                                                                                 OrganizationId = 1
                                                                             }
                                                          },
                                                 Participant = new Participant
                                                               {
                                                                   Id = 1,
                                                                   TimeLimits = new List<TimeLimit>
                                                                                {
                                                                                    new TimeLimit
                                                                                    {
                                                                                        ParticipantId  = 1,
                                                                                        EffectiveMonth = new DateTime(2020, 11, 20),
                                                                                        StateTimelimit = false,
                                                                                    }
                                                                                }
                                                               }
                                             };
            if (AddCutOverEntry)
            {
                participantEnrolledProgram.Participant.ParticipantEnrolledProgramCutOverBridges = new List<ParticipantEnrolledProgramCutOverBridge>
                                                                                                  {
                                                                                                      new ParticipantEnrolledProgramCutOverBridge
                                                                                                      {
                                                                                                          CutOverDate       =  new DateTime(2020, 01, 20),
                                                                                                          EnrolledProgramId = 11,
                                                                                                          EnrolledProgram = new EnrolledProgram
                                                                                                                            {
                                                                                                                                ProgramCode = Model.Interface.Constants.EnrolledProgram.W2ProgramCode.Trim()
                                                                                                                            },
                                                                                                          IsDeleted     = false,
                                                                                                          ModifiedBy    = "Mock",
                                                                                                          ModifiedDate  = DateTime.Now,
                                                                                                          ParticipantId = 1
                                                                                                      }
                                                                                                  };
            }

            var participantEnrolledProgramTj = new ParticipantEnrolledProgram
                                               {
                                                   ParticipantId               = 11196,
                                                   EnrollmentDate              = DateTime.Now.AddDays(-100),
                                                   EnrolledProgramStatusCodeId = EnrolledProgramStatusCode.EnrolledId,
                                                   EnrolledProgram = new EnrolledProgram
                                                                     {
                                                                         ProgramCode = Model.Interface.Constants.EnrolledProgram.TjProgramCode.Trim()
                                                                     }
                                               };
            var participantEnrolledProgramTmj = new ParticipantEnrolledProgram
                                                {
                                                    ParticipantId               = 11196,
                                                    EnrollmentDate              = DateTime.Today.AddDays(-10),
                                                    EnrolledProgramStatusCodeId = EnrolledProgramStatusCode.EnrolledId,
                                                    EnrolledProgram = new EnrolledProgram
                                                                      {
                                                                          ProgramCode = Model.Interface.Constants.EnrolledProgram.TmjProgramCode.Trim()
                                                                      }
                                                };
            var enrolledProgramList = new List<ParticipantEnrolledProgram> { participantEnrolledProgram, participantEnrolledProgramTj, participantEnrolledProgramTmj };
            return enrolledProgramList.FirstOrDefault(clause.Compile());
        }

        #endregion
    }
}
