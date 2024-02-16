using Microsoft.EntityFrameworkCore;
using MyMoneyManager.API.Interfaces.IServices;
using MyMoneyManager.API.Models;
using MyMoneyManager.Shared.ViewModels;

namespace MyMoneyManager.API.Services
{
    public class SplitService : ISplitService
    {
        private readonly IUserService _userService;
        private readonly AppDbContext _appDbContext;
        public SplitService(IUserService userService, AppDbContext dbContext)
        {
            _appDbContext = dbContext;
            _userService = userService;
        }

        public SplitViewModel GetUserSplit(HttpContext httpContext, int id, CancellationToken cancellationToken)
        {
            var userid = _userService.GetUserId(httpContext);
            var tmp = _appDbContext.Split.Where(t => t.AppUserId == userid && t.Id == id).First();
            var splitViewModel = new SplitViewModel
            {
                Id = tmp.Id,
                AccountId = tmp.AccountId,
                Balance = tmp.Balance,
                CurrencyId = tmp.CurrencyId,
            };
            return splitViewModel;
        }

        public async Task<IEnumerable<SplitViewModel>> GetUserSplits(HttpContext httpContext, CancellationToken cancellationToken)
        {
            var userid = _userService.GetUserId(httpContext);
            var splits = await _appDbContext.Split.Where(t => t.AppUserId == userid).ToListAsync();
            var splitViewModelList = new List<SplitViewModel>();
            foreach (var item in splits)
            {
                splitViewModelList.Add(new SplitViewModel
                {
                    Id = item.Id,
                    AccountId = item.AccountId,
                    Balance = item.Balance,
                    CurrencyId = item.CurrencyId,
                });
            }
            return splitViewModelList;
        }

        public async Task CreateUserSplit(HttpContext httpContext, SplitViewModel splitViewModel, CancellationToken cancellationToken)
        {
            var userid = _userService.GetUserId(httpContext);
            var split = new Split()
            {
                AppUserId = userid,
                Id = splitViewModel.Id,
                CurrencyId = splitViewModel.CurrencyId,
                AccountId = splitViewModel.AccountId,
                Balance = splitViewModel.Balance,
                TransactionId = splitViewModel.TransactionId,
            };
            await _appDbContext.Split.AddAsync(split);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task UpdateUserSplit(HttpContext httpContext, SplitViewModel splitViewModel, CancellationToken cancellationToken)
        {
            var userid = _userService.GetUserId(httpContext);
            var tmp = _appDbContext.Split.Where(t => t.AppUserId == userid && t.Id == splitViewModel.Id).First();
            tmp.CurrencyId = splitViewModel.CurrencyId;
            tmp.AccountId = splitViewModel.AccountId;
            tmp.Balance = splitViewModel.Balance;
            tmp.TransactionId = splitViewModel.TransactionId;
            _appDbContext.Split.Update(tmp);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteUserSplit(HttpContext httpContext, int id, CancellationToken cancellationToken)
        {
            var userid = _userService.GetUserId(httpContext);
            var tmp = _appDbContext.Split.Where(t => t.AppUserId == userid && t.Id == id).First();

            _appDbContext.Split.Remove(tmp);
            await _appDbContext.SaveChangesAsync();
        }

    }
}
