using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Specieses.Domain.Entities;
using PetFamily.Specieses.Domain.ValueObjects;

namespace PetFamily.Specieses.Infrastructure.Configurations.Write
{
    public class SpeciesConfiguration : IEntityTypeConfiguration<Species>
    {
        public void Configure(EntityTypeBuilder<Species> builder)
        {
            builder.ToTable("species");

            builder.HasKey(s => s.Id);

            builder.HasMany(s => s.Breeds)
                .WithOne()
                .HasForeignKey("species_id")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

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
