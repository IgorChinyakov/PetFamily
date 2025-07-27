using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Discussions.Contracts.DTOs;
using PetFamily.Discussions.Domain.ValueObjects.Discussion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PetFamily.Discussions.Infrastructure.Configurations.Read
{
    public class DiscussionDtoConfiguration : IEntityTypeConfiguration<DiscussionDto>
    {
        public void Configure(EntityTypeBuilder<DiscussionDto> builder)
        {
            builder.ToTable("discussions");

            builder.HasKey(d => d.DiscussionId).HasName("pk_discussions");

            builder.Property(d => d.DiscussionId).HasColumnName("discussion_id");

            //builder.Property(d => d.UserIds).HasColumnName("users");

            builder.Property(d => d.RelationId).HasColumnName("relation_id");

            builder.Property(d => d.Status)
                .HasConversion(s => s.ToString(), s => ConvertToStatus(s))
                .HasColumnName("discussion_status");

            builder.OwnsMany(d => d.UserIds, builder =>
            {
                builder.ToJson("users");

                builder.Property(ui => ui.Id)
                    .IsRequired(true)
                    .HasJsonPropertyName("id");
            });

            builder.HasMany(d => d.MessageDtos).WithOne().HasForeignKey("discussion_id");

        }

        private static DiscussionStatusDto ConvertToStatus(string stringStatus)
        {
            var status = stringStatus switch
            {
                "Open" => DiscussionStatusDto.Open,
                "Closed" => DiscussionStatusDto.Closed,
                _ => throw new ApplicationException()
            };

            return status;
        }
    }
}
