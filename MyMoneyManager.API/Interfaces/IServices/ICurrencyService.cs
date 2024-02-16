using MyMoneyManager.Shared.ViewModels;

namespace MyMoneyManager.API.Interfaces.IServices
{
    public interface ICurrencyService
    {
        public Task<IEnumerable<CurrencyViewModel>> GetUserCurrencies(HttpContext httpContext, CancellationToken cancellationToken);
        public CurrencyViewModel GetUserCurency(HttpContext httpContext, int id, CancellationToken cancellationToken);
        public Task DeleteUserCurrency(HttpContext httpContext, int id, CancellationToken cancellationToken);
        public Task CreateUserCurrency(HttpContext httpContext, CurrencyViewModel currencyViewModel, CancellationToken cancellationToken);
        public Task UpdateUserCurrency(HttpContext httpContext, CurrencyViewModel currencyViewModel, CancellationToken cancellationToken);
    }
}
