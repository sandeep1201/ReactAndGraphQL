using System;

namespace Dcf.Wwp.Api.Common
{
    public interface IErrorInfoConverter
    {
        ErrorInfo Convert(Exception exception);
    }
}