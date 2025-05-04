using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using PetFamily.Core.DTOs;
using PetFamily.Core.Models;
using PetFamily.Core.Options;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Application.Volunteers.Commands.Create;
using PetFamily.Volunteers.Application.Volunteers.Commands.Delete;
using PetFamily.Volunteers.Application.Volunteers.Commands.UpdateDetails;
using PetFamily.Volunteers.Application.Volunteers.Commands.UpdateMainInfo;
using PetFamily.Volunteers.Application.Volunteers.Commands.UpdateSocialMedia;
using PetFamily.Volunteers.Application.Volunteers.Queries.GetVolunteerById;
using PetFamily.Volunteers.Application.Volunteers.Queries.GetVolunteersWithPagination;
using PetFamily.Volunteers.Contracts;
using PetFamily.Volunteers.Contracts.Requests.Volunteers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Volunteers.Presentation
{
    public class VolunteersContract : IVolunteersContract
    {
        private readonly CreateVolunteerHandler _createVolunteerHandler;
        private readonly DeleteVolunteerHandler _deleteVolunteerHandler;
        private readonly GetVolunteerByIdHandler _getVolunteerByIdHandler;
        private readonly GetVolunteersWithPaginationHandler _getVolunteersWithPaginationHandler;
        private readonly UpdateVolunteerDetailsHandler _updateVolunteerDetailsHandler;
        private readonly UpdateVolunteerMainInfoHandler _updateVolunteerMainInfoHandler;
        private readonly UpdateVolunteerSocialMediaHandler _updateVolunteerSocialMediaHandler;

        public VolunteersContract(
            CreateVolunteerHandler createVolunteerHandler, 
            DeleteVolunteerHandler deleteVolunteerHandler, 
            GetVolunteerByIdHandler getVolunteerByIdHandler, 
            GetVolunteersWithPaginationHandler getVolunteersWithPaginationHandler, 
            UpdateVolunteerDetailsHandler updateVolunteerDetailsHandler, 
            UpdateVolunteerMainInfoHandler updateVolunteerMainInfoHandler, 
            UpdateVolunteerSocialMediaHandler updateVolunteerSocialMediaHandler)
        {
            _createVolunteerHandler = createVolunteerHandler;
            _deleteVolunteerHandler = deleteVolunteerHandler;
            _getVolunteerByIdHandler = getVolunteerByIdHandler;
            _getVolunteersWithPaginationHandler = getVolunteersWithPaginationHandler;
            _updateVolunteerDetailsHandler = updateVolunteerDetailsHandler;
            _updateVolunteerMainInfoHandler = updateVolunteerMainInfoHandler;
            _updateVolunteerSocialMediaHandler = updateVolunteerSocialMediaHandler;
        }

        public async Task<Result<Guid, ErrorsList>> CreateVolunteer(
            CreateVolunteerRequest request, CancellationToken token = default)
        {
            return await _createVolunteerHandler.Handle(
                new CreateVolunteerCommand(
                    request.FullName, 
                    request.Email, 
                    request.Description,
                    request.Experience,
                    request.PhoneNumber,
                    request.DetailsList,
                    request.SocialMediaList));
        }

        public async Task<Result<Guid, ErrorsList>> DeleteVolunteer(
           Guid id, DeletionOptions options,  CancellationToken token = default)
        {
            return await _deleteVolunteerHandler.Handle(
                new DeleteVolunteerCommand(id, options));
        }

        public async Task<Result<VolunteerDto, ErrorsList>> GetVolunteerById(
            Guid id, CancellationToken token = default)
        {
            return await _getVolunteerByIdHandler.Handle(
                new GetVolunteerByIdQuery(id), token);
        }

        public async Task<Result<PagedList<VolunteerDto>, ErrorsList>> GetVolunteersWithPagination(
            GetVolunteersWithPaginationRequest request, CancellationToken token = default)
        {
            return await _getVolunteersWithPaginationHandler.Handle(
                new GetVolunteersWithPaginationQuery(request.Page, request.PageSize), token);
        }

        public async Task<Result<Guid, ErrorsList>> UpdateVounteerDetails(
            Guid id, UpdateVolunteerDetailsRequest request, CancellationToken token = default)
        {
            return await _updateVolunteerDetailsHandler.Handle(
                new UpdateVolunteerDetailsCommand(id, request.Details), token);
        }

        public async Task<Result<Guid, ErrorsList>> UpdateVounteerMainInfo(
            Guid id, UpdateVolunteerMainInfoRequest request, CancellationToken token = default)
        {
            return await _updateVolunteerMainInfoHandler.Handle(
                new UpdateVolunteerMainInfoCommand(
                    id, request.FullName, 
                    request.Email, 
                    request.Description, 
                    request.Experience, 
                    request.PhoneNumber), token);
        }

        public async Task<Result<Guid, ErrorsList>> UpdateVounteerSocialMedia(
            Guid id, UpdateVolunteerSocialMediaRequest request, CancellationToken token = default)
        {
            return await _updateVolunteerSocialMediaHandler.Handle(
                new UpdateVolunteerSocialMediaCommand(id, request.SocialMedia));
        }
    }
}
