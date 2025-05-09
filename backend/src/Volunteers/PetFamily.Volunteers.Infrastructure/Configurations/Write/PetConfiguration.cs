﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Domain.Entities;
using PetFamily.Volunteers.Domain.PetsVO;
using PetFamily.Volunteers.Domain.SharedVO;
using PetFamily.Core.Extensions;
using PetFamily.Core.DTOs;
using PetFamily.Volunteers.Contracts.DTOs;

namespace PetFamily.Volunteers.Infrastructure.Configurations.Write
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

            builder.ComplexProperty(m => m.MainPhoto, mb =>
            {
                mb.Property(p => p.Path)
                    .IsRequired(false)
                    .HasDefaultValue(string.Empty)
                    .HasColumnName("main_photo");
            });

            //builder.ComplexProperty(p => p.MainPhoto, b =>
            //{
            //    b.ComplexProperty(m => m.PathToStorage, mb =>
            //    {
            //        mb.Property(p => p.Path)
            //            .IsRequired(false)
            //            .HasDefaultValue(null)
            //            .HasColumnName("main_photo");
            //    });
            //});

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

            builder.Property(v => v.DetailsList)
                .ValueObjectCollectionJsonConversion(
                    details => details,
                    json => json)
                 .HasColumnName("details");

            builder.Property(v => v.Files)
                .ValueObjectCollectionJsonConversion(
                    file => new PetFileDto { PathToStorage = file.PathToStorage.Path },
                    json => new PetFile(FilePath.Create(json.PathToStorage).Value))
                .HasColumnName("file_paths");

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

            builder.Property(v => v.DeletionDate)
                .IsRequired(false)
                .HasDefaultValue(null)
                .HasColumnName("deletion_date");

            builder.ComplexProperty(p => p.CreationDate, b =>
            {
                b.Property(a => a.Value)
                .IsRequired(true)
                .HasColumnName("creation_date");
            });

            builder.ComplexProperty(p => p.Position, b =>
            {
                b.Property(p => p.Value)
                .IsRequired(true)
                .HasColumnName("position");
            });

            builder.Property(p => p.IsDeleted)
                .HasColumnName("is_deleted");
        }
    }
}
