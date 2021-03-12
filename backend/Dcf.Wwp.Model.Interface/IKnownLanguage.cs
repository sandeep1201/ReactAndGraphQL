using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IKnownLanguage : ICloneable, ICommonDelModel
    {
        int LanguageSectionId { get; set; }
        int LanguageId { get; set; }
        bool? IsPrimary { get; set; }
        bool? IsAbleToRead { get; set; }
        bool? IsAbleToSpeak { get; set; }
        bool? IsAbleToWrite { get; set; }

        int? SortOrder { get; set; }

        ILanguage Language { get; set; }
        ILanguageSection LanguageSection { get; set; }

        bool IsEnglish { get; }
    }
}