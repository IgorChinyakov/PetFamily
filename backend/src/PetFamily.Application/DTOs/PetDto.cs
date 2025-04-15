using PetFamily.Domain.VolunteerContext.PetsVO;
using PetFamily.Domain.VolunteerContext.SharedVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.DTOs
{
    public class PetDto
    {
        public Guid Id { get; init; }

        public string NickName { get; init; } = string.Empty;

        public string Description { get; init; } = string.Empty;

        public Guid SpeciesId { get; init; }

        public Guid BreedId { get; init; }

        public Guid VolunteerId { get; init; }

        public string Color { get; init; } = string.Empty;

        public bool IsSterilized { get; init; }

        public bool IsVaccinated { get; init; }

        public string HealthInformation { get; init; } = string.Empty;

        public string Address { get; init; } = string.Empty;

        public float Weight { get; init; }

        public float Height { get; init; }

        public DateTime Birthday { get; init; }

        public DateTime CreationDate { get; init; }

        public string OwnerPhoneNumber { get; init; } = string.Empty;

        public Status PetStatus { get; init; }

        public int Position { get; init; }

        //public IReadOnlyList<DetailsDto> Details { get; init; }

        //public IReadOnlyList<PetFileDto> Files { get; init; }
    }
}
