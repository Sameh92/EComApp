using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Ecom.Infrasctructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrasctructure.Repositories
{
    public class ProfileRepository:GenericRepositoryForGuid<Profile>,IProfileRepository
    {
        public ProfileRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
