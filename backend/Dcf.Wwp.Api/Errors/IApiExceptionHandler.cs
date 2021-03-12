using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Dcf.Wwp.Api.Errors
{
    public interface IApiExceptionHandler
    {
        Task Handle(HttpContext context, ExceptionDispatchInfo ex);
    }
}