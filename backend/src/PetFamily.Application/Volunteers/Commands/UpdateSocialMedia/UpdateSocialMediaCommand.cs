using PetFamily.Application.Abstractions;
using PetFamily.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.Volunteers.Commands.UpdateSocialMedia
{
    public record UpdateSocialMediaCommand(
        Guid Id, 
        IEnumerable<SocialMediaDto> SocialMedia) : ICommand;
}
