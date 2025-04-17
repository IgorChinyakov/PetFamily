using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerContext.Entities;
using PetFamily.Domain.VolunteerContext.SharedVO;
using PetFamily.Domain.VolunteerContext.VolunteerVO;
using PetFamily.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PetFamily.Infrastructure.Configurations.Write
{
    public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
    {
        public void Configure(EntityTypeBuilder<Volunteer> builder)
        {
            builder.ToTable("volunteers");

            builder.HasKey(v => v.Id);

            builder.HasMany(v => v.Pets)
                .WithOne()
                .HasForeignKey("volunteer_id")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.Property(v => v.SocialMediaList)
                .ValueObjectCollectionJsonConversion(
                    socialMedia => socialMedia,
                    json => json)
                .HasColumnName("social_media");

            builder.Property(v => v.DetailsList)
                .ValueObjectCollectionJsonConversion(
                    details => details,
                    json => json)
                .HasColumnName("details");

            builder.ComplexProperty(v => v.FullName, vb =>
            {
                vb.Property(fn => fn.Name)
                .IsRequired(true)
                .HasColumnName("name")
                .HasMaxLength(FullName.MAX_LENGTH);

                vb.Property(fn => fn.SecondName)
                .IsRequired(false)
                .HasColumnName("second_name")
                .HasMaxLength(FullName.MAX_LENGTH);

                vb.Property(fn => fn.FamilyName)
                .IsRequired(true)
                .HasColumnName("family_name")
                .HasMaxLength(FullName.MAX_LENGTH);
            });

            builder.ComplexProperty(v => v.Experience, vb =>
            {
                vb.Property(e => e.Value)
                .IsRequired(true)
                .HasColumnName("experience");
            });

            builder.ComplexProperty(v => v.Email, vb =>
            {
                vb.Property(e => e.Value)
                .IsRequired(true)
                .HasColumnName("email")
                .HasMaxLength(Domain.Shared.Constants.MAX_LOW_TITLE_LENGTH);
            });

            builder.ComplexProperty(v => v.Description, vb =>
            {
                vb.Property(e => e.Value)
                .IsRequired(true)
                .HasColumnName("description")
                .HasMaxLength(Description.MAX_LENGTH);
            });

            builder.ComplexProperty(v => v.PhoneNumber, vb =>
            {
                vb.Property(e => e.Value)
                .IsRequired(true)
                .HasColumnName("phone_number")
                .HasMaxLength(Domain.Shared.Constants.MAX_LOW_TITLE_LENGTH);
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
