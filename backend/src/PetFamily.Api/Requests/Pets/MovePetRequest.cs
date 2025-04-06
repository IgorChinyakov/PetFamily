using PetFamily.Application.Pets.Move;

namespace PetFamily.Api.Requests.Pets
{
    public record MovePetRequest(int Position)
    {
        public MovePetCommand ToCommand(Guid volunteerId, Guid petId)
            => new MovePetCommand(volunteerId, petId, Position);
    }
}
