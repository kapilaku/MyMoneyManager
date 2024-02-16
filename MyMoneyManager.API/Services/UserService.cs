using Microsoft.AspNetCore.Identity;
using MyMoneyManager.API.Interfaces.IServices;
using System.Security.Claims;

namespace MyMoneyManager.API.Services;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    public UserService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }
    public string GetUserId(HttpContext httpContext)
    {
        var user = httpContext.User;
        var x = _userManager.GetUserId(user);
        return x;
    }
}
