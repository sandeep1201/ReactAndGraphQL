using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Dcf.Wwp.Api.Common;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using ExceptionContext = Microsoft.AspNetCore.Mvc.Filters.ExceptionContext;

namespace Dcf.Wwp.Api.Errors
{
    /// <summary>
    /// Exception Log Filter class
    /// </summary>
    //public class ApiExceptionLogger : IExceptionFilter
    //{
    //    private ILogger _logger;
    //    private ApiExceptionHandler _handler;

    //    public ApiExceptionLogger(IErrorInfoConverter errorInfoConverter)
    //    {
    //        this._logger = Serilog.Log.Logger;
    //        this._handler = new ApiExceptionHandler(errorInfoConverter);
    //    }

    //    public void Log(ExceptionContext context, ErrorInfo info)
    //    {
    //        this._logger.Error(context.Exception, "Handled Exception occured. {message}, {@errorInfo}",info.Message, info);
    //    }

    //    public void OnException(ExceptionContext context)
    //    {
    //        try
    //        {
    //            var errorInfo = this._handler.Handle(context.HttpContext,context.Exception);
    //            this.Log(context,errorInfo);
    //        }
    //        catch
    //        {
    //            //Uh-oh
    //        }
    //    }
    //}
}
