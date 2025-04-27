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
        public Guid Id { get; set; }

        public string NickName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public Guid SpeciesId { get; set; }

        public Guid BreedId { get; set; }  
            
        public Guid VolunteerId { get; set; }

        public string Color { get; set; } = string.Empty;

        public bool IsSterilized { get; set; }

        public bool IsVaccinated { get; set; }

        public string HealthInformation { get; set; } = string.Empty;

        public AddressDto Address { get; set; } = null!;

        public float Weight { get; set; }

        public float Height { get; set; }

        public DateTime Birthday { get; set; }

        public DateTime CreationDate { get; set; }

        public string PhoneNumber { get; set; } = string.Empty;

        public Status Status { get; set; }

        public int Position { get; set; }

        public DetailsDto[] Details { get; set; } = [];

        public PetFileDto[] Files { get; set; } = [];

        public string MainPhoto { get; set; } = string.Empty;

        public bool IsDeleted { get; set; }
    }
}
