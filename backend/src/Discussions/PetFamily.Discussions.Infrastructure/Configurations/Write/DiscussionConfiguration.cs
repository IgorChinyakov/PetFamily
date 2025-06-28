using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Discussions.Domain.Entities;
using PetFamily.Discussions.Domain.ValueObjects.Discussion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Discussions.Infrastructure.Configurations.Write
{
    public class DiscussionConfiguration :
        IEntityTypeConfiguration<Discussion>
    {
        public void Configure(EntityTypeBuilder<Discussion> builder)
        {
            builder.ToTable("discussions");

            builder.HasKey(d => d.Id).HasName("pk_discussions");

            builder.Property(vr => vr.Id).HasConversion(
                requestid => requestid.Value,
                stringId => DiscussionId.Create(stringId))
                .HasColumnName("discussion_id");

            builder.HasMany(d => d.Messages).WithOne().HasForeignKey("discussion_id");

            builder.ComplexProperty(d => d.RelationId, db =>
            {
                db.Property(r => r.Value)
                    .IsRequired(true)
                    .HasColumnName("relation_id");
            });

            builder.OwnsMany(d => d.UserIds, ub =>
            {
                ub.ToJson("users");

                ub.Property(ui => ui.Value)
                    .IsRequired(true)
                    .HasColumnName("id");
            });

            builder.ComplexProperty(d => d.Status, sb =>
            {
                sb.Property(ds => ds.Value)
                    .HasConversion(s => s.ToString(), s => ConvertToStatus(s))
                    .IsRequired(true)
                    .HasColumnName("discussion_status");
            });
        }

        private static DiscussionStatus.Status ConvertToStatus(string stringStatus)
        {
            var status = stringStatus switch
            {
                "Open" => DiscussionStatus.Status.Open,
                "Closed" => DiscussionStatus.Status.Closed,
                _ => throw new ApplicationException()
            };

            return status;
        }
    }
}
