using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Dcf.Wwp.DataAccess.Interfaces;

namespace Dcf.Wwp.Api.ActionFilters
{
    public class ValidEpIdMustExistAttribute : IActionFilter
    {
        #region Properties

        private readonly IEmployabilityPlanRepository _epRepo;

        #endregion

        #region Methods

        public ValidEpIdMustExistAttribute(IEmployabilityPlanRepository epRepo)
        {
            _epRepo = epRepo;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

            var param = context.ActionArguments.FirstOrDefault(p => p.Key == "epId");
            
            if (param.Key == null || param.Value == null)
            {
                var objectResult = new ObjectResult(new { error = "PIN or Employability-Plan Id is null." })
                                   {
                                       StatusCode = StatusCodes.Status400BadRequest
                                   };

                context.Result = objectResult;
                return;
            }

            var id = (int)param.Value;

            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }

            var exists = _epRepo.GetAsQueryable().Any(i => i.Id == id);

            if (!exists)
            {
                var objectResult = new ObjectResult(new { error = "PIN or Employability-Plan Id is not valid." })
                                   {
                                       StatusCode = StatusCodes.Status400BadRequest
                                   };

                context.Result = objectResult;
            }

            var z = 0;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        #endregion
    }
}
