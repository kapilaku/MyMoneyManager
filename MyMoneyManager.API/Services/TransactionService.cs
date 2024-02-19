using Microsoft.EntityFrameworkCore;
using MyMoneyManager.API.Interfaces.IServices;
using MyMoneyManager.API.Models;
using MyMoneyManager.Shared.ViewModels;

namespace MyMoneyManager.API.Services
{
    public class TransactionSerivce : ITransactionService
    {
        private readonly IUserService _userService;
        private readonly AppDbContext _appDbContext;
        public TransactionSerivce(IUserService userService, AppDbContext dbContext)
        {
            _appDbContext = dbContext;
            _userService = userService;
        }

        public TransactionViewModel GetUserTransaction(HttpContext httpContext, int id, CancellationToken cancellationToken)
        {
            var userid = _userService.GetUserId(httpContext);
            var tmp = _appDbContext.Transaction.Where(t => t.AppUserId == userid && t.Id == id).First();
            var transactionViewModel = new TransactionViewModel
            {
                Id = tmp.Id,
                Description = tmp.Description,
                Occured = tmp.Occured,
                TagId = tmp.TagId
            };
            return transactionViewModel;
        }

        public async Task<IEnumerable<TransactionViewModel>> GetUserTransactions(HttpContext httpContext, CancellationToken cancellationToken)
        {
            var userid = _userService.GetUserId(httpContext);
            var transactions = await _appDbContext.Transaction.Where(t => t.AppUserId == userid).ToListAsync();
            var transactionViewModelList = new List<TransactionViewModel>();
            foreach (var item in transactions)
            {
                transactionViewModelList.Add(new TransactionViewModel
                {
                    Id = item.Id,
                    Description = item.Description,
                    Occured = item.Occured,
                    TagId = item.TagId,
                });
            }
            return transactionViewModelList;
        }

        public async Task<int> CreateUserTransaction(HttpContext httpContext, TransactionViewModel transactionViewModel, CancellationToken cancellationToken)
        {
            var userid = _userService.GetUserId(httpContext);
            var transaction = new Transaction()
            {
                AppUserId = userid,
                Description = transactionViewModel.Description,
                Id = transactionViewModel.Id,
                Occured = transactionViewModel.Occured,
                TagId = transactionViewModel.TagId,
            };
            _appDbContext.Transaction.Add(transaction);
            await _appDbContext.SaveChangesAsync();

            return transaction.Id;
        }

        public async Task UpdateUserTransaction(HttpContext httpContext, TransactionViewModel transactionViewModel, CancellationToken cancellationToken)
        {
            var userid = _userService.GetUserId(httpContext);
            var tmp = _appDbContext.Transaction.Where(t => t.AppUserId == userid && t.Id == transactionViewModel.Id).First();
            tmp.Description = transactionViewModel.Description;
            tmp.Occured = transactionViewModel.Occured;
            tmp.TagId = transactionViewModel.TagId;
            _appDbContext.Transaction.Update(tmp);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteUserTransaction(HttpContext httpContext, int id, CancellationToken cancellationToken)
        {
            var userid = _userService.GetUserId(httpContext);
            var tmp = _appDbContext.Transaction.Where(t => t.AppUserId == userid && t.Id == id).First();

            _appDbContext.Transaction.Remove(tmp);
            await _appDbContext.SaveChangesAsync();
        }

    }
}
