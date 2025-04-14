using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Application.Pets.DTOs;
using PetFamily.Application.Volunteers.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PetFamily.Infrastructure.Configurations.Read
{
    public class PetDtoConfiguration : IEntityTypeConfiguration<PetDto>
    {
        public void Configure(EntityTypeBuilder<PetDto> builder)
        {
            builder.ToTable("issues");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Files)
                .HasConversion(
                     files => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                     json => JsonSerializer.Deserialize<PetFileDto[]>(json, JsonSerializerOptions.Default)! );
        }
    }
}
