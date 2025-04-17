using PetFamily.Domain.VolunteerContext.SharedVO;
using PetFamily.Domain.VolunteerContext.VolunteerVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.DTOs
{
    public class VolunteerDto
    {
        public Guid Id { get; set; }

        public FullNameDto FullName { get; set; } = null!;

        public string Email { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int Experience { get; set; }

        public string PhoneNumber { get; set; } = string.Empty;

        public DetailsDto[] Details { get; set; } = null!;

        public SocialMediaDto[] SocialMedia { get; set; } = null!;
    }
} 
