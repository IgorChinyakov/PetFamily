using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.VolunteerRequests.Contracts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Infrastructure.Configurations.Read
{
    public class RejectedRequestDtoConfiguration : IEntityTypeConfiguration<RejectedRequestDto>
    {
        public void Configure(EntityTypeBuilder<RejectedRequestDto> builder)
        {
            builder.ToTable("rejected_requests");

            builder.HasKey(vr => vr.RequestId).HasName("pk_rejected_requests");

            builder.Property(v => v.RequestId).HasColumnName("request_id");

            builder.Property(v => v.RejectionDate).HasColumnName("rejection_date");
        }
    }
}
