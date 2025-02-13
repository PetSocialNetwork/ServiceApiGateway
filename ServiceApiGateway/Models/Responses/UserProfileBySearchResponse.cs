#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;

namespace Service_ApiGateway.Models.Responses
{
    public class UserProfileBySearchResponse
    {
        [Required]
        public Guid Id { get; init; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool WalksDogs { get; set; }
        public string? Profession { get; set; }
        [Required]
        public Guid AccountId { get; set; }
        public bool IsProfileCompleted { get; set; }
        public string PhotoUrl { get; set; }
    }
}
