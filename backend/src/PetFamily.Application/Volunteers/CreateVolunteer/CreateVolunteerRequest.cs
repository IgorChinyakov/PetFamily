namespace PetFamily.Application.Volunteers.CreateVolunteer
{
    public record CreateVolunteerRequest(FullnameDto FullName, 
        string Email, 
        string Description, 
        int Experience, 
        DetailsDto Details,
        string PhoneNumber,
        List<SocialMediaDto> SocialMediaList);

    public record FullnameDto(string Name, string? SecondName, string FamilyName);

    public record DetailsDto(string Title, string Description);

    public record SocialMediaDto(string Title, string Link);
}
