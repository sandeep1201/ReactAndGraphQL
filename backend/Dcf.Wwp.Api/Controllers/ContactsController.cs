using System;
using Dcf.Wwp.Api.ActionFilters;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.ViewModels.ContactsApp;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Api.Controllers
{
    [Route("api/contacts")]
    [EnableCors("AllowAll")]
    [ServiceFilter(typeof(ValidAuthUserMustExistAttribute))]
    public class ContactsController : BaseController
    {
        #region Properties

        private readonly IAuthUser _authUser;

        #endregion

        #region Methods

        public ContactsController (IAuthUser authUser, IRepository repository) : base (repository)
        {
            _authUser = authUser;
        }

        [HttpGet("{pin}/{id}")]
        public IActionResult ContactById(string pin, int id)
        {
            try
            {
                var vm = new ContactsViewModel(Repo, _authUser);
                vm.InitializeFromPin(pin);

                if (!vm.IsPinValid)
                    return BadRequest(new { error = "PIN is not valid." });

                var data = vm.GetParticipantContact(id);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = $"Something unexpected happened. {ex}" });
            }
        }


        [HttpGet("{pin}")]
        public IActionResult ParticipantContacts(string pin)
        {
            try
            {
                var vm = new ContactsViewModel(Repo, _authUser);
                vm.InitializeFromPin(pin);

                if (!vm.IsPinValid)
                    return BadRequest(new { error = "PIN is not valid." });

                var data = vm.GetParticipantContacts();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = $"Something unexpected happened. {ex}" });
            }
        }

        [HttpPost("{pin}/{id}")]
        public IActionResult PostContacts(string pin, int id, [FromBody] ContactContract model)
        {
            if (!_authUser.IsAuthenticated) return Unauthorized();

            if (ModelState.IsValid && model != null)
            {
                try
                {
                    var vm = new ContactsViewModel(Repo, _authUser);
                    vm.InitializeFromPin(pin);

                    // Check if we have everything we need (a valid Pin and valid
                    // assessment to display).
                    if (!vm.IsPinValid)
                        return BadRequest(new { error = "PIN is not valid." });

                    var response = vm.UpsertData(model, pin, id, _authUser.Username);

                    if (response.HasConcurrencyError)
                        return HttpConflict();

                    // We return back the contact as it helps when it's a new
                    // contact and the ID is not known.
                    var data = vm.GetParticipantContact(response.UpdatedModel.Id);
                    return Ok(data);
                }
                catch (Exception ex)
                {
                    return BadRequest(new { error = ex.Message });
                }
            }

            return BadRequest();
        }

        [HttpDelete("delete/{pin}/{id}")]
        public IActionResult DeleteContact(string pin, int id)
        {
            if (!_authUser.IsAuthenticated) return Unauthorized();

            try
            {
                var vm = new ContactsViewModel(Repo, _authUser);
                vm.InitializeFromPin(pin);

                var didDelete = vm.DeleteContact(id, _authUser.Username);
                if (didDelete)
                    return Ok(); // 200
                else
                    return NoContent(); // 204
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = $"Something unexpected happened. {ex}" });
            }
        }

        #endregion
    }
}
