using PetFamily.Volunteers.Application.Pets.Commands.Move;

namespace PetFamily.Volunteers.Contracts.Requests.Pets
{
    public record MovePetRequest(int Position)
    {
        public MovePetCommand ToCommand(Guid volunteerId, Guid petId)
            => new MovePetCommand(volunteerId, petId, Position);
    }
}
