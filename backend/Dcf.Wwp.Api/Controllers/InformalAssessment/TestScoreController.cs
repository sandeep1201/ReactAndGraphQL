using System;
using Dcf.Wwp.Api.ActionFilters;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Api.Library.ViewModels.TestScoresApp;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Dcf.Wwp.Api.Controllers.InformalAssessment
{
    [Route("api/test-score")]
    [EnableCors("AllowAll")]
    [ServiceFilter(typeof(ValidAuthUserMustExistAttribute))]
    public class TestScoreController : BaseController
    {
        private readonly IAuthUser _authUser;
        private readonly ITransactionDomain _transactionDomain;

        public TestScoreController(IRepository repository, IAuthUser authUser, ITransactionDomain transactionDomain) : base(repository)
        {
            _authUser = authUser;
            _transactionDomain = transactionDomain;
        }

        [HttpGet("{pin}")]
        [EnableCors("AllowAll")]
        public IActionResult GetEducationExams(string pin)
        {
            try
            {
                var vm = new TestScoreViewModel(Repo, _authUser, _transactionDomain);
                vm.InitializeFromPin(pin);

                if (!vm.IsPinValid)
                    return BadRequest(new { error = "PIN is not valid." });

                var data = vm.GetEducationExams(pin);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = $"Something unexpected happened. {ex}" });
            }
        }

        [HttpGet("{pin}/exam/{id}")]
        [EnableCors("AllowAll")]
        public IActionResult GetExamById(string pin, int id)
        {
            try
            {
                var vm = new TestScoreViewModel(Repo, _authUser, _transactionDomain);
                vm.InitializeFromPin(pin);

                if (!vm.IsPinValid)
                    return BadRequest(new { error = "PIN is not valid." });

                var data = vm.GetExamById(pin, id);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = $"Something unexpected happened. {ex}" });
            }
        }


        [HttpGet("{pin}/{examName}")]
        [EnableCors("AllowAll")]
        public IActionResult GetEducationExamsByType(string pin, string examName)
        {
            try
            {
                var vm = new TestScoreViewModel(Repo, _authUser, _transactionDomain);
                vm.InitializeFromPin(pin);

                if (!vm.IsPinValid)
                    return BadRequest(new { error = "PIN is not valid." });

                var data = vm.GetExamsByType(pin, examName);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = $"Something unexpected happened. {ex}" });
            }
        }

        [HttpPost]
        [Route("{pin}")]
        [EnableCors("AllowAll")]
        public IActionResult PostEducationExams(string pin, [FromBody] ExamScoreContract model)
        {
            if (!_authUser.IsAuthenticated)
                return Unauthorized();

            if (ModelState.IsValid && model != null)
            {
                try
                {
                    var vm = new TestScoreViewModel(Repo, _authUser, _transactionDomain);
                    vm.InitializeFromPin(pin);

                    // Check if we have everything we need (a valid Pin and valid
                    // examscores to display).
                    if (!vm.IsPinValid)
                        return BadRequest(new { error = "PIN is not valid." });

                    var id = vm.PostEducationExam(model, _authUser.Username);

                    return Ok(id);
                }
                catch (Exception ex)
                {
                    Response.StatusCode = 422; // Replace .AddHeader                            
                    return Json(ex);
                }
            }

            // Breakpoint, Log or examine the list with Exceptions.
            //var errors = ModelState.SelectMany(x => x.Value.Errors.Select(z => z.Exception));

            return BadRequest();
        }

        [HttpPost]
        [Route("{pin}/delete/{Id}")]
        [EnableCors("AllowAll")]
        public IActionResult DeleteExam(string pin, string id)
        {
            try
            {
                var vm = new TestScoreViewModel(Repo, _authUser, _transactionDomain);
                vm.InitializeFromPin(pin);
                if (!vm.IsPinValid)
                    return BadRequest(new { error = "PIN is not valid." });

                var data = TestScoreViewModel.DeleteData(pin, id, _authUser.Username, Repo);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = $"Something unexpected happened. {ex}" });
            }
        }
    }
}
