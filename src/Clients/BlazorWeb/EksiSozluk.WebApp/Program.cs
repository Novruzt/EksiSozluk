using Blazored.LocalStorage;
using EksiSozluk.WebApp.Infrastructure.Auth;
using EksiSozluk.WebApp.Infrastructure.Services;
using EksiSozluk.WebApp.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
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

            builder.Services.AddHttpClient("WebApiClient", client =>
            {
                client.BaseAddress = new Uri("http://localhost:5108");
            }).AddHttpMessageHandler<AuthTokenHandler>();

           

            builder.Services.AddScoped(sp =>
            {
                IHttpClientFactory clientFactory = sp.GetRequiredService<IHttpClientFactory>();

                return clientFactory.CreateClient("WebApiClient");
            });

            builder.Services.Scan(scan =>
            {
                scan.FromAssembliesOf(typeof(VoteService))
                .AddClasses()
                .AsMatchingInterface()
                .WithTransientLifetime();
            });

            builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
            builder.Services.AddScoped<AuthTokenHandler>();

            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddAuthorizationCore();

            await builder.Build().RunAsync();
        }
    }
}
