using AutoMapper;
using PetSocialNetwork.ServiceComments;
using PetSocialNetwork.ServicePet;
using PetSocialNetwork.ServicePhoto;
using Service_ApiGateway.Extensions;
using Service_ApiGateway.Models.Responses;
using Service_ApiGateway.Services.Interfaces;
using PaginationRequest = PetSocialNetwork.ServicePhoto.PaginationRequest;

namespace Service_ApiGateway.Services.Implementations
{
    public class PetService : IPetService
    {
        private readonly IPetProfileClient _petProfileClient;
        private readonly IPetPhotoClient _petPhotoClient;
        private readonly ICommentClient _commentClient;
        private readonly IMapper _mapper;
        public PetService(IPetProfileClient petProfileClient,
            IPetPhotoClient petPhotoCleint,
            ICommentClient commentClient,
            IMapper mapper)
        {
            _petProfileClient = petProfileClient 
                ?? throw new ArgumentException(nameof(petProfileClient));
            _petPhotoClient = petPhotoCleint 
                ?? throw new ArgumentException(nameof(petPhotoCleint));
            _commentClient = commentClient 
                ?? throw new ArgumentException(nameof(commentClient));
            _mapper = mapper 
                ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<PetProfileResponse> AddPetProfileAsync(
           AddPetProfileRequest request,
           IFormFile file, CancellationToken cancellationToken)
        {
            var response = await _petProfileClient.AddPetProfileAsync(request, cancellationToken);
            await _petPhotoClient.AddAndSetPetPhotoAsync
               (new AddPetPhotoRequest()
               {
                   ProfileId = request.ProfileId,
                   PetId = response.Id,
                   FileBytes = await file.ReadBytesAsync(cancellationToken),
                   OriginalFileName = file.FileName
               }, cancellationToken);

            return response;
        }

        public async Task<PetProfileBySearchResponse> GetPetProfileByIdAsync
            (Guid id, CancellationToken cancellationToken)
        {
            var profile = await _petProfileClient.GetPetProfileByIdAsync(id, cancellationToken);
            var photo = await _petPhotoClient.GetMainPetPhotoAsync(profile.Id, profile.ProfileId, cancellationToken);
            var response = _mapper.Map<PetProfileBySearchResponse>(profile);
            response.PhotoUrl = photo.FilePath;
            return response;
        }

        public async Task UpdatePetProfileAsync(
            UpdatePetProfileRequest request,
            IFormFile? file, CancellationToken cancellationToken)
        {
            await _petProfileClient.UpdatePetProfileAsync(request, cancellationToken);
            if (file != null)
            {
                await _petPhotoClient.AddAndSetPetPhotoAsync(new AddPetPhotoRequest()
                {
                    ProfileId = request.ProfileId,
                    PetId = request.Id,
                    FileBytes = await file.ReadBytesAsync(cancellationToken),
                    OriginalFileName = file.FileName
                }, cancellationToken);
            }
        }

        public async Task DeletePetProfileAsync
            (Guid petId, Guid profileId, CancellationToken cancellationToken)
        {
            int offset = 0;
            const int batchSize = 10;
            await _petProfileClient.DeletePetProfileAsync(petId, cancellationToken);

            while (true)
            {
                var request = new PetPhotoBySearchRequest()
                {
                    PetId = petId,
                    ProfileId = profileId,
                    Pagination = new PaginationRequest() { Take = batchSize, Offset = offset }
                };

                var photos = await _petPhotoClient.BySearchAsync(request, cancellationToken);

                if (photos == null || !photos.Any())
                {
                    break;
                }

                var photoIds = photos.Select(p => p.Id).ToArray();

                await Task.WhenAll(
                    _commentClient.DeleteAllCommentAsync(photoIds, cancellationToken),
                    _petPhotoClient.DeleteAllPetPhotosAsync(petId, profileId, cancellationToken)
                );

                offset += batchSize; 
            }
        }

        public async Task<IEnumerable<PetProfileBySearchResponse>> GetPetProfilesAsync
            (Guid profileId, CancellationToken cancellationToken)
        {
            var petProfiles = await _petProfileClient.GetPetProfilesAsync(profileId, cancellationToken);
            List<PetProfileBySearchResponse> result = [];

            if (petProfiles != null)
            {
                foreach (var petProfile in petProfiles)
                {
                    var photo = await _petPhotoClient.GetMainPetPhotoAsync(petProfile.Id, profileId, cancellationToken);
                    var response = _mapper.Map<PetProfileBySearchResponse>(petProfile);
                    response.PhotoUrl = photo.FilePath;
                    result.Add(response);
                }
            }
            return result;
        }
    }
}
