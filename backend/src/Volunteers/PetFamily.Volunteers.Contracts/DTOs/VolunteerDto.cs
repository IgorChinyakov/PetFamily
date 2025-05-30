using PetFamily.Core.DTOs;

namespace PetFamily.Volunteers.Contracts.DTOs
{
    public class VolunteerDto
    {
        public Guid Id { get; set; }

        public FullNameDto FullName { get; set; } = null!;

        public string Email { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int Experience { get; set; }

        public string PhoneNumber { get; set; } = string.Empty;

        //public DetailsDto[] Details { get; set; } = null!;

        //public SocialMediaDto[] SocialMedia { get; set; } = null!;

        public bool IsDeleted { get; set; }
    }
}
