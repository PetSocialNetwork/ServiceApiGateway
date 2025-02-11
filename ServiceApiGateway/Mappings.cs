using AutoMapper;
using PetSocialNetwork.ServicePet;
using Service_ApiGateway.Models.Responses;

namespace Service_ApiGateway
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<PetProfileResponse, PetProfileBySearchResponse>();
        }
    }
}
