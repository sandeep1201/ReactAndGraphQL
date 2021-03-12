using System;
using System.Net;

namespace DCF.Core.Exceptions
{
    public interface IHasHttpStatusCode
    {
        HttpStatusCode HttpStatusCode { get; set; }
    }
}