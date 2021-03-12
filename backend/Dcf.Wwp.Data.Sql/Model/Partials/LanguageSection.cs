using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dcf.Wwp.Model.Interface;


namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class LanguageSection : BaseCommonModel, ILanguageSection, IEquatable<LanguageSection>
    {
        ICollection<IKnownLanguage> ILanguageSection.KnownLanguages
        {
            get { return (from x in KnownLanguages where x.IsDeleted == false select x).Cast<IKnownLanguage>().ToList(); }

            set { KnownLanguages = value.Cast<KnownLanguage>().ToList(); }
        }

        ICollection<IKnownLanguage> ILanguageSection.AllLanguages
        {
            get { return KnownLanguages.Cast<IKnownLanguage>().ToList(); }

            set { KnownLanguages = value.Cast<KnownLanguage>().ToList(); }
        }


        public IKnownLanguage HomeLanguage
        {
            get { return (from x in KnownLanguages where x.IsPrimary != null && x.IsPrimary == true && x.IsDeleted == false select x).SingleOrDefault(); }
            set
            {
                IKnownLanguage existing = null;

                if (value?.Language != null)
                {
                    existing = (from x in KnownLanguages where x.Language.Code == value.Language.Name select x).SingleOrDefault();
                }

                // Update all the existing Home Languages to not be.
                var q = from x in KnownLanguages where x.IsPrimary != null && x.IsPrimary.Value select x;

                foreach (var kl in q)
                {
                    kl.IsPrimary = false;
                }

                // Check if the existing one is in the collection.  If so, just set it to
                // primary.
                if (existing != null)
                {
                    existing.IsPrimary = true;
                }
                else
                {
                    if (value != null)
                    {
                        // Otherwise we need to add it as well.
                        value.IsPrimary = true;
                        KnownLanguages.Add((KnownLanguage) value);
                    }
                }
            }
        }

        #region ICloneable

        public object Clone()
        {
            var ls = new LanguageSection();

            ls.Id                   = Id;
            ls.IsAbleToReadEnglish  = IsAbleToReadEnglish;
            ls.IsAbleToWriteEnglish = IsAbleToWriteEnglish;
            ls.IsAbleToSpeakEnglish = IsAbleToSpeakEnglish;
            ls.IsNeedingInterpreter = IsNeedingInterpreter;
            ls.InterpreterDetails   = InterpreterDetails;
            ls.Notes                = Notes;

            ls.KnownLanguages = KnownLanguages.Select(x => (KnownLanguage) x.Clone()).ToList();

            return ls;
        }

        #endregion ICloneable

        #region IEquatable<T>

#pragma warning disable 659
        public override bool Equals(object other)
#pragma warning restore 659
        {
            if (other == null)
                return false;

            var obj = other as LanguageSection;
            return obj != null && Equals(obj);
        }

        public bool Equals(LanguageSection other)
        {
            //Check whether the compared object is null.
            if (ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (ReferenceEquals(this, other)) return true;

            // Check whether the products' properties are equal.
            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(IsAbleToReadEnglish, other.IsAbleToReadEnglish))
                return false;
            if (!AdvEqual(IsAbleToSpeakEnglish, other.IsAbleToSpeakEnglish))
                return false;
            if (!AdvEqual(IsAbleToWriteEnglish, other.IsAbleToWriteEnglish))
                return false;
            if (!AdvEqual(IsNeedingInterpreter, other.IsNeedingInterpreter))
                return false;
            if (!AdvEqual(InterpreterDetails, other.InterpreterDetails))
                return false;
            if (!AdvEqual(Notes, other.Notes))
                return false;
            if (AreBothNotNull(KnownLanguages, other.KnownLanguages) && !(KnownLanguages.OrderBy(x => x.Id).SequenceEqual(other.KnownLanguages.OrderBy(x => x.Id))))
                return false;

            return true;
        }

        #endregion IEquatable<T>
    }
}
