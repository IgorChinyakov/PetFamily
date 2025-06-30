using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.VolunteerRequests.Domain.Entities;
using PetFamily.VolunteerRequests.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Infrastructure.Configurations.Write
{
    public class VolunteerRequestConfiguration :
        IEntityTypeConfiguration<VolunteerRequest>
    {
        public void Configure(EntityTypeBuilder<VolunteerRequest> builder)
        {
            builder.ToTable("volunteer_requests");

            builder.HasKey(vr => vr.Id).HasName("pk_volunteer_requests");

            builder.Property(vr => vr.Id).HasConversion(
                requestid => requestid.Value,
                stringId => RequestId.Create(stringId))
                .HasColumnName("request_id");

            builder.ComplexProperty(vr => vr.VolunteerInformation, rb =>
            {
                rb.Property(vi => vi.Value)
                    .IsRequired(true)
                    .HasMaxLength(VolunteerInformation.MAX_LENGTH)
                    .HasColumnName("volunteer_information");
            });

            builder.OwnsOne(vr => vr.RejectionComment, rb =>
            {
                rb.Property(rc => rc.Value)
                    .IsRequired(false)
                    .HasMaxLength(VolunteerInformation.MAX_LENGTH)
                    .HasColumnName("rejection_comment");
            });

            builder.OwnsOne(vr => vr.AdminId, rb =>
            {
                rb.Property(ai => ai.Value)
                    .IsRequired(true)
                    .HasDefaultValue(Guid.Empty)
                    .HasColumnName("admin_id");
            });

            builder.ComplexProperty(vr => vr.UserId, rb =>
            {
                rb.Property(ui => ui.Value)
                    .IsRequired(true)
                    .HasColumnName("user_id");
            });

            builder.OwnsOne(vr => vr.DiscussionId, rb =>
            {
                rb.Property(di => di.Value)
                    .IsRequired(true)
                    .HasDefaultValue(Guid.Empty)
                    .HasColumnName("discussion_id");
            });

            builder.ComplexProperty(vr => vr.CreationDate, rb =>
            {
                rb.Property(di => di.Value)
                    .IsRequired(true)
                    .HasColumnName("creation_date");
            });

            builder.ComplexProperty(vr => vr.RequestStatus, sb =>
            {
                sb.Property(rs => rs.Value)
                    .HasConversion(s => s.ToString(), s => ConvertToStatus(s))
                    .IsRequired(true)
                    .HasColumnName("request_status");
            });
        }

        private static RequestStatus.Status ConvertToStatus(string stringStatus)
        {
            var status = stringStatus switch
            {
                "Submitted" => RequestStatus.Status.Submitted,
                "Rejected" => RequestStatus.Status.Rejected,
                "RevisionRequired" => RequestStatus.Status.RevisionRequired,
                "Approved" => RequestStatus.Status.Approved,
                "OnReview" => RequestStatus.Status.OnReview,
                _ => throw new ApplicationException()
            };

            return status;
        }
    }
}
