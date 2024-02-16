using MyMoneyManager.Shared.ViewModels;

namespace MyMoneyManager.API.Interfaces.IServices
{
    public interface IAccountService
    {
        public Task<IEnumerable<AccountViewModel>> GetUserAccounts(HttpContext httpContext, CancellationToken cancellationToken);
        public AccountViewModel GetUserAccount(HttpContext httpContext, int id, CancellationToken cancellationToken);
        public Task DeleteUserAccount(HttpContext httpContext, int id, CancellationToken cancellationToken);
        public Task CreateUserAccount(HttpContext httpContext, AccountViewModel accountViewModel, CancellationToken cancellationToken);
        public Task UpdateUserAccount(HttpContext httpContext, AccountViewModel accountViewModel, CancellationToken cancellationToken);
    }
}
