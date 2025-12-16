using PetSocialNetwork.ServicePetCare;

namespace Service_ApiGateway.Models.Responses
{
    public class ServiceWithPhotoResponse
    {
        public Guid Id { get; set; }
        public Guid ProfileId { get; set; }
        public Guid ServiceTypeId { get; set; }
        public string? Description { get; set; }
        public double? Price { get; set; }
        public ServiceTypeResponse ServiceType { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
