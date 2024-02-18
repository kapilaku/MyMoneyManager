using Microsoft.AspNetCore.Mvc;
using MyMoneyManager.API.Interfaces.IServices;
using MyMoneyManager.Shared.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyMoneyManager.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{

    private readonly IAccountService _accountService;
    private readonly ISplitService _splitService;
    public AccountController(IAccountService accountService, ISplitService splitService)
    {
        _accountService = accountService;
        _splitService = splitService;
    }

    // GET: api/<AccountController>
    [HttpGet]
    public async Task<IEnumerable<AccountViewModel>> Get(CancellationToken cancellationToken)
    {
        return await _accountService.GetUserAccounts(HttpContext, cancellationToken);
    }

    // GET api/<AccountController>/5
    [HttpGet("{id}")]
    public AccountViewModel Get(int id, CancellationToken cancellationToken)
    {
        return _accountService.GetUserAccount(HttpContext, id, cancellationToken);
    }

    [HttpGet("{accountId}/splits")]
    public async Task<IEnumerable<SplitViewModel>> GetSplits([FromRoute] int accountId, CancellationToken cancellationToken)
    {
        return await _splitService.GetUserSplitsByAccountId(HttpContext, accountId, cancellationToken);
    }

    // POST api/<AccountController>
    [HttpPost]
    public async Task Post([FromBody] AccountViewModel accountViewModel, CancellationToken cancellationToken)
    {
        await _accountService.CreateUserAccount(HttpContext, accountViewModel, cancellationToken);
    }

    // PUT api/<AccountController>/5
    [HttpPut("{id}")]
    public async Task Put(int id, [FromBody] AccountViewModel accountViewModel, CancellationToken cancellationToken)
    {
        await _accountService.UpdateUserAccount(HttpContext, accountViewModel, cancellationToken);
    }

    // DELETE api/<AccountController>/5
    [HttpDelete("{id}")]
    public async Task Delete(int id, CancellationToken cancellationToken)
    {
        await _accountService.DeleteUserAccount(HttpContext, id, cancellationToken);
    }
}
