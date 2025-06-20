using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Specieses.Domain.Entities;
using PetFamily.Specieses.Domain.ValueObjects;

namespace PetFamily.Specieses.Infrastructure.Configurations.Write
{
    public class BreedConfiguration : IEntityTypeConfiguration<Breed>
    {
        public void Configure(EntityTypeBuilder<Breed> builder)
        {
            builder.ToTable("breeds");

            builder.HasKey(x => x.Id).HasName("pk_breeds");

            builder.Property(s => s.Id)
                .HasColumnName("breed_id");

            builder.ComplexProperty(s => s.Name, b =>
            {
                b.Property(n => n.Value)
                .IsRequired()
                .HasColumnName("name")
                .HasMaxLength(Name.MAX_LENGTH);
            });

            builder.Property(v => v.IsDeleted)
               .HasColumnName("is_deleted");

            builder.Property(v => v.DeletionDate)
                .IsRequired(false)
                .HasDefaultValue(null)
                .HasColumnName("deletion_date");
        }
    }
}
