using EksiSozluk.Common.Models.Queries;

namespace EksiSozluk.WebApp.Infrastructure.Services.Interfaces;
public interface IUserService
{
    Task<bool> ChangeUserPassword(string oldPassword, string newPassword);
    Task<UserDetailsViewModel> GetUserDetail(Guid? id);
    Task<UserDetailsViewModel> GetUserDetail(string userName);
    Task<bool> UpdateUser(UserDetailsViewModel user);
}