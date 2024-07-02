using Blazored.LocalStorage;
using EksiSozluk.WebApp.Infastructure.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EksiSozluk.WebApp.Infastructure.Auth;

public class AuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService localStorageService;
    private readonly AuthenticationState anonymous;

    public AuthStateProvider(ILocalStorageService localStorageService)
    {
        this.localStorageService = localStorageService;
        this.anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {

        var apiToken = await localStorageService.GetToken();

        if (string.IsNullOrEmpty(apiToken))
            return anonymous;

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        JwtSecurityToken securityToken = tokenHandler.ReadJwtToken(apiToken);

        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(securityToken.Claims, "jwtAuthType"));

        return new AuthenticationState(claimsPrincipal);
    }


    public void NotifyUserLogin(string userName, Guid userId)
    {
        ClaimsPrincipal cp = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, userName),
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        }, "jwtAuthType"));

        Task<AuthenticationState> authState = Task.FromResult(new AuthenticationState(cp));

        NotifyAuthenticationStateChanged(authState);
    }

    public void NotifyUserLogout()
    {
        Task<AuthenticationState> authState = Task.FromResult(anonymous);

        NotifyAuthenticationStateChanged(authState);
    }
}
