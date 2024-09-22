using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using RetroCassetteVHS.Domain.Interface;
using RetroCassetteVHS.Infrastructure.Repository;

namespace RetroCassette.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<ICassetteRepository, CassetteRepository>();
            services.AddTransient<IWalletRepository, WalletRepository>();
            services.AddTransient<IRentalRepository, RentalRepository>();
            return services;
        }
    }
}
