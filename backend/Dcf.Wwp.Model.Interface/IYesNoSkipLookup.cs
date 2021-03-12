using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IYesNoSkipLookup : ICloneable, IHasId
    {
        String Name { get; set; }
    }
}
