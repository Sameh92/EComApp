using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Entities
{
    public  class AppUser :IdentityUser
    {
        public string DisplayName { get;set; }
        [NotMapped]
        public int Age { get; set; } = 33;
        public Address Address { get;set; }
    }
}
