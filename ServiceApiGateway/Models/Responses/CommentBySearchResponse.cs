#pragma warning disable CS8618 
using System.ComponentModel.DataAnnotations;

namespace Service_ApiGateway.Models.Responses
{
    public class CommentBySearchResponse
    {
        [Required]
        public Guid Id { get; init; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public Guid UserId { get; init; }
        [Required]
        public Guid PhotoId { get; init; }
        [Required]
        public DateTime CreatedAt { get; init; }
    }
}
