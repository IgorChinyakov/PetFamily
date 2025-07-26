using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.VolunteerRequests.Contracts.DTOs;
using PetFamily.VolunteerRequests.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Infrastructure.Configurations.Read
{
    public class RequestDtoConfiguration : 
        IEntityTypeConfiguration<VolunteerRequestDto>
    {
        public void Configure(EntityTypeBuilder<VolunteerRequestDto> builder)
        {
            builder.ToTable("volunteer_requests");

            builder.HasKey(vr => vr.Id).HasName("pk_volunteer_requests");

            builder.Property(v => v.Id).HasColumnName("request_id");

            builder.Property(v => v.UserId).HasColumnName("user_id");

            builder.Property(v => v.DiscussionId).HasColumnName("discussion_id");

            builder.Property(v => v.AdminId).HasColumnName("admin_id");

            builder.Property(v => v.UserId).HasColumnName("user_id");

            builder.Property(v => v.CreationDate).HasColumnName("creation_date");

            builder.Property(v => v.VolunteerInformation).HasColumnName("volunteer_information");

            builder.Property(v => v.RejectionComment).HasColumnName("rejection_comment");

            builder.Property(v => v.Status).HasConversion(s => s.ToString(), s => ConvertToStatus(s)).HasColumnName("request_status");
        }

        private static RequestStatusDto ConvertToStatus(string stringStatus)
        {
            var status = stringStatus switch
            {
                "Submitted" => RequestStatusDto.Submitted,
                "Rejected" => RequestStatusDto.Rejected,
                "RevisionRequired" => RequestStatusDto.RevisionRequired,
                "Approved" => RequestStatusDto.Approved,
                "OnReview" => RequestStatusDto.OnReview,
                _ => throw new ApplicationException()
            };

            return status;
        }
    }
}
