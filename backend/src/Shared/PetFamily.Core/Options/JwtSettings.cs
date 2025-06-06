﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Core.Options
{
    public class JwtSettings
    {
        public const string JwtPath = "JwtSettings";

        public string SecretKey { get; set; } = string.Empty;

        public string Audience {  get; set; } = string.Empty;

        public string Issuer {  get; set; } = string.Empty;

        public int TokenLifeTime {  get; set; }
    }
}
