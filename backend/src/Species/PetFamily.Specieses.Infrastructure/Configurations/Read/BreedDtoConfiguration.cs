using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core.DTOs;
using PetFamily.Specieses.Contracts.DTOs;

namespace PetFamily.Specieses.Infrastructure.Configurations.Read
{
    public class BreedDtoConfiguration : IEntityTypeConfiguration<BreedDto>
    {
        public void Configure(EntityTypeBuilder<BreedDto> builder)
        {
            builder.ToTable("breeds");

            builder.HasKey(x => x.Id).HasName("pk_breeds");

            builder.Property(s => s.Id)
                .HasColumnName("breed_id");
        }
    }
}
