using CSharpFunctionalExtensions;
using PetFamily.Core.DTOs;
using PetFamily.Core.Models;
using PetFamily.Volunteers.Contracts.Requests.Pets;

namespace PetFamily.Volunteers.Contracts
{
    public interface IVolunteersContract
    {
        Task<Result<PagedList<PetDto>>> GetFilteredPetsWithPagination(
            GetFilteredPetsWithPaginationRequest request,
            CancellationToken cancellationToken = default);
    }
}
