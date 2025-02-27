using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Pets.Value_objects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerContext.Entities;
using PetFamily.Domain.VolunteerContext.SharedVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Infrastructure.Configurations
{
    public class PetConfiguration : IEntityTypeConfiguration<Pet>
    {
        public void Configure(EntityTypeBuilder<Pet> builder)
        {
            builder.ToTable("pets");

            builder.HasKey(p => p.Id);

            builder.ComplexProperty(p => p.PetStatus, b =>
            {
                b.Property(ps => ps.Value)
                .IsRequired(true)
                .HasColumnName("status");
            });

            builder.Property(p => p.BreedId).HasConversion(
                id => id.Value,
                value => BreedId.Create(value).Value);


            builder.Property(p => p.SpeciesId).HasConversion(
                id => id.Value,
                value => SpeciesId.Create(value).Value);

            builder.ComplexProperty(p => p.Address, b =>
            {
                b.Property(a => a.Apartment)
                .IsRequired(true)
                .HasColumnName("apartment")
                .HasMaxLength(Address.MAX_LENGTH);

                b.Property(a => a.Street)
                .IsRequired(true)
                .HasColumnName("street")
                .HasMaxLength(Address.MAX_LENGTH);

                b.Property(a => a.City)
                .IsRequired(true)
                .HasColumnName("city")
                .HasMaxLength(Address.MAX_LENGTH);
            });

            builder.ComplexProperty(p => p.NickName, b =>
            {
                b.Property(a => a.Value)
                .IsRequired(true)
                .HasColumnName("nick_name")
                .HasMaxLength(NickName.MAX_LENGTH);
            });

            builder.ComplexProperty(p => p.Birthday, b =>
            {
                b.Property(a => a.Value)
                .IsRequired(true)
                .HasColumnName("birthday");
            });

            builder.ComplexProperty(p => p.Color, b =>
            {
                b.Property(a => a.Value)
                .IsRequired(true)
                .HasColumnName("color")
                .HasMaxLength(Constants.MAX_LOW_TITLE_LENGTH);
            });

            builder.ComplexProperty(v => v.Description, vb =>
            {
                vb.Property(e => e.Value)
                .IsRequired(true)
                .HasColumnName("description")
                .HasMaxLength(Description.MAX_LENGTH);
            });

            builder.ComplexProperty(v => v.Details, vb =>
            {
                vb.Property(d => d.Title)
                .IsRequired(true)
                .HasColumnName("title")
                .HasMaxLength(Constants.MAX_LOW_TITLE_LENGTH);

                vb.Property(d => d.Description)
               .IsRequired(true)
               .HasColumnName("description")
               .HasMaxLength(Constants.MAX_HIGH_TITLE_LENGTH);
            });

            builder.ComplexProperty(v => v.OwnerPhoneNumber, vb =>
            {
                vb.Property(e => e.Value)
                .IsRequired(true)
                .HasColumnName("phone_number")
                .HasMaxLength(Constants.MAX_LOW_TITLE_LENGTH);
            });

            builder.ComplexProperty(p => p.HealthInformation, b =>
            {
                b.Property(a => a.Value)
                .IsRequired(true)
                .HasColumnName("health_information")
                .HasMaxLength(HealthInformation.MAX_LENGTH);
            });

            builder.ComplexProperty(p => p.Height, b =>
            {
                b.Property(a => a.Value)
                .IsRequired(true)
                .HasColumnName("height");
            });

            builder.ComplexProperty(p => p.Weight, b =>
            {
                b.Property(a => a.Value)
                .IsRequired(true)
                .HasColumnName("weight");
            });

            builder.ComplexProperty(p => p.IsSterilized, b =>
            {
                b.Property(a => a.Value)
                .IsRequired(true)
                .HasColumnName("is_sterilized");
            });

            builder.ComplexProperty(p => p.IsVaccinated, b =>
            {
                b.Property(a => a.Value)
                .IsRequired(true)
                .HasColumnName("is_vaccinated");
            });

            builder.ComplexProperty(p => p.CreationDate, b =>
            {
                b.Property(a => a.Value)
                .IsRequired(true)
                .HasColumnName("creation_date");
            });
        }
    }
}
