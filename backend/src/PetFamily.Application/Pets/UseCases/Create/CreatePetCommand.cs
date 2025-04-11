using PetFamily.Application.Volunteers.UseCases.DTOs;
using PetFamily.Domain.VolunteerContext.PetsVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.Pets.UseCases.Create
{
    public record CreatePetCommand(
        Guid VolunteerId,
        string NickName,
        string Description,
        Guid SpeciesId,
        Guid BreedId,
        string Color,
        bool IsSterilized,
        bool IsVaccinated,
        string HealthInformation,
        AddressDto Address,
        float Weight,
        float Height,
        DateTime Birthday,
        Status PetStatus);
}
