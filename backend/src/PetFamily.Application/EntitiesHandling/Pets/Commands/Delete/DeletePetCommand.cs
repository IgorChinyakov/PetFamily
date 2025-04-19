using PetFamily.Application.Abstractions;
using PetFamily.Application.EntitiesHandling;

namespace PetFamily.Api.Controllers
{
    public record DeletePetCommand(
        Guid VolunteerId, 
        Guid PetId, 
        DeletionOptions Options,
        string BucketName) : ICommand;
}