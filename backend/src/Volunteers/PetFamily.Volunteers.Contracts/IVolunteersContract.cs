using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.DTOs;
using PetFamily.Core.Models;
using PetFamily.Core.Options;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Application.Volunteers.Commands.Create;
using PetFamily.Volunteers.Contracts.Requests.Pets;
using PetFamily.Volunteers.Contracts.Requests.Volunteers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Volunteers.Contracts
{
    public interface IVolunteersContract
    {
        Task<Result<Guid, ErrorsList>> CreateVolunteer(
            CreateVolunteerRequest request,
            CancellationToken token = default);

        Task<Result<Guid, ErrorsList>> DeleteVolunteer(
            Guid id,
            DeletionOptions options,
            CancellationToken token = default);

        Task<Result<Guid, ErrorsList>> UpdateVounteerDetails(
            Guid id,
            UpdateVolunteerDetailsRequest request,
            CancellationToken token = default);

        Task<Result<Guid, ErrorsList>> UpdateVounteerMainInfo(
            Guid id,
            UpdateVolunteerMainInfoRequest request,
            CancellationToken token = default);

        Task<Result<Guid, ErrorsList>> UpdateVounteerSocialMedia(
            Guid id,
            UpdateVolunteerSocialMediaRequest request,
            CancellationToken token = default);

        Task<Result<VolunteerDto, ErrorsList>> GetVolunteerById(
            Guid id,
            CancellationToken token = default);

        Task<Result<PagedList<VolunteerDto>, ErrorsList>> GetVolunteersWithPagination(
            GetVolunteersWithPaginationRequest request,
            CancellationToken token = default);

        Task<Result<string, ErrorsList>> ChoosePetMainPhoto(
            ChoosePetMainPhotoRequest request,
            Guid volunteerId,
            Guid petId,
            CancellationToken token = default);

        Task<Result<Guid, ErrorsList>> DeletePet(
            Guid volunteerId,
            Guid petId,
            DeletionOptions options,
            CancellationToken token = default);

        Task<UnitResult<ErrorsList>> MovePet(
            MovePetRequest request,
            CancellationToken token = default);
    }
}
