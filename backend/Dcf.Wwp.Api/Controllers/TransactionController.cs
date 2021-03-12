using Dcf.Wwp.Api.ActionFilters;
using Dcf.Wwp.Api.Library.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Dcf.Wwp.Api.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/transaction")]
    [ServiceFilter(typeof(ValidAuthUserMustExistAttribute))]
    public class TransactionController : Controller
    {
        #region Properties

        private readonly ITransactionDomain _transactionDomain;

        #endregion

        #region Methods

        public TransactionController(ITransactionDomain transactionDomain)
        {
            _transactionDomain = transactionDomain;
        }


        [HttpGet("list/{participantId?}")]
        public IActionResult GetTransactionsByParticipant(int participantId)
        {
            return Ok(_transactionDomain.GetTransactions(participantId));
        }

        #endregion
    }
}
