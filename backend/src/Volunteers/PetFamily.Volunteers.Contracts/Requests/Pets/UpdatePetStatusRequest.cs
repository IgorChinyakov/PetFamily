using PetFamily.Core.DTOs;

namespace PetFamily.Volunteers.Contracts.Requests.Pets
{
    public record UpdatePetStatusRequest(PetStatusDto Status);
}