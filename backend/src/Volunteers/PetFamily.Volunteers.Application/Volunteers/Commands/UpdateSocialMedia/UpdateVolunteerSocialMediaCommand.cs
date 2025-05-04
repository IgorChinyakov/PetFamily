using PetFamily.Core.Abstractions;
using PetFamily.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.UpdateSocialMedia
{
    public record UpdateVolunteerSocialMediaCommand(
        Guid Id,
        IEnumerable<SocialMediaDto> SocialMedia) : ICommand;
}
