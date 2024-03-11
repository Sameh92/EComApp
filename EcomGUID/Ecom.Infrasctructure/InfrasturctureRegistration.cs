using Ecom.Core.Interfaces;
using Ecom.Infrasctructure.Data;
using Ecom.Infrasctructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrasctructure
{
    public static class InfrasturctureRegistration
    {
        public static IServiceCollection InfrastructureConfiguration(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddScoped(typeof(IGenereicRepository<>), typeof(GenercRepository<>));
            services.AddScoped(typeof(IGenericRepositoryForGuidId<>), typeof(GenericRepositoryForGuid<>));
            services.AddScoped(typeof(IGenericRepositoryForIntegerId<>), typeof(GenericRepositoryForInteger<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddDbContext<ApplicationDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            return services;
        }
    }
}
