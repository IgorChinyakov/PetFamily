using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.VolunteerRequests.Infrastructure.Outbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Infrastructure.Configurations.Write
{
    public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.ToTable("outbox_messages");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id).HasColumnName("id");

            builder.Property(o => o.Payload)
                .HasColumnName("payload")
                .HasColumnType("jsonb")
                .HasMaxLength(2000)
                .IsRequired();

            builder.Property(o => o.Type)
                .HasColumnName("type")
                .HasMaxLength(2000)
                .IsRequired();

            builder.Property(o => o.OccuredOnUtc)
                .HasColumnName("occured_on_utc")
                .HasConversion(
                    v => v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                .IsRequired();

            builder.Property(o => o.ProcessedOnUtc)
                .HasColumnName("processed_on_utc")
                .HasConversion(
                    v => v!.Value.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                .IsRequired(false);

            builder.Property(o => o.Error).HasColumnName("error");

            builder.HasIndex(o => new
            {
                o.OccuredOnUtc,
                o.ProcessedOnUtc
            })
            .HasDatabaseName("idx_outbox_messages_unprocessed")
            .IncludeProperties(o => new
            {
                o.Id,
                o.Type,
                o.Payload
            })
            .HasFilter("processed_on_utc is NULL");
        }
    }
}
