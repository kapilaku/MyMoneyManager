using Microsoft.AspNetCore.Mvc;
using MyMoneyManager.API.Interfaces.IServices;
using MyMoneyManager.Shared.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyMoneyManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }
        // GET: api/<TransactionController>
        [HttpGet]
        public async Task<IEnumerable<TransactionViewModel>> Get(CancellationToken cancellationToken)
        {
            return await _transactionService.GetUserTransactions(HttpContext, cancellationToken);
        }

        // GET api/<TransactionController>/5
        [HttpGet("{id}")]
        public TransactionViewModel Get(int id, CancellationToken cancellationToken)
        {
            return _transactionService.GetUserTransaction(HttpContext, id, cancellationToken);
        }

        // POST api/<TransactionController>
        [HttpPost]
        public async Task Post([FromBody] TransactionViewModel transactionViewModel, CancellationToken cancellationToken)
        {
            await _transactionService.CreateUserTransaction(HttpContext, transactionViewModel, cancellationToken);
        }

        // PUT api/<TransactionController>/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] TransactionViewModel transactionViewModel, CancellationToken cancellationToken)
        {
            await _transactionService.UpdateUserTransaction(HttpContext, transactionViewModel, cancellationToken);
        }

        // DELETE api/<TransactionController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id, CancellationToken cancellationToken)
        {
            await _transactionService.DeleteUserTransaction(HttpContext, id, cancellationToken);
        }
    }
}
