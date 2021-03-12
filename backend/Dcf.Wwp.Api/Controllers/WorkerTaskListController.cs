using Dcf.Wwp.Api.ActionFilters;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Interfaces;

namespace Dcf.Wwp.Api.Controllers
{
    [Route("api/worker-task")]
    [EnableCors("AllowAll")]
    [ServiceFilter(typeof(ValidAuthUserMustExistAttribute))]
    public class WorkerTaskListController : Controller
    {
        #region Properties

        private readonly IWorkerTaskListDomain _workerTaskListDomain;

        #endregion

        #region Methods

        public WorkerTaskListController(IWorkerTaskListDomain workerTaskListDomain)
        {
            _workerTaskListDomain = workerTaskListDomain;
        }

        [HttpGet("{wiuid}")]
        public IActionResult GetWorkerTaskList(string wiuid)
        {
            var contract = _workerTaskListDomain.GetWorkerTaskLists(wiuid);
            var res      = Ok(contract);

            return res;
        }

        [HttpPost]
        public IActionResult UpsertWorkerTaskList([FromBody] WorkerTaskListContract workerTaskListContract)
        {
            var contract = _workerTaskListDomain.UpsertWorkerTaskList(workerTaskListContract);
            var res      = Ok(contract);

            return res;
        }

        [HttpPut("{id}/{workerId}")]
        public IActionResult ReassignWorker(int id, int workerId)
        {
            _workerTaskListDomain.ReassignWorker(id, workerId);
            var res      = Ok();

            return Ok(res);
        }

        #endregion
    }
}
