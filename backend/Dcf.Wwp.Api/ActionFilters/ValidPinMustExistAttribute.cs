using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Dcf.Wwp.DataAccess.Interfaces;

namespace Dcf.Wwp.Api.ActionFilters
{
    public class ValidPinMustExistAttribute : IActionFilter
    {
        #region Properties

        private readonly IParticipantRepository _paticipantRepo;

        #endregion

        #region Methods

        public ValidPinMustExistAttribute(IParticipantRepository paticipantRepo)
        {
            _paticipantRepo = paticipantRepo;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var param = context.ActionArguments.FirstOrDefault(p => p.Value is string);

            if (param.Key == null || param.Value == null)
            {
                var objectResult = new ObjectResult(new { error = "PIN is not valid." })
                                   {
                                       StatusCode = StatusCodes.Status400BadRequest
                                   };

                context.Result = objectResult;
                return;
            }

            decimal pin = 0;

            decimal.TryParse((string) param.Value, out pin);
            
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }

            var exists = _paticipantRepo.GetAsQueryable().Any(i => i.PinNumber == pin);

            if (!exists)
            {
                var objectResult = new ObjectResult(new { error = "PIN is not valid." })
                                   {
                                       StatusCode = StatusCodes.Status400BadRequest
                                   };

                context.Result = objectResult;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        #endregion
    }
}
