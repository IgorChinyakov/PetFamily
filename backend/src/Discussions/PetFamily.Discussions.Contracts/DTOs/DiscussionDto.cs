using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Discussions.Contracts.DTOs
{
    public record DiscussionDto
    {
        public Guid DiscussionId { get; init; }

        public Guid RelationId { get; init; }

        public DiscussionStatusDto Status { get; init; }

        public List<UserDto> UserIds { get; init; } = [];

        public List<MessageDto> MessageDtos { get; init; } = [];

        //ef core
        public DiscussionDto()
        {
        }
    };
}
