using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using System.Security.Claims;

namespace MyMoneyManager.Web
{
    public class MyAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;
        public MyAuthenticationStateProvider(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorageService.GetItemAsStringAsync("accessToken");

            if (string.IsNullOrEmpty(token))
            {
                return new AuthenticationState(new ClaimsPrincipal());
            }

            var claims = new List<Claim>()
            {
               new Claim(ClaimTypes.NameIdentifier, token),
               new Claim(ClaimTypes.Email, token)
            };

            var identity = new ClaimsIdentity(claims);

            var principal = new ClaimsPrincipal(identity);
            return new AuthenticationState(principal);
        }
    }
}
