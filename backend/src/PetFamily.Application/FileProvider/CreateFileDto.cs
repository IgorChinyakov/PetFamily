﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.FileProvider
{
    public record CreateFileDto(Stream Content, string FileName);
}
