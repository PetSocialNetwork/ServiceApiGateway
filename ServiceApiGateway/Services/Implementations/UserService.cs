using AutoMapper;
using PetSocialNetwork.ServicePhoto;
using PetSocialNetwork.ServiceUser;
using Service_ApiGateway.Extensions;
using Service_ApiGateway.Models.Responses;
using Service_ApiGateway.Services.Interfaces;

namespace Service_ApiGateway.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserProfileClient _userProfileClient;
        private readonly IPersonalPhotoClient _personalPhotoClient;
        private readonly IMapper _mapper;
        public UserService(IUserProfileClient userProfileClient,
            IPersonalPhotoClient personalPhotoClient,
            IMapper mapper)
        {
            _userProfileClient = userProfileClient ?? throw new ArgumentException(nameof(userProfileClient));
            _personalPhotoClient = personalPhotoClient ?? throw new ArgumentException(nameof(personalPhotoClient));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<UserProfileBySearchResponse> GetUserProfileByIdAsync
            (Guid id, CancellationToken cancellationToken)
        {
            var userProfile = await _userProfileClient.GetUserProfileByIdAsync(id, cancellationToken);
            var photo = await _personalPhotoClient.GetMainPersonalPhotoAsync(userProfile.Id, cancellationToken);
            var response = _mapper.Map<UserProfileBySearchResponse>(userProfile);
            response.PhotoUrl = photo.FilePath;
            return response;
        }

        public async Task<UserProfileBySearchResponse> GetUserProfileByAccountIdAsync
            (Guid id, CancellationToken cancellationToken)
        {
            var userProfile = await _userProfileClient.GetUserProfileByAccountIdAsync(id, cancellationToken);
            var photo = await _personalPhotoClient.GetMainPersonalPhotoAsync(userProfile.Id, cancellationToken);
            var response = _mapper.Map<UserProfileBySearchResponse>(userProfile);
            response.PhotoUrl = photo.FilePath;
            return response;
        }

        public async Task DeleteUserProfileAsync
            (Guid id, CancellationToken cancellationToken)
        {
            await _userProfileClient.DeleteUserProfileAsync(id, cancellationToken);
        }

        public async Task DeleteUserProfileByAccountIdAsync
            (Guid accountId, CancellationToken cancellationToken)
        {
            await _userProfileClient.DeleteUserProfileByAccountIdAsync(accountId, cancellationToken);
        }

        public async Task<UserProfileResponse> AddUserProfileAsync
            (AddUserProfileRequest request, CancellationToken cancellationToken)
        {
            return await _userProfileClient.AddUserProfileAsync(request, cancellationToken);
        }

        public async Task UpdateUserProfileAsync(
            UpdateUserProfileRequest request,
            IFormFile? file, CancellationToken cancellationToken)
        {
            await _userProfileClient.UpdateUserProfileAsync(request, cancellationToken);
            if (file != null)
            {
                await _personalPhotoClient.AddAndSetPersonalPhotoAsync
               (new AddPersonalPhotoRequest()
               {
                   ProfileId = request.Id,
                   FileBytes = await file.ReadBytesAsync(cancellationToken),
                   OriginalFileName = file.FileName
               }, cancellationToken);
            }
        }

        public async Task<IEnumerable<UserProfileBySearchResponse>> FindUserProfileByNameAsync
            (FindUserProfileRequest request, CancellationToken cancellationToken)
        {
            var userProfiles = await _userProfileClient.FindUserProfileByNameAsync(request, cancellationToken);
            var userProfileIds = userProfiles.Select(x => x.Id).ToList();

            var photos = await _personalPhotoClient.GetMainPersonalPhotoByIdsAsync(userProfileIds, cancellationToken);
            var photoDictionary = photos.ToDictionary(p => p.ProfileId, p => p.FilePath);

            var result = userProfiles.Select(userProfile =>
            {
                var photoUrl = photoDictionary.GetValueOrDefault(userProfile.Id);

                return new UserProfileBySearchResponse
                {
                    Id = userProfile.Id,
                    FirstName = userProfile.FirstName,
                    LastName = userProfile.LastName,
                    DateOfBirth = userProfile.DateOfBirth,
                    WalksDogs = userProfile.WalksDogs,
                    Profession = userProfile.Profession,
                    AboutSelf = userProfile.AboutSelf,
                    Interests = userProfile.Interests,
                    AccountId = userProfile.AccountId,
                    IsProfileCompleted = userProfile.IsProfileCompleted,
                    PhotoUrl = photoUrl
                };
            }).ToList();

            return result;
        }
    }
}
