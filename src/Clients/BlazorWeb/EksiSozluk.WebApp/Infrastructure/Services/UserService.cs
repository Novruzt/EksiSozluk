using EksiSozluk.Common.Infrastructure.Exceptions;
using EksiSozluk.Common.Infrastructure.Results;
using EksiSozluk.Common.Models.Queries;
using EksiSozluk.Common.Models.RequestModels.UserCommands;
using EksiSozluk.WebApp.Infrastructure.Services.Interfaces;
using System.Net.Http.Json;
using System.Text.Json;

namespace EksiSozluk.WebApp.Infrastructure.Services;
public class UserService : IUserService
{
    private readonly HttpClient client;

    public UserService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<UserDetailsViewModel> GetUserDetail(Guid? id)
    {
        UserDetailsViewModel? userDetail = await client.GetFromJsonAsync<UserDetailsViewModel>($"/api/user/{id}");

        return userDetail;
    }

    public async Task<UserDetailsViewModel> GetUserDetail(string userName)
    {
        UserDetailsViewModel? userDetail = await client.GetFromJsonAsync<UserDetailsViewModel>($"/api/user/username/{userName}");

        return userDetail;
    }

    public async Task<bool> UpdateUser(UserDetailsViewModel user)
    {
        HttpResponseMessage res = await client.PostAsJsonAsync($"/api/user/update", user);

        return res.IsSuccessStatusCode;
    }

    public async Task<bool> ChangeUserPassword(string oldPassword, string newPassword)
    {
        ChangeUserPasswordCommand command = new ChangeUserPasswordCommand(null, oldPassword, newPassword);
        HttpResponseMessage? httpResponse = await client.PostAsJsonAsync($"/api/User/ChangePassword", command);

        if (httpResponse != null && !httpResponse.IsSuccessStatusCode)
        {
            if (httpResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                string responseStr = await httpResponse.Content.ReadAsStringAsync();
                ValidationResponseModel? validation = JsonSerializer.Deserialize<ValidationResponseModel>(responseStr, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                responseStr = validation.FlattenErrors;
                throw new DatabaseValidationException(responseStr);
            }

            return false;
        }

        return httpResponse.IsSuccessStatusCode;
    }
}
