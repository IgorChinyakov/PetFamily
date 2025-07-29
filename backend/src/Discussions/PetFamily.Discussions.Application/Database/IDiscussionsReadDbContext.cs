using PetFamily.Discussions.Contracts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Discussions.Application.Database
{
    public interface IDiscussionsReadDbContext
    {
        IQueryable<DiscussionDto> Discussions { get; }

        IQueryable<MessageDto> Messages { get; }
    }
}
