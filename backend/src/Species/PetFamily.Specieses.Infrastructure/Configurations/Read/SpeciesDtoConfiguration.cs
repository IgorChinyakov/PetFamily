using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core.DTOs;
using PetFamily.Specieses.Contracts.DTOs;

namespace PetFamily.Specieses.Infrastructure.Configurations.Read
{
    public class SpeciesDtoConfiguration : IEntityTypeConfiguration<SpeciesDto>
    {
        public void Configure(EntityTypeBuilder<SpeciesDto> builder)
        {
            builder.ToTable("species");

            builder.HasKey(x => x.Id);
        }
    }
}
