using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MyMoneyManager.API.Interfaces.IServices;
using MyMoneyManager.API.Models;
using MyMoneyManager.Shared.ViewModels;
using System.Linq.Expressions;


namespace MyMoneyManager.API.Services;

public class TagService : ITagService
{
    private readonly IUserService _userService;
    private readonly AppDbContext _appDbContext;
    public TagService(IUserService userService, AppDbContext dbContext)
    {
        _userService = userService;
        _appDbContext = dbContext;
    }

    public TagViewModel GetUserTag(HttpContext httpContext, int id, CancellationToken cancellationToken)
    {
        var userid = _userService.GetUserId(httpContext);
        var tmp = _appDbContext.Tag.Where(t => t.AppUserId == userid && t.Id == id).First();
        var tagViewModel = new TagViewModel { Id = tmp.Id, TagName = tmp.TagName };
        return tagViewModel;
    }

    public async Task<IEnumerable<TagViewModel>> GetUserTags(HttpContext httpContext, CancellationToken cancellationToken)
    {
        var userid = _userService.GetUserId(httpContext);
        var tags = await _appDbContext.Tag.Where(t => t.AppUserId == userid).ToListAsync();
        var tagViewModelList = new List<TagViewModel>();
        foreach (var item in tags)
        {
            tagViewModelList.Add(new TagViewModel { Id = item.Id, TagName = item.TagName });
        }
        return tagViewModelList;
    }

    public async Task CreateUserTag(HttpContext httpContext, TagViewModel tagViewModel, CancellationToken cancellationToken)
    {
        var userid = _userService.GetUserId(httpContext);
        var tag = new Tag() { AppUserId = userid, TagName = tagViewModel.TagName };
        await _appDbContext.Tag.AddAsync(tag);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task DeleteUserTag(HttpContext httpContext, int id, CancellationToken cancellationToken)
    {
        var userid = _userService.GetUserId(httpContext);
        var tmp = _appDbContext.Tag.Where(t => t.AppUserId == userid && t.Id == id).First();

        _appDbContext.Tag.Remove(tmp);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task UpdateUserTag(HttpContext httpContext, TagViewModel tagViewModel, CancellationToken cancellationToken)
    {
        var userid = _userService.GetUserId(httpContext);
        var tmp = _appDbContext.Tag.Where(t => t.AppUserId == userid && t.Id == tagViewModel.Id).First();
        tmp.TagName = tagViewModel.TagName;
        _appDbContext.Tag.Update(tmp);
        await _appDbContext.SaveChangesAsync();
    }
}
