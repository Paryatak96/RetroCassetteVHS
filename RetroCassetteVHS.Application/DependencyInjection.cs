using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using RetroCassetteVHS.Application.Interfaces;
using RetroCassetteVHS.Application.Services;

namespace RetroCassetteVHS.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddTransient<ICassetteService, CassetteService>();
            services.AddTransient<IRentalService, RentalService>();
            services.AddTransient<IWalletService, WalletService>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
