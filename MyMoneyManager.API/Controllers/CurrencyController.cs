using Microsoft.AspNetCore.Mvc;
using MyMoneyManager.API.Interfaces.IServices;
using MyMoneyManager.API.Services;
using MyMoneyManager.Shared.ViewModels;
using System.Threading;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyMoneyManager.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CurrencyController : ControllerBase
{
    private readonly ICurrencyService _currencyService;
    public CurrencyController(ICurrencyService currencyService)
    {
        _currencyService = currencyService;
    }

    // GET: api/<CurrencyController>
    [HttpGet]
    public async Task<IEnumerable<CurrencyViewModel>> Get(CancellationToken cancellationToken)
    {
        return await _currencyService.GetUserCurrencies(HttpContext, cancellationToken);
    }

    // GET api/<CurrencyController>/5
    [HttpGet("{id}")]
    public CurrencyViewModel Get(int id, CancellationToken cancellationToken)
    {
        return _currencyService.GetUserCurency(HttpContext, id, cancellationToken);
    }

    // POST api/<CurrencyController>
    [HttpPost]
    public async Task Post([FromBody] CurrencyViewModel currencyViewModel, CancellationToken cancellationToken)
    {
        await _currencyService.CreateUserCurrency(HttpContext, currencyViewModel, cancellationToken);
    }

    // PUT api/<CurrencyController>/5
    [HttpPut("{id}")]
    public async Task Put(int id, [FromBody] CurrencyViewModel currencyViewModel, CancellationToken cancellationToken)
    {
        currencyViewModel.Id = id;
        await _currencyService.UpdateUserCurrency(HttpContext, currencyViewModel, cancellationToken);
    }

    // DELETE api/<CurrencyController>/5
    [HttpDelete("{id}")]
    public async Task Delete(int id, CancellationToken cancellationToken)
    {
        await _currencyService.DeleteUserCurrency(HttpContext, id, cancellationToken);
    }
}
