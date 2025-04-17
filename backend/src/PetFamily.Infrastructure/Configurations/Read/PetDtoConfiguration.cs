﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Application.DTOs;
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
            builder.ToTable("pets");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Files)
                .HasConversion(
                     files => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                     json => JsonSerializer.Deserialize<PetFileDto[]>(json, JsonSerializerOptions.Default)!)
                .HasColumnName("file_paths");

            builder.Property(p => p.Details)
                .HasConversion(
                     files => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                     json => JsonSerializer.Deserialize<DetailsDto[]>(json, JsonSerializerOptions.Default)!)
                .HasColumnName("details");

            builder.ComplexProperty(p => p.Address, b =>
            {
                b.Property(a => a.City)
                    .HasColumnName("city");

                b.Property(fn => fn.Street)
                    .HasColumnName("street");

                b.Property(fn => fn.Apartment)
                    .HasColumnName("apartment");
            });
        }
    }
}
