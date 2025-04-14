﻿using PetFamily.Application.Abstractions;
using PetFamily.Application.Volunteers.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.Volunteers.Commands.UpdateDetails
{
    public record UpdateDetailsCommand(
        Guid VolunteerId, 
        IEnumerable<DetailsDto> Details) : ICommand;
}
