using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Shared;
using PetFamily.Domain.SpeciesContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Infrastructure.Configurations
{
    public class BreedConfiguration : IEntityTypeConfiguration<Breed>
    {
        public void Configure(EntityTypeBuilder<Breed> builder)
        {
            builder.ToTable("breeds");

            builder.HasKey(x => x.Id);

            builder.ComplexProperty(s => s.Name, b =>
            {
                b.Property(n => n.Value)
                .IsRequired()
                .HasColumnName("name")
                .HasMaxLength(Constants.MAX_LOW_TITLE_LENGTH);
            });
        }
    }
}
