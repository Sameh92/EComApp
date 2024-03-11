using Ecom.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Service
{
    public interface ITokenService
    {
        string CreateToken(AppUser appUser);
    }
}
