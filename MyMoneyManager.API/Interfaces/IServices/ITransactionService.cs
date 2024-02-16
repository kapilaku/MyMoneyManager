using MyMoneyManager.Shared.ViewModels;

namespace MyMoneyManager.API.Interfaces.IServices
{
    public interface ITransactionService
    {
        public Task<IEnumerable<TransactionViewModel>> GetUserTransactions(HttpContext httpContext, CancellationToken cancellationToken);
        public TransactionViewModel GetUserTransaction(HttpContext httpContext, int id, CancellationToken cancellationToken);
        public Task DeleteUserTransaction(HttpContext httpContext, int id, CancellationToken cancellationToken);
        public Task CreateUserTransaction(HttpContext httpContext, TransactionViewModel transactionViewModel, CancellationToken cancellationToken);
        public Task UpdateUserTransaction(HttpContext httpContext, TransactionViewModel transactionViewModel, CancellationToken cancellationToken);
    }
}
