using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Ecom.Core.Service;
using Ecom.Infrastructure.Data;
using Ecom.Infrastructure.Data.Config;
using Ecom.Infrastructure.Repositories;
using Ecom.Infrastructure.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure
{
    public static class InfrasturctureRegistration
    {
        public static IServiceCollection InfrastructureConfiguration(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            // after apply unit of wrok we have to register any repo with Irepo like below
            // and also we have to inject the repos in the constructor of the controler 
            //services.AddScoped<ICategoryRepository, CategoryRepository>();
            //services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Configurer DB
            services.AddDbContext<ApplicationDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            //Configure Identity
            services.AddIdentity<AppUser, IdentityRole>(opt =>
            {
               // opt.Password.RequireNonAlphanumeric=false;
               // opt.Password.RequiredLength = 0;
            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            services.AddMemoryCache();
            services.AddAuthentication(opt => { 
            opt.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme;              
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }
            
            ).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:Key"])),
                    ValidIssuer = configuration["Token:Issuer"],
                    ValidateIssuer = true,
                    ValidateAudience = false

                };
            });

            // Configure Token Service
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IOrderServices, OrderServices>(); 
            

            return services;

        }

        public static async void InfrastructureConfigMiddleware(this IApplicationBuilder app)
        {
            using(var scope=app.ApplicationServices.CreateScope())
            {
                var usingManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                await IdentitySeedData.SeedUserDataAsync(usingManager);
            }
        }
    }
}
