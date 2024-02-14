using AutoMapper;
using EksiSozluk.Api.Application.Features.Commands.User;
using EksiSozluk.Common.Models.RequestModels;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Api.Application.Extensions
{
    public static class Registration
    {
        public static IServiceCollection AddApplicationRegistration(this IServiceCollection services)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(LoginUserCommandHandler).Assembly));
            services.AddAutoMapper(assembly);
            services.AddValidatorsFromAssembly(assembly);

            return services;
        }
    }
}
