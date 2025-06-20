using Microsoft.EntityFrameworkCore;
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

            builder.HasKey(p => p.Id).HasName("pk_pets");

            builder.Property(p => p.Id).HasColumnName("pet_id");

            builder.Property(p => p.BreedId).HasColumnName("breed_id");

            builder.Property(p => p.SpeciesId).HasColumnName("species_id");

            builder.ComplexProperty(p => p.PetStatus, pb =>
            {
                pb.Property(ps => ps.Value)
                .IsRequired(true)
                .HasColumnName("status");
            });

            builder.Property(p => p.BreedId).HasConversion(
                id => id.Value,
                value => BreedId.Create(value).Value);

            builder.Property(p => p.SpeciesId).HasConversion(
                id => id.Value,
                value => SpeciesId.Create(value).Value);

            builder.ComplexProperty(p => p.Address, pb =>
            {
                pb.Property(a => a.Apartment)
                .IsRequired(true)
                .HasColumnName("apartment")
                .HasMaxLength(Address.MAX_LENGTH);

                pb.Property(a => a.Street)
                .IsRequired(true)
                .HasColumnName("street")
                .HasMaxLength(Address.MAX_LENGTH);

                pb.Property(a => a.City)
                .IsRequired(true)
                .HasColumnName("city")
                .HasMaxLength(Address.MAX_LENGTH);
            });

            builder.ComplexProperty(p => p.NickName, pb =>
            {
                pb.Property(a => a.Value)
                .IsRequired(true)
                .HasColumnName("nick_name")
                .HasMaxLength(NickName.MAX_LENGTH);
            });

            builder.ComplexProperty(p => p.Birthday, pb =>
            {
                pb.Property(a => a.Value)
                .IsRequired(true)
                .HasColumnName("birthday");
            });

            builder.ComplexProperty(m => m.MainPhoto, pb =>
            {
                pb.Property(p => p.Path)
                    .IsRequired(false)
                    .HasDefaultValue(string.Empty)
                    .HasColumnName("main_photo");
            });

            builder.ComplexProperty(p => p.Color, pb =>
            {
                pb.Property(c => c.Value)
                .IsRequired(true)
                .HasColumnName("color")
                .HasMaxLength(Constants.MAX_LOW_TITLE_LENGTH);
            });

            builder.ComplexProperty(p => p.Description, pb =>
            {
                pb.Property(e => e.Value)
                .IsRequired(true)
                .HasColumnName("description")
                .HasMaxLength(Description.MAX_LENGTH);
            });

            builder.OwnsMany(p => p.DetailsList, pb =>
            {
                pb.ToJson("details_list");

                pb.Property(d => d.Title)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_LOW_TITLE_LENGTH)
                    .HasColumnName("title");

                pb.Property(d => d.Description)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_HIGH_TITLE_LENGTH)
                    .HasColumnName("description");
            });

            //builder.OwnsMany(p => p.Files, pb =>
            //{
            //    pb.ToJson();

            //    pb.Property(d => d.PathToStorage)
            //        .IsRequired()
            //        .HasMaxLength(Constants.MAX_LOW_TITLE_LENGTH)
            //        .HasColumnName("title");
            //});

            //builder.Property(v => v.DetailsList)
            //    .ValueObjectCollectionJsonConversion(
            //        details => details,
            //        json => json)
            //     .HasColumnName("details");

            builder.Property(v => v.Files)
                .ValueObjectCollectionJsonConversion(
                    file => new PetFileDto { PathToStorage = file.PathToStorage.Path },
                    json => new PetFile(FilePath.Create(json.PathToStorage).Value))
                .HasColumnName("file_paths");

            builder.ComplexProperty(p => p.OwnerPhoneNumber, pb =>
            {
                pb.Property(o => o.Value)
                .IsRequired(true)
                .HasColumnName("phone_number")
                .HasMaxLength(Constants.MAX_LOW_TITLE_LENGTH);
            });

            builder.ComplexProperty(p => p.HealthInformation, pb =>
            {
                pb.Property(h => h.Value)
                .IsRequired(true)
                .HasColumnName("health_information")
                .HasMaxLength(HealthInformation.MAX_LENGTH);
            });

            builder.ComplexProperty(p => p.Height, pb =>
            {
                pb.Property(h => h.Value)
                .IsRequired(true)
                .HasColumnName("height");
            });

            builder.ComplexProperty(p => p.Weight, pb =>
            {
                pb.Property(w => w.Value)
                .IsRequired(true)
                .HasColumnName("weight");
            });

            builder.ComplexProperty(p => p.IsSterilized, pb =>
            {
                pb.Property(i => i.Value)
                .IsRequired(true)
                .HasColumnName("is_sterilized");
            });

            builder.ComplexProperty(p => p.IsVaccinated, pb =>
            {
                pb.Property(i => i.Value)
                .IsRequired(true)
                .HasColumnName("is_vaccinated");
            });

            builder.Property(p => p.DeletionDate)
                .IsRequired(false)
                .HasDefaultValue(null)
                .HasColumnName("deletion_date");

            builder.ComplexProperty(p => p.CreationDate, pb =>
            {
                pb.Property(a => a.Value)
                .IsRequired(true)
                .HasColumnName("creation_date");
            });

            builder.ComplexProperty(p => p.Position, pb =>
            {
                pb.Property(p => p.Value)
                .IsRequired(true)
                .HasColumnName("position");
            });

            builder.Property(p => p.IsDeleted)
                .HasColumnName("is_deleted");
        }
    }
}
