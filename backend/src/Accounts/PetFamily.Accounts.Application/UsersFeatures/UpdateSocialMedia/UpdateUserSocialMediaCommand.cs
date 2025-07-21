using PetFamily.Core.Abstractions;
using PetFamily.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Application.Features.UpdateSocialMedia
{
    public record UpdateUserSocialMediaCommand(
        Guid Id,
        IEnumerable<SocialMediaDto> SocialMedia) : ICommand;
}
