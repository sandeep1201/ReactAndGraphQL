using System;
using System.Collections.Generic;
using System.Linq;
using EnumsNET;

namespace DCF.Timelimits.Rules.Domain
{
    [Flags]
    public enum ClockTypes
    {
        None = 0,
        Federal = 1,            // 1 << 0  =  00000000000001 = 1
        State = 2,              // 1 << 1  =  00000000000010 = 2
        CSJ = 4,                // 1 << 2  =  00000000000100 = 4
        W2T = 8,                // 1 << 3  =  00000000001000 = 8
        TNP = 16,               // 1 << 4  =  00000000010000 = 16
        TMP = 32,               // 1 << 5  =  00000000100000 = 32
        CMC = 64,               // 1 << 6  =  00000001000000 = 64
        OPC = 128,              // 1 << 7  =  00000010000000 = 128
        OTF = 256,              // 1 << 8  =  00000100000000 = 256
        TRIBAL = 512,           // 1 << 9  =  00001000000000 = 512
        TJB = 1024,             // 1 << 10 =  00010000000000 = 1024
        JOBS = 2048,            // 1 << 11 =  00100000000000 = 2048
        NoPlacementLimit = 4096,             // 1 << 12 =  01000000000000 = 4096
        TEMP = ClockTypes.TNP | ClockTypes.TMP,
        Other = ClockTypes.OTF | ClockTypes.TRIBAL | ClockTypes.NoPlacementLimit,
        PlacementLimit = ClockTypes.CSJ | ClockTypes.W2T | ClockTypes.TEMP,
        PlacementTypes = PlacementLimit | ClockTypes.CMC | ClockTypes.JOBS | ClockTypes.TJB,
        ExtensableTypes = ClockTypes.PlacementLimit | ClockTypes.State,
        CreateableTypes = ClockTypes.OTF | ClockTypes.TRIBAL | ClockTypes.TJB | ClockTypes.JOBS | ClockTypes.OPC | ClockTypes.CMC | ClockTypes.PlacementLimit,
        OtherTypes = ClockTypes.OTF | ClockTypes.TRIBAL | ClockTypes.TJB | ClockTypes.JOBS
    }

    public static class ClockTypesExtensions
    {
        public static Boolean IsSingleFlag(ClockTypes flags)
        {

            return FlagEnums.GetFlags(flags).Any();
            //TODO: Perf against operations against eachother
            //return flags.Equals(0) || (flags & (flags - 1)).Equals(0);
        }

        public static IEnumerable<ClockTypes> GetClockTypeFlags(ClockTypes flags)
        {

            var allFlags = FlagEnums.GetAllFlags<ClockTypes>();
            return FlagEnums.GetFlags(allFlags);

            //ulong flag = 1;
            //foreach (var value in Enum.GetValues(flags.GetType()).Cast<T>())
            //{
            //    ulong bits = Convert.ToUInt64(value);
            //    while (flag < bits)
            //    {
            //        flag <<= 1;
            //    }

            //    if (flag == bits && flags.HasFlag(value))
            //    {
            //        yield return value;
            //    }
            //}
        }
    }

}