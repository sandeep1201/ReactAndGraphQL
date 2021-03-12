using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface.Repository;
using DCF.Common.Logging;
using Dcf.Wwp.Model.Interface.Constants;
using DCF.Common.Extensions;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Data.Sql.Model;
using WorkerTaskStatus = Dcf.Wwp.Model.Interface.Constants.WorkerTaskStatus;

namespace Dcf.Wwp.Api.Library.ViewModels
{
    public class ReassignFepViewModel
    {
        #region Properties

        private IRepository Repo   { get; }
        private ILog        Logger { get; }

        #endregion


        #region Methods

        public ReassignFepViewModel(IRepository repository)
        {
            Repo   = repository;
            Logger = LogProvider.GetLogger(GetType());
        }

        public void UpdatePins(string newMainframeId, IEnumerable<string> pins, bool canUpdateWorkerTask = true)
        {
            var modifiedBy   = "CWW";
            var modifiedDate = DateTime.Now;
            foreach (var pin in pins)
            {
                // Just in case we get PINs with leading zeros, we will strip them off.
                var participant = Repo.GetParticipant(pin.TrimStart('0'));
                var worker      = Repo.WorkerByMainframeId(newMainframeId);

                // If the new FEP doesn’t exist in WWP, or is not in the FEP or FEP Supervisor 
                // role or is not in the agency the participant is in, return an error to CWW.
                // CWW will not roll back reassignment. CWW write the failure to a log and email
                // failures to W -2 help desk overnight.

                // TODO: Handle when the worker is not known to our system by checking EntSec.

                if (worker == null || worker.WorkerActiveStatusCode == "INACTIVE")
                {
                    Logger.Warn($"The supplied Mainframe ID '{newMainframeId}' is not known to WWP");
                    throw new InvalidOperationException("The supplied Mainframe ID is not known to WWP.");
                }

                if (!worker.Roles.Contains("FEP"))
                {
                    Logger.Warn($"The supplied Mainframe ID '{newMainframeId}' is not known to WWP in a FEP role.");
                    throw new InvalidOperationException("The supplied Mainframe ID is not known to WWP in a FEP role.");
                }

                if (participant != null)
                {
                    if (!participant.PinNumber.HasValue)
                        throw new InvalidOperationException("There is a problem with the Participants PIN.");

                    var peps = Repo.GetPepRecordsForPin(participant.PinNumber.Value).ToList();

                    foreach (var peploop in peps)
                    {
                        // We only need to handle Eligibility programs.
                        // WWP will update for the most recent instance of a Learnfare or W-2 program (enrolled or disenrolled) for the PIN. 
                        if (peploop == null) continue;

                        var pep = Repo.GetPepById(peploop.Id);
                        if (pep.EnrolledProgram.ProgramType.Trim() == "Eligibility")
                        {
                            if (pep.Office.ContractArea.OrganizationId == worker.OrganizationId && pep.WorkerId != worker.Id && (pep.IsEnrolled || pep.IsDisenrolled))
                            {
                                pep.Worker       = worker;
                                pep.ModifiedBy   = modifiedBy;
                                pep.ModifiedDate = modifiedDate;

                                #region WorkerTasks

                                if (canUpdateWorkerTask && Repo.GetFeatureValue(WorkerTaskStatus.WorkerTaskList).First().ParameterValue.ToDateMonthDayYear().IsSameOrBefore(DateTime.Today))
                                {
                                    pep.Participant
                                       .WorkerTaskLists
                                       .Where(i => i.WorkerTaskStatus.Code == WorkerTaskStatus.Open)
                                       .ForEach(i =>
                                                {
                                                    if (i.IsSystemGenerated == true)
                                                        i.Worker = worker;
                                                    else
                                                        i.WorkerTaskStatusId = Repo.GetWorkerTaskStatus(WorkerTaskStatus.Closed).Id;

                                                    i.ModifiedBy   = modifiedBy;
                                                    i.ModifiedDate = modifiedDate;
                                                });

                                    var category = Repo.GetWorkerTaskCategory(WorkerTaskCategoryCodes.PinReassignCode);
                                    var workerTaskListContract = new WorkerTaskList
                                                                 {
                                                                     TaskDetails        = category.Description,
                                                                     CategoryId         = category.Id,
                                                                     WorkerTaskStatusId = Repo.GetWorkerTaskStatus(WorkerTaskStatus.Open).Id,
                                                                     WorkerId           = pep.Worker.Id,
                                                                     ParticipantId      = pep.ParticipantId,
                                                                     TaskDate           = modifiedDate,
                                                                     StatusDate         = modifiedDate,
                                                                     IsSystemGenerated  = true,
                                                                     ModifiedBy         = modifiedBy,
                                                                     ModifiedDate       = modifiedDate
                                                                 };

                                    Repo.NewWorkerTask(workerTaskListContract);
                                }

                                #endregion
                            }
                        }
                    }

                    Repo.Save();
                }
            }
        }

        #endregion
    }
}
