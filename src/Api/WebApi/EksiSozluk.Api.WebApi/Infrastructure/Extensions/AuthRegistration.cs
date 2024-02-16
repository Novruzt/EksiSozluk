using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RestSharp.Authenticators;
using System.Text;

namespace EksiSozluk.Api.WebApi.Infrastructure.Extensions;

public static class AuthRegistration
{
    public static IServiceCollection ConfirugeAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var singingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["AuthConfig:Secret"]));

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                IssuerSigningKey=singingKey,
            };
        });

        return services;
    }
}
