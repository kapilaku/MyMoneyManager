using Microsoft.AspNetCore.Mvc;
using MyMoneyManager.API.Interfaces.IServices;
using MyMoneyManager.Shared.ViewModels;
using System.Buffers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyMoneyManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SplitController : ControllerBase
    {
        private readonly ISplitService _splitService;
        public SplitController(ISplitService splitService)
        {
            _splitService = splitService;
        }
        // GET: api/<SplitController>
        [HttpGet]
        public async Task<IEnumerable<SplitViewModel>> Get(CancellationToken cancellationToken)
        {
            return await _splitService.GetUserSplits(HttpContext, cancellationToken);
        }

        // GET api/<SplitController>/5
        [HttpGet("{id}")]
        public SplitViewModel Get(int id, CancellationToken cancellationToken)
        {
            return _splitService.GetUserSplit(HttpContext, id, cancellationToken);
        }

        // POST api/<SplitController>
        [HttpPost]
        public async Task Post([FromBody] SplitViewModel splitViewModel, CancellationToken cancellationToken)
        {
            await _splitService.CreateUserSplit(HttpContext, splitViewModel, cancellationToken);
        }

        // PUT api/<SplitController>/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] SplitViewModel splitViewModel, CancellationToken cancellationToken)
        {
            await _splitService.UpdateUserSplit(HttpContext, splitViewModel, cancellationToken);
        }

        // DELETE api/<SplitController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id, CancellationToken cancellationToken)
        {
            await _splitService.DeleteUserSplit(HttpContext, id, cancellationToken);
        }
    }
}
