using AutoMapper;
using PetSocialNetwork.ServicePet;
using PetSocialNetwork.ServiceUser;
using Service_ApiGateway.Models.Responses;

namespace Service_ApiGateway
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<PetProfileResponse, PetProfileBySearchResponse>();
            CreateMap<UserProfileResponse, UserProfileBySearchResponse>();
        }
    }
}
