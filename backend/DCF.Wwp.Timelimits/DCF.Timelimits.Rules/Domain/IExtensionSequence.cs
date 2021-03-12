using System.Collections.Generic;

namespace DCF.Timelimits.Rules.Domain
{
    public interface IExtensionSequence
    {
        ClockTypes ClockType { get; set; }
        Extension CurrentExtension { get; }
        int SequenceId { get; set; }

        IReadOnlyList<Extension> GetAllExtensions();
    }
}