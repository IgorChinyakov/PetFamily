﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.DTOs
{
    public class BreedDto
    {
        public Guid Id { get; set; }

        public Guid SpeciesId { get; set; }

        public string Name { get; init; } = string.Empty;
    }
}
