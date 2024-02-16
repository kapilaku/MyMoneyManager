using Microsoft.EntityFrameworkCore;
using MyMoneyManager.API.Interfaces.IServices;
using MyMoneyManager.API.Models;
using MyMoneyManager.Shared.ViewModels;

namespace MyMoneyManager.API.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly IUserService _userService;
        private readonly AppDbContext _appDbContext;
        public CurrencyService(IUserService userService, AppDbContext dbContext)
        {
            _userService = userService;
            _appDbContext = dbContext;
        }

        public CurrencyViewModel GetUserCurency(HttpContext httpContext, int id, CancellationToken cancellationToken)
        {
            var userid = _userService.GetUserId(httpContext);
            var tmp = _appDbContext.Currency.Where(t => t.AppUserId == userid && t.Id == id).First();
            var currencyViewModel = new CurrencyViewModel { Id = tmp.Id, CurrencyCode = tmp.CurrencyCode, CurrencyName = tmp.CurrencyName };
            return currencyViewModel;
        }

        public async Task<IEnumerable<CurrencyViewModel>> GetUserCurrencies(HttpContext httpContext, CancellationToken cancellationToken)
        {
            var userid = _userService.GetUserId(httpContext);
            var currencies = await _appDbContext.Currency.Where(t => t.AppUserId == userid).ToListAsync();
            var currencyViewModelList = new List<CurrencyViewModel>();
            foreach (var item in currencies)
            {
                currencyViewModelList.Add(new CurrencyViewModel { Id = item.Id, CurrencyCode = item.CurrencyCode, CurrencyName = item.CurrencyName });
            }
            return currencyViewModelList;
        }

        public async Task CreateUserCurrency(HttpContext httpContext, CurrencyViewModel currencyViewModel, CancellationToken cancellationToken)
        {
            var userid = _userService.GetUserId(httpContext);
            var currency = new Currency()
            {
                AppUserId = userid,
                CurrencyCode = currencyViewModel.CurrencyCode,
                CurrencyName = currencyViewModel.CurrencyName,
                Id = currencyViewModel.Id,
            };
            await _appDbContext.Currency.AddAsync(currency);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task UpdateUserCurrency(HttpContext httpContext, CurrencyViewModel currencyViewModel, CancellationToken cancellationToken)
        {
            var userid = _userService.GetUserId(httpContext);
            var tmp = _appDbContext.Currency.Where(t => t.AppUserId == userid && t.Id == currencyViewModel.Id).First();
            tmp.CurrencyName = currencyViewModel.CurrencyName;
            tmp.CurrencyCode = currencyViewModel.CurrencyCode;
            _appDbContext.Currency.Update(tmp);
            await _appDbContext.SaveChangesAsync();
        }
        
        public async Task DeleteUserCurrency(HttpContext httpContext, int id, CancellationToken cancellationToken)
        {
            var userid = _userService.GetUserId(httpContext);
            var tmp = _appDbContext.Currency.Where(t => t.AppUserId == userid && t.Id == id).First();

            _appDbContext.Currency.Remove(tmp);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
