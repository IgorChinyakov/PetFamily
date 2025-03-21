﻿using PetFamily.Application.Volunteers.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.Volunteers.UpdateMainInfo
{
    public record UpdateMainInfoCommand(
        Guid Id, 
        FullNameDto FullName,
        string Email,
        string Description,
        int Experience,
        string PhoneNumber);
}
