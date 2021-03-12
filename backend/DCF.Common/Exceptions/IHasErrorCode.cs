using System;

namespace DCF.Core.Exceptions
{
    public interface IHasErrorCode
    {
        Int32 Code { get; set; }
    }
}