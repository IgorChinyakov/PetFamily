using PetFamily.Core.Abstractions;
using PetFamily.Core.Options;

namespace PetFamily.Volunteers.Application.Pets.Commands.Delete
{
    public record DeletePetCommand(
        Guid VolunteerId,
        Guid PetId,
        DeletionOptions Options,
        string BucketName) : ICommand;
}