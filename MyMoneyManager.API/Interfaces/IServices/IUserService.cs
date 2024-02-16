namespace MyMoneyManager.API.Interfaces.IServices;

public interface IUserService
{
    public string GetUserId(HttpContext httpContext);
}
