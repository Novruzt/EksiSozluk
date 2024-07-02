using Blazored.LocalStorage;
using EksiSozluk.WebApp.Infastructure.Extensions;

namespace EksiSozluk.WebApp.Infastructure.Auth;

public class AuthTokenHandler : DelegatingHandler
{
    private readonly ISyncLocalStorageService syncLocalStorageService;

    public AuthTokenHandler(ISyncLocalStorageService syncLocalStorageService)
    {
        this.syncLocalStorageService = syncLocalStorageService;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        string token = syncLocalStorageService.GetToken();

        if (!string.IsNullOrEmpty(token) && (request.Headers.Authorization is null || string.IsNullOrEmpty(request.Headers.Authorization.Parameter)))
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token);

        return base.SendAsync(request, cancellationToken);
    }
}
