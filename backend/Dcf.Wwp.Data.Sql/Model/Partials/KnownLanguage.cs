using System;
using Dcf.Wwp.Model.Interface;
using System.ComponentModel.DataAnnotations;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class KnownLanguage : BaseCommonModel, IKnownLanguage, IEquatable<KnownLanguage>
    {
        ILanguage IKnownLanguage.Language
        {
            get { return Language; }

            set { Language = (Language) value; }
        }

        ILanguageSection IKnownLanguage.LanguageSection
        {
            get { return LanguageSection; }

            set { LanguageSection = (LanguageSection) value; }
        }

        public bool IsEnglish => (Language?.Code == "en");

        #region ICloneable

        public object Clone()
        {
            var ls = new KnownLanguage();

            ls.Id                = this.Id;
            ls.LanguageSectionId = this.LanguageSectionId;
            ls.LanguageId        = this.LanguageId;
            ls.IsPrimary         = this.IsPrimary;
            ls.IsDeleted         = this.IsDeleted;
            ls.IsAbleToRead      = this.IsAbleToRead;
            ls.IsAbleToSpeak     = this.IsAbleToSpeak;
            ls.IsAbleToWrite     = this.IsAbleToWrite;
            ls.SortOrder         = this.SortOrder;
            ls.Language          = (Language) this.Language?.Clone();

            // NOTE: We don't clone references to "parent" objects such as LanguageSection

            return ls;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as KnownLanguage;
            return obj != null && Equals(obj);
        }

        public bool Equals(KnownLanguage other)
        {
            // Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            // Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;


            // Check whether the products' properties are equal.
            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(LanguageSectionId, other.LanguageSectionId))
                return false;
            if (!AdvEqual(LanguageId, other.LanguageId))
                return false;
            if (!AdvEqual(IsPrimary, other.IsPrimary))
                return false;
            if (!AdvEqual(IsDeleted, other.IsDeleted))
                return false;
            if (!AdvEqual(IsAbleToRead, other.IsAbleToRead))
                return false;
            if (!AdvEqual(IsAbleToSpeak, other.IsAbleToSpeak))
                return false;
            if (!AdvEqual(IsAbleToWrite, other.IsAbleToWrite))
                return false;
            if (!AdvEqual(SortOrder, other.SortOrder))
                return false;
            if (!AdvEqual(Language, other.Language))
                return false;

            return true;
        }

        #endregion IEquatable<T>
    }
}
