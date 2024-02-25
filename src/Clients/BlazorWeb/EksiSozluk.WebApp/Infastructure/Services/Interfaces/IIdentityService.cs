using EksiSozluk.Common.Models.RequestModels.UserCommands;

namespace EksiSozluk.WebApp.Infastructure.Services.Interfaces;
public interface IIdentityService
{
    bool IsLoggedIn { get; }

    Guid GetUserId();
    string GetUserName();
    string GetUserToken();
    Task<bool> Login(LoginUserCommand command);
    void Logout();
}