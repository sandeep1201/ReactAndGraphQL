using System;
using System.Collections.Generic;


namespace Dcf.Wwp.Model.Interface
{
    public interface ILanguageSection : ICloneable, ICommonDelModel
    {
        bool? IsAbleToReadEnglish { get; set; }
        bool? IsAbleToWriteEnglish { get; set; }
        bool? IsAbleToSpeakEnglish { get; set; }
        bool? IsNeedingInterpreter { get; set; }
        string InterpreterDetails { get; set; }
        string Notes { get; set; }

        ICollection<IKnownLanguage> KnownLanguages { get; set; }
        ICollection<IKnownLanguage> AllLanguages { get; set; }

        IKnownLanguage HomeLanguage { get; set; }
    }
}