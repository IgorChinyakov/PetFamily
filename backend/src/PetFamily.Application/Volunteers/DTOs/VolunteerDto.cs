using PetFamily.Domain.VolunteerContext.SharedVO;
using PetFamily.Domain.VolunteerContext.VolunteerVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.Volunteers.DTOs
{
    public class VolunteerDto
    {
        public Guid Id { get; init; }

        public string Name { get; init; } = string.Empty;

        public string? SecondName { get; init; } = string.Empty;

        public string FamilyName { get; init; } = string.Empty;

        public string Email { get; init; } = string.Empty;

        public string Description { get; init; } = string.Empty;

        public int Experience { get; init; }

        public string PhoneNumber { get; init; } = string.Empty;

        public IReadOnlyList<DetailsDto> Details {  get; init; }

        public IReadOnlyList<SocialMediaDto> SocialMedia { get; init; }
    }
} 
