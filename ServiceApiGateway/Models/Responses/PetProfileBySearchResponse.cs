#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;

namespace Service_ApiGateway.Models.Responses
{
    public class PetProfileBySearchResponse
    {
        [Required]
        public Guid Id { get; init; }
        [Required]
        public Guid ProfileId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public int Age { get; set; }
        public string? Description { get; set; }
        public string PhotoUrl { get; set; }
    }
}
