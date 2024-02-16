
using EksiSozluk.Infrastructure.Persistence.Extensions;
using EksiSozluk.Api.Application.Extensions;
using FluentValidation.AspNetCore;
using EksiSozluk.Api.WebApi.Infrastructure.Extensions;
using FluentValidation;
using EksiSozluk.Common.Infastructure.Results;

namespace EksiSozluk.Api.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers()
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.PropertyNamingPolicy = null;
                });

            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddFluentValidationClientsideAdapters();
            builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

            //  .AddFluentValidation();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.ConfirugeAuth(builder.Configuration);

            builder.Services.AddApplicationRegistration();
            builder.Services.AddInfrastructureRegistration(builder.Configuration); //db configs injection

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

           app.ConfigureExceptionHandling(includeExceptionDetails: app.Environment.IsDevelopment());

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
