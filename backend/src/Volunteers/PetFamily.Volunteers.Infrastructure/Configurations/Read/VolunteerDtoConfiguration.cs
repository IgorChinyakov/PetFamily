using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core.DTOs;
using PetFamily.Volunteers.Contracts.DTOs;
using System.Text.Json;

namespace PetFamily.Volunteers.Infrastructure.Configurations.Read
{
    public class VolunteerDtoConfiguration : IEntityTypeConfiguration<VolunteerDto>
    {
        public void Configure(EntityTypeBuilder<VolunteerDto> builder)
        {
            builder.ToTable("volunteers");

            builder.HasKey(v => v.Id);

            builder.ComplexProperty(v => v.FullName, b =>
            {
                b.Property(fn => fn.Name)
                    .HasColumnName("name");

                b.Property(fn => fn.SecondName)
                    .HasColumnName("second_name");

                b.Property(fn => fn.FamilyName)
                    .HasColumnName("family_name");
            });

            //builder.Property(i => i.Details)
            // .HasConversion(
            //     details => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
            //     json => JsonSerializer.Deserialize<DetailsDto[]>(json, JsonSerializerOptions.Default)!);

            //builder.Property(i => i.SocialMedia)
            // .HasConversion(
            //     socialMedia => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
            //     json => JsonSerializer.Deserialize<SocialMediaDto[]>(json, JsonSerializerOptions.Default)!);
        }
    }
}
