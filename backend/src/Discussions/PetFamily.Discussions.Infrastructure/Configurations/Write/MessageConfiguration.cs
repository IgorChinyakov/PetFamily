using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Discussions.Domain.Entities;
using PetFamily.Discussions.Domain.ValueObjects.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Discussions.Infrastructure.Configurations.Write
{
    public class MessageConfiguration :
        IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("messages");

            builder.HasKey(m => m.Id).HasName("pk_message");

            builder.Property(m => m.Id).HasConversion(
                requestid => requestid.Value,
                stringId => MessageId.Create(stringId))
                .HasColumnName("message_id");

            builder.ComplexProperty(m => m.CreationDate, mb =>
            {
                mb.Property(cd => cd.Value)
                    .IsRequired(true)
                    .HasColumnName("creation_date");
            });

            builder.ComplexProperty(m => m.UserId, mb =>
            {
                mb.Property(ui => ui.Value)
                    .IsRequired(true)
                    .HasColumnName("user_id");
            });

            builder.ComplexProperty(m => m.Text, mb =>
            {
                mb.Property(t => t.Value)
                    .IsRequired(true)
                    .HasColumnName("text");
            });

            builder.ComplexProperty(m => m.IsEdited, mb =>
            {
                mb.Property(ie => ie.Value)
                    .IsRequired(true)
                    .HasDefaultValue(false)
                    .HasColumnName("is_edited");
            });
        }
    }
}
