using MyMoneyManager.Shared.ViewModels;

namespace MyMoneyManager.API.Interfaces.IServices
{
    public interface ISplitService
    {
        public Task<IEnumerable<SplitViewModel>> GetUserSplitsByAccountId(HttpContext httpContext, int accountId, CancellationToken cancellationToken);
        public Task<IEnumerable<SplitViewModel>> GetUserSplits(HttpContext httpContext, CancellationToken cancellationToken);
        public SplitViewModel GetUserSplit(HttpContext httpContext, int id, CancellationToken cancellationToken);
        public Task DeleteUserSplit(HttpContext httpContext, int id, CancellationToken cancellationToken);
        public Task CreateUserSplit(HttpContext httpContext, SplitViewModel splitViewModel, CancellationToken cancellationToken);
        public Task UpdateUserSplit(HttpContext httpContext, SplitViewModel splitViewModel, CancellationToken cancellationToken);
    }
}
