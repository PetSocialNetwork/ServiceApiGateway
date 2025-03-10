#pragma warning disable CS8618 
using System.ComponentModel.DataAnnotations;

namespace Service_ApiGateway.Models.Responses
{
    public class ChatBySearchResponse
    {
        [Required]
        public Guid Id { get; init; }
        [Required]
        public Guid UserId { get; init; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public List<Guid> FriendIds { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string PhotoUrl { get; set; }
        public string? LastMessage { get; set; }
        public string? UserName { get; set; }
    }
}
