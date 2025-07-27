using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Discussions.Contracts.DTOs
{
    public record MessageDto
    {
        public Guid MessageId { get; init; }
        public Guid UserId { get; init; }
        public string Text { get; init; }
        public DateTime CreationDate { get; init; }
        public bool IsEdited { get; init; }

        //ef core
        public MessageDto()
        {
        }
    }
}
