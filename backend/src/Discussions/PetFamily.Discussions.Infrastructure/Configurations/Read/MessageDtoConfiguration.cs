using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Discussions.Contracts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Discussions.Infrastructure.Configurations.Read
{
    public class MessageDtoConfiguration : IEntityTypeConfiguration<MessageDto>
    {
        public void Configure(EntityTypeBuilder<MessageDto> builder)
        {
            builder.ToTable("messages");

            builder.HasKey(d => d.MessageId).HasName("pk_message");

            builder.Property(d => d.MessageId).HasColumnName("message_id");

            builder.Property(d => d.UserId).HasColumnName("user_id");

            builder.Property(d => d.IsEdited).HasColumnName("is_edited");

            builder.Property(d => d.CreationDate).HasColumnName("creation_date");

            builder.Property(d => d.Text).HasColumnName("text");
        }
    }
}
