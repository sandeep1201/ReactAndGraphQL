using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Dcf.Wwp.Model.Interface.Core;
using Microsoft.AspNetCore.Http;

namespace Dcf.Wwp.Api.ActionFilters
{
    public class ValidAuthUserMustExistAttribute : IActionFilter
    {
        #region Properties

        private readonly IAuthUser _authUser;

        #endregion

        #region Methods

        public ValidAuthUserMustExistAttribute(IAuthUser authUser)
        {
            _authUser = authUser;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (_authUser == null || (_authUser != null && !_authUser.IsAuthenticated))
            {
                var objectResult = new ObjectResult(new { error = "User is not authorized" })
                                   {
                                       StatusCode = StatusCodes.Status401Unauthorized
                                   };

                context.Result = objectResult;
                return;
            }

            if (context.ModelState.IsValid) return;
            context.Result = new BadRequestObjectResult(context.ModelState);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        #endregion
    }
}
