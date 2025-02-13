#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;

namespace Service_ApiGateway.Models.Responses
{
    public class FriendsInfoResponse
    {
        [Required]
        public Guid Id{ get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string PhotoUrl{ get; set; }
    }
}
