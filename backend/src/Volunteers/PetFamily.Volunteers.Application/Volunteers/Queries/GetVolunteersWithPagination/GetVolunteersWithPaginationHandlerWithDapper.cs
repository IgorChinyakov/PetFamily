using Dapper;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Abstractions.Database;
using PetFamily.Core.DTOs;
using PetFamily.Core.Models;
using PetFamily.Volunteers.Contracts.DTOs;
using System.Text.Json;

namespace PetFamily.Volunteers.Application.Volunteers.Queries.GetVolunteersWithPagination
{
    public class GetVolunteersWithPaginationHandlerWithDapper
        : IQueryHandler<PagedList<VolunteerDto>, GetVolunteersWithPaginationQueryWithDapper>
    {
        private readonly ISqlDbConnectionFactory _sqlDbConnectionFactory;

        public GetVolunteersWithPaginationHandlerWithDapper(ISqlDbConnectionFactory sqlDbConnectionFactory)
        {
            _sqlDbConnectionFactory = sqlDbConnectionFactory;
        }

        public async Task<PagedList<VolunteerDto>> Handle(
            GetVolunteersWithPaginationQueryWithDapper query,
            CancellationToken cancellationToken = default)
        {
            var connection = _sqlDbConnectionFactory.Create();

            var parameters = new DynamicParameters();

            var totalCount = await connection.ExecuteScalarAsync<long>("SELECT COUNT(*) FROM pet_management.volunteers");

            var sql = """
                    SELECT id, description, email, experience, phone_number, name, second_name, family_name FROM pet_management.volunteers
                    LIMIT @PageSize OFFSET @OffSet
                    """;

            parameters.Add("@PageSize", query.PageSize);
            parameters.Add("@OffSet", (query.Page - 1) * query.PageSize);

            var volunteers = await connection.QueryAsync<VolunteerDto, FullNameDto, VolunteerDto>(
                sql,
                (volunteer, fullName) =>
                {
                    //var details = JsonSerializer.Deserialize<DetailsDto[]>(jsonDetails) ?? [];
                    //var socialMedia = JsonSerializer.Deserialize<SocialMediaDto[]>(jsonSocialMedia) ?? [];

                    //volunteer.Details = details;
                    //volunteer.SocialMedia = socialMedia;
                    volunteer.FullName = fullName;

                    return volunteer;
                },
                splitOn: "name",
                param: parameters);

            return new PagedList<VolunteerDto>()
            {
                Items = volunteers.ToList(),
                TotalCount = totalCount,
                Page = query.Page,
                PageSize = query.PageSize
            };
        }
    }
}
