using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.VolunteerRequests.Domain.Entities;
using PetFamily.VolunteerRequests.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Infrastructure.Configurations.Write
{
    public class RejectedRequestConfiguration :
        IEntityTypeConfiguration<RejectedRequest>
    {
        public void Configure(EntityTypeBuilder<RejectedRequest> builder)
        {
            builder.ToTable("rejected_requests");

            builder.HasKey(vr => vr.Id).HasName("pk_rejected_requests");

            builder.Property(vr => vr.Id).HasConversion(
                requestid => requestid.Value,
                stringId => RequestId.Create(stringId))
                .HasColumnName("request_id");

            builder
                .HasOne(r => r.VolunteerRequest)
                .WithOne(r => r.RejectedRequest)
                .HasForeignKey<RejectedRequest>(r => r.Id);

            builder.ComplexProperty(r => r.RejectionDate, rb =>
            {
                rb.Property(r => r.Value)
                    .IsRequired()
                    .HasColumnName("rejection_date");
            });
        }
    }
}
