using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Entities
{
    public class Profile :BaseEntity<Guid>
    {
        public string NameOfProfile { get; set; }
        public int AgeOFDOB { get; set; }
        public string Gender { get; set; }
    }
}
