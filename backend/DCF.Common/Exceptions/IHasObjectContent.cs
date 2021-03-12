using System;

namespace DCF.Core.Exceptions
{
    public interface IHasObjectContent
    {
        Object Content { get; set; }
    }
}