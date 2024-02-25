using Blazored.LocalStorage;
using EksiSozluk.WebApp.Infastructure.Services;
using EksiSozluk.WebApp.Infastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace EksiSozluk.WebApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            string url = builder.Configuration["profiles:https:applicationUrl"];
            builder.Services.AddHttpClient("WebApiClient", client =>
            {
                client.BaseAddress = new Uri(url);
            });

            //AuthTokenHandler goes here.

            builder.Services.AddScoped(sp =>
            {
                IHttpClientFactory clientFactory = sp.GetRequiredService<IHttpClientFactory>();

                return clientFactory.CreateClient("WebApiClient");
            });

            builder.Services.AddTransient<IVoteService, VoteService>();
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<IEntryService, EntryService>();
            builder.Services.AddTransient<IFavService, FavService>();
            builder.Services.AddTransient<IIdentityService, IdentityService>();

            builder.Services.AddBlazoredLocalStorage();

            await builder.Build().RunAsync();
        }
    }
}
