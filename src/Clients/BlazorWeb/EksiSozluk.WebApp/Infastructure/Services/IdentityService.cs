using Blazored.LocalStorage;
using EksiSozluk.Common.Infastructure.Exceptions;
using EksiSozluk.Common.Infastructure.Results;
using EksiSozluk.Common.Models.Queries;
using EksiSozluk.Common.Models.RequestModels.UserCommands;
using EksiSozluk.WebApp.Infastructure.Extensions;
using EksiSozluk.WebApp.Infastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Text.Json;

namespace EksiSozluk.WebApp.Infastructure.Services;

public class IdentityService : IIdentityService
{
    private JsonSerializerOptions defaultJsonOpt => new JsonSerializerOptions()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient httpClient;
    private readonly ISyncLocalStorageService syncLocalStorageService;
    private readonly AuthenticationStateProvider authenticationStateProvider;


    public IdentityService(HttpClient httpClient, ISyncLocalStorageService syncLocalStorageService, AuthenticationStateProvider authenticationStateProvider)
    {
        this.httpClient = httpClient;
        this.syncLocalStorageService = syncLocalStorageService;
        this.authenticationStateProvider = authenticationStateProvider;
    }


    public bool IsLoggedIn => !string.IsNullOrEmpty(GetUserToken());

    public string GetUserToken()
    {
        return syncLocalStorageService.GetToken();
    }

    public string GetUserName()
    {
        return syncLocalStorageService.GetToken();
    }

    public Guid GetUserId()
    {
        return syncLocalStorageService.GetUserId();
    }

    public async Task<bool> Login(LoginUserCommand command)
    {
        string responseStr;
        HttpResponseMessage? httpResponse = await httpClient.PostAsJsonAsync("/api/User/Login", command);

        if (httpResponse != null && !httpResponse.IsSuccessStatusCode)
        {
            if (httpResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                responseStr = await httpResponse.Content.ReadAsStringAsync();
                ValidationResponseModel? validation = JsonSerializer.Deserialize<ValidationResponseModel>(responseStr, defaultJsonOpt);
                responseStr = validation.FlattenErrors;
                throw new DatabaseValidationException(responseStr);
            }

            return false;
        }


        responseStr = await httpResponse.Content.ReadAsStringAsync();
        LoginUserViewModel? response = JsonSerializer.Deserialize<LoginUserViewModel>(responseStr);

        if (!string.IsNullOrEmpty(response.Token)) // login success
        {
            syncLocalStorageService.SetToken(response.Token);
            syncLocalStorageService.SetUsername(response.UserName);
            syncLocalStorageService.SetUserId(response.Id);

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", response.Token);

            return true;
        }

        return false;
    }

    public void Logout()
    {
        syncLocalStorageService.RemoveItem(LocalStorageExtension.TokenName);
        syncLocalStorageService.RemoveItem(LocalStorageExtension.UserName);
        syncLocalStorageService.RemoveItem(LocalStorageExtension.UserId);

        httpClient.DefaultRequestHeaders.Authorization = null;
    }
}
