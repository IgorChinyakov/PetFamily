﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.Volunteers.Delete
{
    public record DeleteVolunteerCommand(Guid Id, DeletionOptions Options);
}
