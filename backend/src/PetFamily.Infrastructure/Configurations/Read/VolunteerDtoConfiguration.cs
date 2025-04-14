using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Application.Volunteers.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Infrastructure.Configurations.Read
{
    public class VolunteerDtoConfiguration : IEntityTypeConfiguration<VolunteerDto>
    {
        public void Configure(EntityTypeBuilder<VolunteerDto> builder)
        {
            builder.ToTable("volunteers");

            builder.HasKey(p => p.Id);
        }
    }
}
