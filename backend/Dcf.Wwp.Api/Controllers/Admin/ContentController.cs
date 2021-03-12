using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dcf.Wwp.Api.Library.Contracts.Content;
using Dcf.Wwp.Api.Library.ViewModels.Content;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;
using Dcf.Wwp.Model.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Dcf.Wwp.Api.Controllers.Admin
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class ContentController : BaseController
    {
        private readonly IAuthUser _authUser;
        private readonly IContentService _contentService;
        private readonly JsonSerializerSettings _jsonSettings;

        public ContentController(IRepository repository, IAuthUser authUser, IContentService contentService, JsonSerializerSettings jsonSettings) : base(repository)
        {
            this._authUser = authUser;
            this._contentService = contentService;
            this._jsonSettings = jsonSettings;
        }

        [HttpGet("page/{id:int}")]
        public IActionResult GetPage(Int32 id)
        {
            var vm = new ContentViewModel(this._contentService, _jsonSettings, this.Repo, this._authUser);
            var data = vm.GetPageById(id);
            return Ok(data);
        }

        [HttpPost("page")]
        public IActionResult SavePage([FromBody] PageContract contract)
        {
            var vm = new ContentViewModel(this._contentService, _jsonSettings, this.Repo, this._authUser);
            var data = vm.UpsertData(contract);
            return Ok(data.UpdatedModel);
        }

        [HttpGet("page/{slug}")]
        public IActionResult GetPage(String slug)
        {
            var vm = new ContentViewModel(this._contentService, _jsonSettings, this.Repo, this._authUser);
            var data = vm.GetPageBySlug(slug);
            return Ok(data);
        }

        [HttpGet("module/{id}")]
        public IActionResult GetModule(Int32 id)
        {
            var vm = new ContentViewModel(this._contentService, _jsonSettings, this.Repo, this._authUser);
            var data = vm.GetModuleContractById(id);
            return Ok(data);
        }

        [HttpGet("module/meta/{id}")]
        public IActionResult GetModuleMeta(Int32 id)
        {
            var vm = new ContentViewModel(this._contentService, _jsonSettings, this.Repo, this._authUser);
            var data = vm.GetMetaContractById(id);
            return Ok(data);
        }

        // TODO: Get Collections by parentId and Get Meta by name and ModuleId

    }
}
