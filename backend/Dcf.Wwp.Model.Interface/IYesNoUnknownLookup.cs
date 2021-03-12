using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IYesNoUnknownLookup : ICloneable
    {
        Int32 Id { get; set; }
        String Code { get; set; }
        String Name { get; set; }
    }
}

