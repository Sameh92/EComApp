using Ecom.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrasctructure.Data.Config
{
    internal class ProfileConfiguration : IEntityTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder.Property(x => x.Gender).IsRequired();
            builder.HasData(
        new Profile{Id  = Guid.NewGuid(),NameOfProfile="Sameh",AgeOFDOB=31,Gender="Male" },
        new Profile{Id = Guid.NewGuid(), NameOfProfile = "Rana", AgeOFDOB = 30, Gender="Female" });
        }
    }
}
