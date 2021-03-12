using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Constants;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Api.Library.Utils;

namespace Dcf.Wwp.Api.Library.ViewModels.TestScoresApp
{
    public class TestScoreViewModel : BaseInformalAssessmentViewModel
    {
        private readonly ITransactionDomain _transactionDomain;

        public TestScoreViewModel(IRepository repo, IAuthUser authUser, ITransactionDomain transactionDomain) : base(repo, authUser)
        {
            _transactionDomain = transactionDomain;
        }

        public List<ExamScoreContract> GetEducationExams(string pin)
        {
            var examScores = GetExamScoreContractList(Participant, null);

            return examScores;
        }

        public ExamScoreContract GetExamById(string pin, int id)
        {
            var examScoreContract = new ExamScoreContract();

            var p = Repo.GetParticipant(pin);
            if (p == null)
                throw new InvalidOperationException("Pin not valid.");

            var educationExamModel = (from x in p.EducationExams
                                      where x.Id == id
                                      select x).FirstOrDefault();

            if (educationExamModel == null)
                throw new InvalidOperationException("Exam not Found.");

            examScoreContract.Id         = educationExamModel.Id;
            examScoreContract.DateTaken  = educationExamModel.DateTaken.ToStringMonthDayYear();
            examScoreContract.Details    = educationExamModel.Details;
            examScoreContract.ExamTypeId = educationExamModel.ExamTypeId;
            examScoreContract.ExamName   = educationExamModel.ExamType?.Name;
            examScoreContract.RowVersion = educationExamModel.RowVersion;

            var subjects = educationExamModel.ExamResults.Select(er => new SubjectContract
                                                                       {
                                                                           SubjectTypeId         = er.ExamSubjectType.Id,
                                                                           Name                  = er.ExamSubjectType?.Name,
                                                                           DatePassed            = er.DatePassed.ToStringMonthDayYear(),
                                                                           NrsTypeId             = er.NRSTypeId,
                                                                           NrsTypeRating         = er.NRSType?.Rating,
                                                                           SplTypeId             = er.SPLTypeId,
                                                                           SplTypeRating         = er.SPLType?.Rating,
                                                                           Version               = er.Version,
                                                                           Score                 = er.Score,
                                                                           TotalScore            = er.MaxScoreRange,
                                                                           ExamPassTypeId        = er.ExamPassTypeId,
                                                                           Level                 = er.Level,
                                                                           Form                  = er.Form,
                                                                           CasasGradeEquivalency = er.CasasGradeEquivalency,
                                                                           GradeEquivalency      = er.GradeEquivalency,
                                                                           Id                    = er.Id
                                                                       })
                                             .ToList();

            examScoreContract.ExamResults = subjects;

            return examScoreContract;
        }

        public List<ExamScoreContract> GetExamsByType(string pin, string examName)
        {
            switch (examName)
            {
                case "ged":
                    examName = @"GED/HSED";
                    break;
                default:
                    throw new InvalidOperationException("Section is not recognized");
            }

            var examScoreContract = GetExamScoreContractList(Participant, examName);

            return examScoreContract;
        }

        public bool PostEducationExam(ExamScoreContract contract, string user)
        {
            var participantModel = Participant;
            var modifiedDate     = DateTime.Now;

            if (participantModel == null)
                throw new InvalidOperationException("Pin not valid.");

            if (contract == null)
                throw new InvalidOperationException("Exam score data is missing.");

            if (contract.DateTaken.IsNullOrEmpty() || contract.ExamTypeId < 1)
                throw new InvalidOperationException("Test score date is missing.");

            var model = participantModel.EducationExams.SingleOrDefault(x => x.Id == contract.Id) ?? Repo.NewEducationExam(participantModel, user);

            Repo.StartChangeTracking(model);

            model.DateTaken  = contract.DateTaken.ToDateTimeMonthDayYear();
            model.ExamTypeId = contract.ExamTypeId;
            model.Details    = contract.Details;

            if (contract.ExamResults != null)
            {
                foreach (var erContract in contract.ExamResults)
                {
                    var examResultModel = model.ExamResults.FirstOrDefault(x => x.Id == erContract.Id && x.Id > 0) ?? Repo.NewExamResult(model, user);
                    examResultModel.Score     = erContract.Score;
                    examResultModel.NRSTypeId = erContract.NrsTypeId;
                    examResultModel.SPLTypeId = erContract.SplTypeId;

                    if (erContract.ExamPassTypeId == ExamPassType.PassId)
                    {
                        examResultModel.DatePassed = contract.DateTaken.ToDateTimeMonthDayYear();
                    }

                    examResultModel.Version               = erContract.Version?.Trim() == "" ? null : erContract.Version;
                    examResultModel.GradeEquivalency      = erContract.GradeEquivalency;
                    examResultModel.Level                 = erContract.Level?.Trim()                 == "" ? null : erContract.Level;
                    examResultModel.Form                  = erContract.Form?.Trim()                  == "" ? null : erContract.Form;
                    examResultModel.CasasGradeEquivalency = erContract.CasasGradeEquivalency?.Trim() == "" ? null : erContract.CasasGradeEquivalency;
                    examResultModel.IsDeleted             = false;
                    examResultModel.MaxScoreRange         = erContract.TotalScore;
                    examResultModel.ExamSubjectTypeId     = erContract.SubjectTypeId;
                    examResultModel.ExamPassTypeId        = erContract.ExamPassTypeId;
                    examResultModel.ModifiedBy            = user;
                    examResultModel.ModifiedDate          = modifiedDate;
                    model.ExamResults.Add(examResultModel);
                }
            }

            // Do a concurrency check.
            if (!Repo.IsRowVersionStillCurrent(model, contract.RowVersion))
            {
                //UpsertError = "Version has changed.";
                return false;
            }

            if (Repo.HasChanged(model))
            {
                var transactionContract = new TransactionContract
                                          {
                                              ParticipantId       = Participant.Id,
                                              WorkerId            = Repo.WorkerByWIUID(AuthUser.WIUID).Id,
                                              OfficeId            = ParticipantHelper.GetMostRecentEnrolledProgram(Participant, AuthUser).Office.Id,
                                              EffectiveDate       = modifiedDate,
                                              CreatedDate         = modifiedDate,
                                              TransactionTypeCode = TransactionTypes.TestScores,
                                              ModifiedBy          = AuthUser.WIUID
                                          };

                var transaction = _transactionDomain.InsertTransaction(transactionContract, true);

                if (transaction != null)
                    Repo.NewTransaction(transaction as ITransaction);

                model.ModifiedBy   = user;
                model.ModifiedDate = modifiedDate;

                Repo.Save();
            }

            return true;
        }

        public static bool DeleteData(string pin, string id, string user, IRepository repo)
        {
            var p            = repo.GetParticipant(pin);
            var modifiedDate = DateTime.Now;

            int idInt;
            if (int.TryParse(id, out idInt))
            {
                var deletedExam = p.EducationExams.SingleOrDefault(x => x.Id == idInt);
                if (deletedExam == null)
                    return false;

                if (deletedExam.ExamResults != null)
                {
                    // Mark Children objects as deleted as well.
                    foreach (var er in deletedExam.ExamResults)
                    {
                        er.IsDeleted    = true;
                        er.ModifiedBy   = user;
                        er.ModifiedDate = modifiedDate;
                    }
                }

                deletedExam.IsDeleted    = true;
                deletedExam.ModifiedBy   = user;
                deletedExam.ModifiedDate = modifiedDate;

                repo.Save();
                return true;
            }

            return false;
        }

        private List<ExamScoreContract> GetExamScoreContractList(IParticipant participant, string examName)
        {
            var examScoreContracts = new List<ExamScoreContract>();

            if (Participant == null)
                throw new InvalidOperationException("Pin not valid.");

            IList<IEducationExam> educationExams = string.IsNullOrEmpty(examName) ? participant.EducationExams?.OrderBy(x => x.DateTaken).ToList() : participant.EducationExams.Where(x => x.ExamType != null && x.ExamType.Name == examName).OrderBy(x => x.DateTaken).ToList();

            if (educationExams == null || !educationExams.Any()) return null;

            foreach (var ee in educationExams)
            {
                var esc = new ExamScoreContract
                          {
                              Id             = ee.Id,
                              DateTaken      = ee.DateTaken.ToStringMonthDayYear(),
                              ExamTypeId     = ee.ExamTypeId,
                              ExamName       = ee.ExamType?.Name,
                              Details        = ee.Details,
                              RowVersion     = ee.RowVersion,
                              ModifiedBy     = ee.ModifiedBy,
                              ModifiedDate   = ee.ModifiedDate,
                              ModifiedByName = Repo.GetWorkerNameByWamsId(ee.ModifiedBy)
                          };

                var subjects = ee.ExamResults.Select(item => new SubjectContract
                                                             {
                                                                 SubjectTypeId         = item.ExamSubjectTypeId,
                                                                 Name                  = item.ExamSubjectType?.Name,
                                                                 NrsTypeId             = item.NRSTypeId,
                                                                 NrsTypeRating         = item.NRSType?.Rating,
                                                                 SplTypeId             = item.SPLTypeId,
                                                                 SplTypeRating         = item.SPLType?.Rating,
                                                                 Version               = item.Version,
                                                                 Score                 = item.Score,
                                                                 TotalScore            = item.MaxScoreRange,
                                                                 ExamPassTypeId        = item.ExamPassTypeId,
                                                                 Level                 = item.Level,
                                                                 CasasGradeEquivalency = item.CasasGradeEquivalency,
                                                                 Form                  = item.Form,
                                                                 GradeEquivalency      = item.GradeEquivalency,
                                                                 Id                    = item.Id,
                                                                 DatePassed            = GetDatePassedForSubject(item, educationExams)
                                                             }).ToList();

                esc.ExamResults = subjects;

                examScoreContracts.Add(esc);
            }

            return examScoreContracts;
        }

        private string GetDatePassedForSubject(IExamResult exam, IList<IEducationExam> educationExams)
        {
            var datePassed = string.Empty;

            if (educationExams == null)
                return datePassed;

            var ee = exam.EducationExam;

            if (ee == null)
                return datePassed;

            var examResults = educationExams.Where(x => x.ExamTypeId == ee.ExamTypeId).SelectMany(x => x.ExamResults);
            var lastPassed  = examResults.Where(x => x.ExamSubjectTypeId == exam.ExamSubjectTypeId && x.ExamPassType?.Name == "Pass" && x.DatePassed.HasValue).OrderByDescending(x => x.DatePassed).Select(x => x.DatePassed).FirstOrDefault();

            if (lastPassed.HasValue)
                datePassed = lastPassed.ToStringMonthDayYear();

            return datePassed;
        }
    }
}
