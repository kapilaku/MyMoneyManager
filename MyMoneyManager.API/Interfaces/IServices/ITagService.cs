using MyMoneyManager.API.Models;
using MyMoneyManager.Shared.ViewModels;

namespace MyMoneyManager.API.Interfaces.IServices;

public interface ITagService
{
    public Task<IEnumerable<TagViewModel>> GetUserTags(HttpContext httpContext, CancellationToken cancellationToken);
    public TagViewModel GetUserTag(HttpContext httpContext, int id, CancellationToken cancellationToken);
    public Task DeleteUserTag(HttpContext httpContext, int id, CancellationToken cancellationToken);
    public Task CreateUserTag(HttpContext httpContext,  TagViewModel tagViewModel, CancellationToken cancellationToken);
    public Task UpdateUserTag(HttpContext httpContext, TagViewModel tagViewModel, CancellationToken cancellationToken);
}
