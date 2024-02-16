using Microsoft.EntityFrameworkCore;
using MyMoneyManager.API.Interfaces.IServices;
using MyMoneyManager.API.Models;
using MyMoneyManager.Shared.ViewModels;

namespace MyMoneyManager.API.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserService _userService;
        private readonly AppDbContext _appDbContext;
        private readonly ICurrencyService _currencyService;
        public AccountService(IUserService userService, ICurrencyService currencyService, AppDbContext dbContext)
        {
            _userService = userService;
            _appDbContext = dbContext;
            _currencyService = currencyService;
        }

        private ICollection<AccountViewModel> GetChildrenAccountViewModels(Account account)
        {
            if (account.ChildAccounts == null)
            {
                return null;
            }
            var result = new List<AccountViewModel>();
            var accounts = _appDbContext.Account.Where(x => x.ParentAccountId == account.Id).Include(x => x.ChildAccounts);
            foreach (var childAccount in account.ChildAccounts)
            {
                var accountViewModel = new AccountViewModel
                {
                    Id = childAccount.Id,
                    AccountType = childAccount.Type,
                    Balance = childAccount.Balance,
                    CurrencyId = childAccount.CurrencyId,
                    Name = childAccount.Name,
                    ParentAccountId = (int)childAccount.ParentAccountId,
                };
                accountViewModel.Children = GetChildrenAccountViewModels(childAccount);
                result.Add(accountViewModel);
            }
            return result;
        }

        private async Task SeedAccounts(HttpContext httpContext, string userid, CancellationToken cancellationToken)
        {
            //var currencyViewModel = new CurrencyViewModel
            //{
            //    CurrencyCode = "156",
            //    CurrencyName = "CNY",
            //};
            //await _currencyService.CreateUserCurrency(httpContext, currencyViewModel, cancellationToken);
            //var currency = await _currencyService.GetUserCurrencies(httpContext, cancellationToken);

            var currency = new Currency()
            {
                AppUserId = userid,
                CurrencyCode = "156",
                CurrencyName = "CNY",
            };
            await _appDbContext.Currency.AddAsync(currency);


            var asset = new Account
            {
                AppUserId = userid,
                Balance = 0,
                Currency = currency,
                Name = "资产",
                Type = Shared.AccountType.ASSET,
            };
            var liability = new Account
            {
                AppUserId = userid,
                Balance = 0,
                Currency = currency,
                Name = "负债",
                Type = Shared.AccountType.LIABILITY,
            };
            var expense = new Account
            {
                AppUserId = userid,
                Balance = 0,
                Currency = currency,
                Name = "支出",
                Type = Shared.AccountType.EXPENSE,
            };
            var income = new Account
            {
                AppUserId = userid,
                Balance = 0,
                Currency = currency,
                Name = "收入",
                Type = Shared.AccountType.INCOME,
            };
            var equity = new Account
            {
                AppUserId = userid,
                Balance = 0,
                Currency = currency,
                Name = "权益",
                Type = Shared.AccountType.EQUITY,
            };
            var openbalance = new Account
            {
                AppUserId = userid,
                Balance = 0,
                Currency = currency,
                Name = "open balance",
                Type = Shared.AccountType.EQUITY,
                ParentAccount = equity,
            };

            await _appDbContext.Account.AddRangeAsync(asset, liability, equity, income, expense, openbalance);
            await _appDbContext.SaveChangesAsync();
        }

        public AccountViewModel GetUserAccount(HttpContext httpContext, int id, CancellationToken cancellationToken)
        {
            var userid = _userService.GetUserId(httpContext);
            var tmp = _appDbContext.Account.Where(t => t.AppUserId == userid && t.Id == id).Include(a => a.ChildAccounts).First();
            var accountViewModel = new AccountViewModel
            {
                Id = tmp.Id,
                AccountType = tmp.Type,
                Balance = tmp.Balance,
                CurrencyId = tmp.CurrencyId,
                Name = tmp.Name,
                ParentAccountId = tmp.ParentAccountId,
            };
            accountViewModel.Children = GetChildrenAccountViewModels(tmp);
            return accountViewModel;
        }

        public async Task<IEnumerable<AccountViewModel>> GetUserAccounts(HttpContext httpContext, CancellationToken cancellationToken)
        {
            var userid = _userService.GetUserId(httpContext);
            var accounts = await _appDbContext.Account.Where(t => t.AppUserId == userid && t.ParentAccount == null).Include(a => a.ChildAccounts).ToListAsync();
            if (accounts.Count == 0)
            {
                await SeedAccounts(httpContext, userid, cancellationToken);
                accounts = await _appDbContext.Account.Where(t => t.AppUserId == userid && t.ParentAccount == null).Include(a => a.ChildAccounts).ToListAsync();
            }
            var accountViewModelList = new List<AccountViewModel>();
            foreach (var item in accounts)
            {
                var accountViewModel = new AccountViewModel()
                {
                    Id = item.Id,
                    AccountType = item.Type,
                    Balance = item.Balance,
                    CurrencyId = item.CurrencyId,
                    Name = item.Name,
                    ParentAccountId = item.ParentAccountId,
                };
                accountViewModel.Children = GetChildrenAccountViewModels(item);
                accountViewModelList.Add(accountViewModel);
            }
            return accountViewModelList;
        }

        public async Task CreateUserAccount(HttpContext httpContext, AccountViewModel accountViewModel, CancellationToken cancellationToken)
        {
            var userid = _userService.GetUserId(httpContext);
            var account = new Account()
            {
                Id = accountViewModel.Id,
                Type = accountViewModel.AccountType,
                Balance = accountViewModel.Balance,
                CurrencyId = accountViewModel.CurrencyId,
                Name = accountViewModel.Name,
                ParentAccountId = accountViewModel.ParentAccountId,
                AppUserId = userid,
            };
            await _appDbContext.Account.AddAsync(account);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteUserAccount(HttpContext httpContext, int id, CancellationToken cancellationToken)
        {
            var userid = _userService.GetUserId(httpContext);
            var tmp = _appDbContext.Account.Where(t => t.AppUserId == userid && t.Id == id).First();

            _appDbContext.Account.Remove(tmp);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task UpdateUserAccount(HttpContext httpContext, AccountViewModel accountViewModel, CancellationToken cancellationToken)
        {
            var userid = _userService.GetUserId(httpContext);
            var tmp = _appDbContext.Account.Where(t => t.AppUserId == userid && t.Id == accountViewModel.Id).First();
            tmp.CurrencyId = accountViewModel.CurrencyId;
            tmp.ParentAccountId = accountViewModel.ParentAccountId;
            tmp.Balance = accountViewModel.Balance;
            tmp.Name = accountViewModel.Name;
            tmp.Type = accountViewModel.AccountType;
            _appDbContext.Account.Update(tmp);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
