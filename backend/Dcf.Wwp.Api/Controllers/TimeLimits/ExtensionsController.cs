using System;
using System.Collections.Generic;
using Dcf.Wwp.Api.Library.ViewModels;
using Dcf.Wwp.Model.Interface.Repository;
using Microsoft.AspNetCore.Mvc;
using Dcf.Wwp.Api.ActionFilters;
using Dcf.Wwp.Api.Library.Contracts.Timelimits;
using Dcf.Wwp.Api.Library.Services;
using Dcf.Wwp.Model.Interface.Core;
using DCF.Common.Exceptions;
using DCF.Timelimts.Service;


namespace Dcf.Wwp.Api.Controllers.TimeLimits
{
    [Route("api/[controller]")]
    public class ExtensionsController : BaseController
    {
        private IAuthUser _authUser;
        private readonly IDb2TimelimitService _db2TimelimitService;
        private ITimelimitService _timelimitService;

        public ExtensionsController(IRepository repo, IAuthUser authUser, IDb2TimelimitService db2TimelimitService, ITimelimitService timelimitService) : base(repo)
        {
            this._authUser = authUser;
            this._db2TimelimitService = db2TimelimitService;
            this._timelimitService = timelimitService;
        }


        [HttpGet("{pin}/{id}")]
        public IActionResult GetExtensionSequenceById(string pin, Int32 id)
        {
                var vm = new ExtensionSequenceViewModel(this.Repo,this._authUser, _timelimitService, _db2TimelimitService);
                vm.InitializeFromPin(pin);
                if (!vm.IsPinValid)
                    throw new UserFriendlyException("PIN is not valid.");

                var data = vm.GetExtensionSequenceById(id);
                return this.Ok(data);
        }

        [HttpGet("reasons/denial")]
        public IActionResult GetExtensionDenialReasons()
        {
                var vm = new ExtensionReasonViewModel(this.Repo,this._authUser);
                var data = vm.GetDenialExtensionReasons();
                return this.Ok(data);
        }

        [HttpGet("reasons/approval")]
        public IActionResult GetExtensionApprovalReasons()
        {
                var vm = new ExtensionReasonViewModel(this.Repo,this._authUser);
                var data = vm.GetApprovalExtensionReasons();
                return this.Ok(data);
        }

        [ValidationResponseFilter]
        [HttpPost("{pin}")]
        public IActionResult SaveExtension(string pin, [FromBody] ExtensionContract model)
        {
                var vm = new ExtensionSequenceViewModel(this.Repo, this._authUser, _timelimitService, _db2TimelimitService);
                vm.InitializeFromPin(pin);
                if (!vm.IsPinValid)
                    throw new UserFriendlyException("PIN is not valid.");

                var savedData = vm.UpsertData(model);

                return this.Ok(savedData.UpdatedModel);

        }

        [HttpGet("{pin}/notice/{id}")]
        public IActionResult GenerateNoticeTrigger(string pin, Int32 id)
        {
            var vm = new ExtensionSequenceViewModel(this.Repo,this._authUser, _timelimitService, this._db2TimelimitService);
            vm.InitializeFromPin(pin);
            if (!vm.IsPinValid)
                throw new UserFriendlyException("PIN is not valid.");

            var data = vm.GenerateExtensionNotice(id);
            return this.Ok(data);
        }

        [HttpGet("{pin}/notice/{id}/sql")]
        public IActionResult GenerateNoticeTriggerSql(string pin, Int32 id)
        {
            var vm = new ExtensionSequenceViewModel(this.Repo, this._authUser, _timelimitService, this._db2TimelimitService);
            vm.InitializeFromPin(pin);
            if (!vm.IsPinValid)
                throw new UserFriendlyException("PIN is not valid.");

            var data = vm.GenerateExtensionNotice(id);
            var dataString = $@"
                DECLARE @CS_RFA_PRV_PIN_NUM varchar(10) = '{data.CS_RFA_PRV_PIN_NUM}'
                DECLARE @CS_RFA_PRV_PIN_IND varchar(1) = '{data.CS_RFA_PRV_PIN_IND}'
                DECLARE @DEPT_ID varchar(4) = '{data.DEPT_ID}'
                DECLARE @PROGRAM_CD varchar(3) = '{data.PROGRAM_CD}'
                DECLARE @SUBPROGRAM_CD varchar(1) = '{data.SUBPROGRAM_CD}'
                DECLARE @AG_SEQ_NUM varchar(4) = '{data.AG_SEQ_NUM}'
                DECLARE @RQST_TMS varchar(26) = '{data.RQST_TMS}'
                DECLARE @CNTY_NUM varchar(4) = '{data.CNTY_NUM}'
                DECLARE @CRE_IND varchar(1) = '{data.CRE_IND}'
                DECLARE @DOC_CD varchar(4) = '{data.DOC_CD}'
                DECLARE @LTR_MO varchar(6) = '{data.LTR_MO}'
                DECLARE @OFC_NUM varchar(4) = '{data.OFC_NUM}'
                DECLARE @PROC_DT varchar(10) = '{data.PROC_DT}'
                DECLARE @PRVD_LOC_NUM varchar(4) = '{data.PRVD_LOC_NUM}'
                DECLARE @SEC_RCPT_ID varchar(10) = '{data.SEC_RCPT_ID}'
                DECLARE @SPRS_USER_ID varchar(6) = '{data.SPRS_USER_ID}'
                DECLARE @USER_ID varchar(6) = '{data.USER_ID}'
                DECLARE @LTR_TXT varchar(2400) = '{data.LTR_TXT}'

                EXECUTE [wwp].[DB2_T0754_Insert] @CS_RFA_PRV_PIN_NUM  ,@CS_RFA_PRV_PIN_IND  ,@DEPT_ID  ,@PROGRAM_CD  ,@SUBPROGRAM_CD  ,@AG_SEQ_NUM  ,@RQST_TMS  ,@CNTY_NUM  ,@CRE_IND  ,@DOC_CD  ,@LTR_MO  ,@OFC_NUM  ,@PROC_DT  ,@PRVD_LOC_NUM  ,@SEC_RCPT_ID  ,@SPRS_USER_ID  ,@USER_ID  ,@LTR_TXT
            ";
            
            return this.Content(dataString);
        }

        [HttpPost("{pin}/notice/{id}")]
        public IActionResult InsertNoticeTrigger(string pin, Int32 id)
        {
            var vm = new ExtensionSequenceViewModel(this.Repo, this._authUser, _timelimitService, this._db2TimelimitService);
            vm.InitializeFromPin(pin);
            if (!vm.IsPinValid)
                throw new UserFriendlyException("PIN is not valid.");

            var data = vm.InsertExtensionNotice(id);
            return this.Ok(data);
        }
    }
}