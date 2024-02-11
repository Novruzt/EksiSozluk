﻿using EksiSozluk.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;



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

            return services;
        }
    }
}