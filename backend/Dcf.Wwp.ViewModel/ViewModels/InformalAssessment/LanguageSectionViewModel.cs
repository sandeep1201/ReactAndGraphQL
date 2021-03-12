using System;
using System.Linq;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Contracts.InformalAssessment;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Constants;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Api.Library.ViewModels.InformalAssessment
{
    public class LanguageSectionViewModel : BaseInformalAssessmentViewModel
    {
        public LanguageSectionViewModel(IRepository repo, IAuthUser authUser) : base(repo, authUser)
        {
        }

        public static LanguageSectionContract GetContract(IInformalAssessment ia, IParticipant participant)
        {
            var contract = new LanguageSectionContract();

            if (ia != null && participant != null)
            {
                if (participant.LanguageSection != null)
                {
                    var ls = participant.LanguageSection;

                    contract.RowVersion   = ls.RowVersion;
                    contract.ModifiedDate = ls.ModifiedDate;
                    contract.ModifiedBy   = ls.ModifiedBy;

                    if (ls.HomeLanguage != null)
                    {
                        contract.HomeLanguageTypeId        = ls.HomeLanguage.LanguageId;
                        contract.HomeLanguageName          = ls.HomeLanguage.Language?.Name?.SafeTrim();
                        contract.IsAbleToReadHomeLanguage  = ls.HomeLanguage.IsAbleToRead;
                        contract.IsAbleToSpeakHomeLanguage = ls.HomeLanguage.IsAbleToSpeak;
                        contract.IsAbleToWriteHomeLanguage = ls.HomeLanguage.IsAbleToWrite;
                    }

                    if (ls.KnownLanguages.Count > 0)
                    {
                        foreach (var l in ls.KnownLanguages.OrderBy(x => x.SortOrder).ToList())
                        {
                            // Check for Primary(Home) Language
                            if (l.IsPrimary.HasValue && l.IsPrimary.Value && l.IsDeleted == false)
                            {
                                contract.HomeLanguageTypeId        = l.LanguageId;
                                contract.HomeLanguageId            = l.Id;
                                contract.HomeLanguageName          = l.Language?.Name?.SafeTrim();
                                contract.IsAbleToReadHomeLanguage  = l.IsAbleToRead;
                                contract.IsAbleToSpeakHomeLanguage = l.IsAbleToSpeak;
                                contract.IsAbleToWriteHomeLanguage = l.IsAbleToWrite;
                            }
                        }

                        // List of languages with English and Primary Removed
                        var nonHomeLanguages = ls.KnownLanguages.Where(x => (x.IsEnglish == false) && (x.IsPrimary == false)).ToList();

                        foreach (var l in nonHomeLanguages)
                        {
                            if (l.IsDeleted == false)
                            {
                                var kl = new KnownLanguageContract();

                                kl.Id           = l.Id;
                                kl.LanguageId   = l.LanguageId;
                                kl.LanguageName = l.Language?.Name?.SafeTrim();
                                kl.CanRead      = l.IsAbleToRead;
                                kl.CanSpeak     = l.IsAbleToSpeak;
                                kl.CanWrite     = l.IsAbleToWrite;

                                contract.KnownLanguages.Add(kl);
                            }
                        }
                    }

                    contract.IsAbleToReadEnglish  = ls.IsAbleToReadEnglish;
                    contract.IsAbleToWriteEnglish = ls.IsAbleToWriteEnglish;
                    contract.IsAbleToSpeakEnglish = ls.IsAbleToSpeakEnglish;
                    contract.InterpreterDetails   = ls.InterpreterDetails;
                    contract.IsNeedingInterpreter = ls.IsNeedingInterpreter;
                    contract.Notes                = ls.Notes;
                }

                // We look at the assessment section now which at this point just
                // indicates it was submitted via the driver flow.
                if (ia.LanguageAssessmentSection != null)
                {
                    contract.AssessmentRowVersion     = ia.LanguageAssessmentSection.RowVersion;
                    contract.IsSubmittedViaDriverFlow = true;
                }
            }

            return contract;
        }

        public LanguageSectionContract GetData() => GetContract(InformalAssessment, Participant);

        public bool PostData(LanguageSectionContract contract, string user)
        {
            // ID of English in the database.
            const int englishLanguage = Languages.EnglishId;

            var p = Participant;

            if (p == null)
            {
                throw new InvalidOperationException("PIN not valid.");
            }

            if (contract == null)
            {
                throw new InvalidOperationException("Languages data is missing.");
            }

            ILanguageSection           ls  = null;
            ILanguageAssessmentSection las = null;

            var updateTime = DateTime.Now;  // this is for DCF-BI's benefit. (Master record timestamped when detail record is updated)

            var ia = InformalAssessment; // appears to be the same as p.InProgressInformalAssessment

            // If we have an in progress assessment, then we will set the ILanguageAssessmentSection
            // and IInformalAssessment objects.  We will not create new ones as the user may be
            // editing the data outside of an assessment.
            if (p.InProgressInformalAssessment != null)
            {
                las = p.InProgressInformalAssessment.LanguageAssessmentSection ?? Repo.NewLanguageAssessmentSection(p.InProgressInformalAssessment, user);

                Repo.StartChangeTracking(las);

                // To force a new assessment section to be modified so that it is registered as changed,
                // we will update the ReviewCompleted.
                las.ReviewCompleted = true;
                las.ModifiedBy      = AuthUser.Username;
                las.ModifiedDate    = updateTime;
            }

            ls = p.LanguageSection ?? Repo.NewLanguageSection(p, user);
            ls.ModifiedBy   = AuthUser.Username;
            ls.ModifiedDate = updateTime;

            Repo.StartChangeTracking(ls);

            var userRowVersion       = contract.RowVersion;
            var userAssessRowVersion = contract.AssessmentRowVersion;

            ls.Notes = contract.Notes.SafeTrim();

            // Remove the one empty language that is being passed from the front-end.
            foreach (var ml in contract.KnownLanguages)
            {
                if (ml.LanguageId == 0)
                {
                    contract.KnownLanguages.Remove(ml);
                    break;
                }
            }

            // Mark all DB languages as deleted.
            foreach (var x in ls.KnownLanguages)
            {
                x.IsDeleted = true;
            }

            // Get the list of existing known languages to restore later on.
            var existingLanguagesDb = (from x in ls.AllLanguages select x).ToList();

            var home = new KnownLanguageContract
                       {
                           Id         = contract.HomeLanguageId,
                           LanguageId = contract.HomeLanguageTypeId.GetValueOrDefault(),
                           CanRead    = contract.IsAbleToReadHomeLanguage,
                           CanSpeak   = contract.IsAbleToSpeakHomeLanguage,
                           CanWrite   = contract.IsAbleToWriteHomeLanguage,
                           IsPrimary  = true
                       };

            contract.KnownLanguages.Insert(0, home);

            if (contract.HomeLanguageTypeId != englishLanguage)
            {
                ls.IsAbleToReadEnglish  = contract.IsAbleToReadEnglish;
                ls.IsAbleToWriteEnglish = contract.IsAbleToWriteEnglish;
                ls.IsAbleToSpeakEnglish = contract.IsAbleToSpeakEnglish;
                ls.InterpreterDetails   = contract.InterpreterDetails.SafeTrim();
                ls.IsNeedingInterpreter = contract.IsNeedingInterpreter;
            }
            else
            {
                foreach (var kn in contract.KnownLanguages)
                {
                    if (kn.LanguageId == Languages.EnglishId && kn.IsPrimary == false)
                    {
                        kn.LanguageId = 0;
                        kn.CanRead    = null;
                        kn.CanSpeak   = null;
                        kn.CanWrite   = null;
                    }
                }

                ls.IsAbleToReadEnglish  = null;
                ls.IsAbleToWriteEnglish = null;
                ls.IsAbleToSpeakEnglish = null;
                ls.InterpreterDetails   = null;
                ls.IsNeedingInterpreter = null;
            }

            // This list of languages can go to ∞.
            var sortOrder = 0;

            foreach (var knownLanguage in contract.KnownLanguages)
            {
                if (knownLanguage.LanguageId == 0 && knownLanguage.CanRead == null && knownLanguage.CanSpeak == null && knownLanguage.CanWrite == null)
                {
                    continue;
                }

                IKnownLanguage kl;

                if (knownLanguage.Id == 0)
                {
                    // We are looking for an existing, deleted language that we can re-use based
                    // upon its language type ID.
                    kl = existingLanguagesDb.SingleOrDefault(x => x.LanguageId == knownLanguage.LanguageId) ?? Repo.NewKnownLanguage(ls);
                }
                else
                {
                    // Otherwise we'll look it up by it's ID.
                    kl = existingLanguagesDb.SingleOrDefault(x => x.Id == knownLanguage.Id) ?? Repo.NewKnownLanguage(ls);
                }

                sortOrder++;
                kl.SortOrder     = sortOrder;
                kl.IsPrimary     = knownLanguage.IsPrimary;
                kl.LanguageId    = knownLanguage.LanguageId;
                kl.IsAbleToWrite = knownLanguage.CanWrite;
                kl.IsAbleToSpeak = knownLanguage.CanSpeak;
                kl.IsAbleToRead  = knownLanguage.CanRead;
                kl.ModifiedBy    = AuthUser.Username;
                kl.ModifiedDate  = updateTime;
                kl.IsDeleted     = false;
            }

            var currentIA = Repo.GetMostRecentAssessment(p);
            currentIA.ModifiedBy   = AuthUser.Username;
            currentIA.ModifiedDate = updateTime;

            // Do a concurrency check.
            if (!Repo.IsRowVersionStillCurrent(ls, userRowVersion))
            {
                return false;
            }

            if (!Repo.IsRowVersionStillCurrent(las, userAssessRowVersion))
            {
                return false;
            }

            // If the first save completes, it actually has already saved the LanguageAssessmentSection
            // object as well since they are on the save repository context.  But if the LanguageSection didn't
            // need saving, we still need to SaveIfChangd on the LanguageAssessmentSection.
            if (!Repo.SaveIfChanged(ls, user))
            {
                Repo.SaveIfChanged(las, user);
            }

            return true;
        }
    }
}
