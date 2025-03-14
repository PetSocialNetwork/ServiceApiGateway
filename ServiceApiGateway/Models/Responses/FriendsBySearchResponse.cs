#pragma warning disable CS8618
using PetSocialNetwork.ServiceFriend;
using System.ComponentModel.DataAnnotations;

namespace Service_ApiGateway.Models.Responses
{
    public class FriendsBySearchResponse
    {
        [Required]
        public Guid Id { get; init; }
        [Required]
        public Guid FriendId { get; init; }
        [Required]
        public FriendStatus Status { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public string PhotoUrl { get; set; }
    }
}
