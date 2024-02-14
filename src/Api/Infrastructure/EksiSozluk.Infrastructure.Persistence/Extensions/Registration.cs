using EksiSozluk.Api.Application.Interfaces.Repositories;
using EksiSozluk.Infrastructure.Persistence.Context;
using EksiSozluk.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;




namespace EksiSozluk.Infrastructure.Persistence.Extensions
{
    public static class Registration
    {
        public static IServiceCollection AddInfrastructureRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(conf =>
            {
                string connString = configuration["ConnectionString"].ToString();
                conf.UseSqlServer(connString, opt =>
                {
                    opt.EnableRetryOnFailure();
                });
            });

            var dataSeed = new DataSeed();
            dataSeed.SeedAsync(configuration).GetAwaiter().GetResult();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEmailConfirmationRepository, EmailConfirmationRepository>();
            services.AddScoped<IEntryRepository, EntryRepository>();
            services.AddScoped<IEntryCommentRepository, EntryCommentRepository>();

            return services;
        }
    }
}
