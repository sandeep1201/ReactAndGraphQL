using System;

namespace DCF.Timelimits.Rules.Domain
{
    [Flags]
    public enum ClockStates
    {
        None = 0,
        Warn = 1,
        Danger = 2,
        OutOfTicks = 4,
        InExtension = 8,
        InStateExtensionGap = 16,
        CoveredByStateExtension = 32
    }
}