using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using log4net;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Api.Library.Model;
using Dcf.Wwp.Api.Library.Utils;
using Dcf.Wwp.ConnectedServices.Cww;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Api.Library.ViewModels
{
    public class ParticipantsViewModel : BaseViewModel
    {
        #region Properties

        private static readonly ILog                    _log = LogManager.GetLogger("Dcf.Wwp.Api.Library.ViewModels.ParticipantsViewModel");
        private readonly        IConfidentialityChecker _confidentiality;
        private readonly        IAuthAccessChecker      _authAccessChecker;
        private                 IAuthUser               _authUser;

        #endregion

        #region Methods

        public ParticipantsViewModel(IRepository repository, IAuthUser authUser, IConfidentialityChecker confidentiality, IReadOnlyCollection<ICountyAndTribe> countyAndTribes, IAuthAccessChecker authAccessChecker) : base(repository, authUser, countyAndTribes)
        {
            _confidentiality   = confidentiality;
            _authAccessChecker = authAccessChecker;
            _countyAndTribes   = countyAndTribes;
            _authUser          = authUser;
        }

        public List<FeatureToggleContract> GetFeatureToggleValues(string fieldName)
        {
            var value = Repo.GetFeatureValue(fieldName)
                            .Select(i => FeatureToggleContract.CreateContract(i.Id, i.ParameterName, i.ParameterValue, i.ModifiedDate?.ToString("MM/dd/yyyy")))
                            .ToList();

            return value;
        }

        public List<ParticipantsContract> GetRecentParticipants(string wamsId, bool useWS)
        {
            // 1.  Get a flattened result set from sproc
            // 2.  Transform into a lookup by participant Id
            // 3a. The first few columns comprise the 
            //     participant unique data...
            // 3b. the remainder contain the program data

            var dataPoint = new PerfDataPoint("GetRecentParticipants", _authUser.Username);
            dataPoint.Start();

            var participants = Repo.GetRecentParticipants(wamsId).ToList();
            var results      =         GetParticipantsResult(participants, useWS, dataPoint);

            dataPoint.Stop();
            _log.Debug(dataPoint);

            return (results);
        }

        public List<ParticipantsContract> GetParticipantsBySearch(string firstName, string lastName, string middleName, string gender, DateTime? dob, bool useWS)
        {
            var dataPoint = new PerfDataPoint("GetParticipantsBySearch", _authUser.Username);
            dataPoint.Start();

            var participants = Repo.GetParticipantsBySearch(firstName, lastName, middleName, gender, dob).ToList();
            var results      = GetParticipantsResult(participants, useWS, dataPoint);

            dataPoint.Stop();
            _log.Debug(dataPoint);

            return (results);
        }

        public List<ParticipantsContract> GetParticipantsForWorker(string wamsId, string agency, string workerProgram, bool useWS)
        {
            if (string.IsNullOrEmpty(workerProgram))
                return new List<ParticipantsContract>();

            var dataPoint = new PerfDataPoint("GetParticipantsForWorker", _authUser.Username);
            dataPoint.Start();

            var participants = Repo.GetParticipantsForWorker(wamsId, agency, workerProgram).ToList();

            var lookup = participants.ToLookup(p => p.ParticipantId);

            var results = lookup.Select(group =>
                                        {
                                            var program = group.FirstOrDefault();

                                            if (program == null)
                                            {
                                                return null;
                                            }

                                            var pinNo = (decimal) program.PinNumber;
                                            var dt    = Repo.GetMostRecentPrograms(pinNo);

                                            var rs = dt.AsEnumerable()
                                                       .Select(r => new MostRecentProgram
                                                                    {
                                                                        ProgramName      = r.Field<string>("ProgramName"),
                                                                        RecentStatus     = r.Field<string>("RecentStatus"),
                                                                        RecentStatusDate = r.Field<DateTime?>("RecentStatusDate"),
                                                                        AssignedWorker   = r.Field<string>("AssignedWorker"),
                                                                        WIUID            = r.Field<string>("WIUID")
                                                                    })
                                                       .ToList();

                                            var prog     = rs.FirstOrDefault(i => i.ProgramName.Trim() == "WW" || i.ProgramName.Trim() == "LF");
                                            var workerId = prog?.AssignedWorker;


                                            var cwwReply = new GetKeySecurityInfoResponse();
                                            var wiuid    = string.Empty;

                                            //Reading the flag from config file to check if we need to call the web service or not
                                            if (useWS)
                                            {
                                                cwwReply = _confidentiality.Check(pinNo, null, workerId);
                                                wiuid    = Repo.WorkerByMainframeId(cwwReply.FEPSupervisor)?.WIUID;

                                                // perf code
                                                dataPoint.Cached  = _confidentiality.Cached;
                                                dataPoint.Web     = _confidentiality.Web;
                                                dataPoint.Retries = _confidentiality.Retries;
                                                dataPoint.Total   = _confidentiality.Total;
                                                // end perf code
                                            }
                                            else
                                            {
                                                cwwReply.CaseCofidentailStatus = "N";
                                            }

                                            var hasAccess = _authAccessChecker.HasAccess(wiuid, rs);

                                            var c = ParticipantsContract.Create(program.ParticipantId ?? -1,
                                                                                program.ParticipantFirstName,
                                                                                program.ParticipantMiddleName,
                                                                                program.ParticipantLastName,
                                                                                program.ParticipantSuffixName,
                                                                                program.PinNumber,
                                                                                program.ParticipantDateOfBirth,
                                                                                cwwReply?.CaseCofidentailStatus?.ToUpper() == "Y",
                                                                                hasAccess,
                                                                                null,
                                                                                null,
                                                                                null,
                                                                                group.Select(usp => new
                                                                                                    {
                                                                                                        // an anonymous type is so we don't have to modify
                                                                                                        // any existing angular code for empty programs
                                                                                                        hasProgram = !string.IsNullOrEmpty(usp.ProgramCode),
                                                                                                        p = new EnrolledProgramContract
                                                                                                            {
                                                                                                                Id                = usp.PEPId,
                                                                                                                EnrolledProgramId = usp.EnrolledProgramId,
                                                                                                                ParticipantId     = program.ParticipantId ?? -1,
                                                                                                                ProgramCode       = usp.ProgramName,
                                                                                                                ProgramCd         = usp.ProgramCode, // called ProgramCd on Angular side
                                                                                                                Status            = usp.RecentStatus,
                                                                                                                IsTransfer        = usp.IsTransfer,
                                                                                                                StatusDate        = usp.RecentStatusDate,
                                                                                                                OfficeCounty      = usp.CountyName,
                                                                                                                AgencyCode        = usp.EntsecAgencyCode,
                                                                                                                AgencyName        = usp.WorkerAgencyName,
                                                                                                                OfficeNumber      = usp.OfficeNumber,
                                                                                                                AssignedWorker = new WorkerContract
                                                                                                                                 {
                                                                                                                                     IsActive      = usp.WorkerActiveStatusCode,
                                                                                                                                     FirstName     = usp.WorkerFirstName,
                                                                                                                                     MiddleInitial = usp.WorkerMiddleInitial,
                                                                                                                                     LastName      = usp.WorkerLastName,
                                                                                                                                     WorkerId      = usp.MFUserId
                                                                                                                                 }
                                                                                                            }
                                                                                                    }).Where(x => x.hasProgram)
                                                                                     .Select(x => x.p)
                                                                                     .ToList(),
                                                                                program.GroupOrder,
                                                                                program.Rn);

                                            return (c);
                                        }).ToList();

            dataPoint.Stop();
            _log.Debug(dataPoint);

            return (results);
        }

        public List<ParticipantsContract> GetReferralsAndTransfersResults(string userId, bool refresh, bool useWS)
        {
            var authorizations = string.Join(",", AuthUser.Authorizations);
#if DEBUG
            var sw = new Stopwatch();
            sw.Start();
#endif
            var dataPoint = new PerfDataPoint("GetReferralsAndTransfers", _authUser.Username);
            dataPoint.Start();

            var worker      = Repo.WorkerByWamsId(userId);
            var refandTrans = Repo.GetReferralsAndTransfersResults(worker, refresh, AuthUser.AgencyCode, authorizations).ToList();

            Func<IUSP_ReferralsAndTransfers_Result, ParticipantsContract> packageUp = p =>
                                                                                      {
                                                                                          var pinNo = (decimal) p.PinNumber;
                                                                                          var dt    = Repo.GetMostRecentPrograms(pinNo);

                                                                                          var rs = dt.AsEnumerable()
                                                                                                     .Select(r => new MostRecentProgram
                                                                                                                  {
                                                                                                                      ProgramName      = r.Field<string>("ProgramName"),
                                                                                                                      RecentStatus     = r.Field<string>("RecentStatus"),
                                                                                                                      RecentStatusDate = r.Field<DateTime?>("RecentStatusDate"),
                                                                                                                      AssignedWorker   = r.Field<string>("AssignedWorker"),
                                                                                                                      WIUID            = r.Field<string>("WIUID")
                                                                                                                  })
                                                                                                     .ToList();

                                                                                          var program  = rs.FirstOrDefault(i => i.ProgramName.Trim() == "WW" || i.ProgramName.Trim() == "LF");
                                                                                          var workerId = program?.AssignedWorker;

                                                                                          var cwwReply = new GetKeySecurityInfoResponse();
                                                                                          var wiuid    = string.Empty;

                                                                                          //Reading the flag from config file to check if we need to call the web service or not
                                                                                          if (useWS)
                                                                                          {
                                                                                              cwwReply = _confidentiality.Check(pinNo, null, workerId);
                                                                                              wiuid    = Repo.WorkerByMainframeId(cwwReply.FEPSupervisor)?.WIUID;

                                                                                              // perf code
                                                                                              dataPoint.Cached  = _confidentiality.Cached;
                                                                                              dataPoint.Web     = _confidentiality.Web;
                                                                                              dataPoint.Retries = _confidentiality.Retries;
                                                                                              dataPoint.Total   = _confidentiality.Total;
                                                                                              // end perf code
                                                                                          }
                                                                                          else
                                                                                          {
                                                                                              cwwReply.CaseCofidentailStatus = "N";
                                                                                          }

                                                                                          var hasAccess = _authAccessChecker.HasAccess(wiuid, rs);

                                                                                          var contract = ParticipantsContract.Create(-1,
                                                                                                                                     p.ParticipantFirstname,
                                                                                                                                     p.ParticipantMiddleInitial,
                                                                                                                                     p.ParticipantLastName,
                                                                                                                                     p.ParticipantSuffix,
                                                                                                                                     p.PinNumber,
                                                                                                                                     p.ParticipantDOB,
                                                                                                                                     cwwReply?.CaseCofidentailStatus?.ToUpper() == "Y", //p.IsConfidential, // UNTIL WE FIGURE OUT THE PAGING...
                                                                                                                                     hasAccess,
                                                                                                                                     null,
                                                                                                                                     null,
                                                                                                                                     null,
                                                                                                                                     new List<EnrolledProgramContract>
                                                                                                                                     {
                                                                                                                                         new EnrolledProgramContract
                                                                                                                                         {
                                                                                                                                             Id          = 0,
                                                                                                                                             StatusDate  = p.ReferralDate,
                                                                                                                                             Status      = p.Status,
                                                                                                                                             IsTransfer  = p.IsTransfer,
                                                                                                                                             ProgramCd   = Repo.GetEnrolledProgramCd(p.EnrolledProgram),
                                                                                                                                             ProgramCode = p.EnrolledProgram,
                                                                                                                                             AssignedWorker = new WorkerContract
                                                                                                                                                              {
                                                                                                                                                                  FirstName     = p.WorkerFirstName,
                                                                                                                                                                  MiddleInitial = p.WorkerMiddleInitial,
                                                                                                                                                                  LastName      = p.WorkerLastName,
                                                                                                                                                                  WorkerId      = p.MFUserId
                                                                                                                                                              },
                                                                                                                                             AgencyCode   = " ",
                                                                                                                                             AgencyName   = p.AgencyName,
                                                                                                                                             OfficeCounty = p.CountyName,
                                                                                                                                             OfficeNumber = p.OfficeNumber
                                                                                                                                         }
                                                                                                                                     },
                                                                                                                                     //officeCountyName: null, 
                                                                                                                                     groupOrder: p.GroupOrder,
                                                                                                                                     sortOrder: p.Rn
                                                                                                                                    );

                                                                                          return (contract);
                                                                                      };

            var results = refandTrans.Select(p => packageUp(p)).ToList();
#if DEBUG
            sw.Stop();
#endif
            dataPoint.Stop();
            _log.Debug(dataPoint);

            return (results);
        }

        private List<ParticipantsContract> GetParticipantsResult(IList<IUSP_RecentlyAccessed_ProgramStatus_Result> participants, bool useWS, PerfDataPoint dataPoint)
        {
            var lookup = participants.ToLookup(p => p.ParticipantId);

            var results = lookup.Select(group =>
                                        {
                                            var p = group.FirstOrDefault();

                                            if (p == null)
                                            {
                                                return null;
                                            }

                                            var pinNo = (decimal) p.PinNumber;
                                            var dt    = Repo.GetMostRecentPrograms(pinNo);

                                            var rs = dt.AsEnumerable()
                                                       .Select(r => new MostRecentProgram
                                                                    {
                                                                        ProgramName      = r.Field<string>("ProgramName"),
                                                                        RecentStatus     = r.Field<string>("RecentStatus"),
                                                                        RecentStatusDate = r.Field<DateTime?>("RecentStatusDate"),
                                                                        AssignedWorker   = r.Field<string>("AssignedWorker"),
                                                                        WIUID            = r.Field<string>("WIUID")
                                                                    })
                                                       .ToList();

                                            var program  = rs.FirstOrDefault(i => i.ProgramName.Trim() == "WW" || i.ProgramName.Trim() == "LF");
                                            var workerId = program?.AssignedWorker;
                                            var cwwReply = new GetKeySecurityInfoResponse();
                                            var wiuid    = string.Empty;

                                            //Reading the flag from config file to check if we need to call the web service or not
                                            if (useWS)
                                            {
                                                cwwReply = _confidentiality.Check(pinNo, null, workerId);
                                                wiuid    = Repo.WorkerByMainframeId(cwwReply.FEPSupervisor)?.WIUID;

                                                // perf code
                                                dataPoint.Cached  = _confidentiality.Cached;
                                                dataPoint.Web     = _confidentiality.Web;
                                                dataPoint.Retries = _confidentiality.Retries;
                                                dataPoint.Total   = _confidentiality.Total;
                                                // end perf code
                                            }
                                            else
                                            {
                                                cwwReply.CaseCofidentailStatus = "N";
                                            }

                                            var hasAccess = _authAccessChecker.HasAccess(wiuid, rs);

                                            var contract = ParticipantsContract.Create(p.ParticipantId ?? -1,
                                                                                       p.ParticipantFirstName,
                                                                                       p.ParticipantMiddleName,
                                                                                       p.ParticipantLastName,
                                                                                       p.ParticipantSuffixName,
                                                                                       p.PinNumber,
                                                                                       p.ParticipantDateOfBirth,
                                                                                       cwwReply.CaseCofidentailStatus?.ToUpper() == "Y", //p.IsConfidential,
                                                                                       hasAccess,
                                                                                       null,
                                                                                       null,
                                                                                       p.GenderIndicator,
                                                                                       group.Select(usp => new
                                                                                                           {
                                                                                                               // anonymous type is so we don't have to modify
                                                                                                               // any existing angular code on empty programs
                                                                                                               hasProgram = !string.IsNullOrEmpty(usp.ProgramCode),
                                                                                                               p = new EnrolledProgramContract
                                                                                                                   {
                                                                                                                       Id                = usp.PEPId,
                                                                                                                       EnrolledProgramId = usp.EnrolledProgramId,
                                                                                                                       ParticipantId     = p.ParticipantId ?? -1,
                                                                                                                       ProgramCode       = usp.ProgramName,
                                                                                                                       ProgramCd         = usp.ProgramCode, // called ProgramCd on Angular side
                                                                                                                       Status            = usp.RecentStatus,
                                                                                                                       IsTransfer        = usp.IsTransfer,
                                                                                                                       StatusDate        = usp.RecentStatusDate,
                                                                                                                       OfficeCounty      = usp.CountyName,
                                                                                                                       AgencyCode        = usp.EntsecAgencyCode,
                                                                                                                       AgencyName        = usp.WorkerAgencyName,
                                                                                                                       OfficeNumber      = usp.OfficeNumber,
                                                                                                                       AssignedWorker = new WorkerContract
                                                                                                                                        {
                                                                                                                                            IsActive      = usp.WorkerActiveStatusCode,
                                                                                                                                            FirstName     = usp.WorkerFirstName,
                                                                                                                                            MiddleInitial = usp.WorkerMiddleInitial,
                                                                                                                                            LastName      = usp.WorkerLastName,
                                                                                                                                            WorkerId      = usp.MFUserId
                                                                                                                                        }
                                                                                                                   }
                                                                                                           }).Where(x => x.hasProgram)
                                                                                            .Select(x => x.p)
                                                                                            .ToList()
                                                                                      );

                                            return (contract);
                                        }
                                       ).ToList();

            return results;
        }

        #endregion
    }
}
