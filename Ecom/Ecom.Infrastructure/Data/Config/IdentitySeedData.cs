using Ecom.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Data.Config
{
    public class IdentitySeedData
    {
        public static async Task SeedUserDataAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                //create new user
                var user = new AppUser
                {
                    DisplayName = "Sameh",
                    Email = "Sameh@Rana.com",
                    UserName = "Sameh@Rana.com",
                    Address = new Address
                    {
                        FirstName = "Sameh",
                        LastName = "Rana",
                        City = "Damasucs",
                        State = "Draa",
                        Street = "jalan ceylon",
                        ZipCode = "50200"

                    }
                };
                await userManager.CreateAsync(user, "P@$$w0rd");
            }
        }
    }
}
