using Microsoft.AspNetCore.Mvc;
using PetSocialNetwork.ServiceFriend;
using PetSocialNetwork.ServicePhoto;
using PetSocialNetwork.ServiceUser;
using Service_ApiGateway.Models.Responses;

namespace Service_ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendShipController : ControllerBase
    {
        private readonly IFriendShipClient _friendShipClient;
        private readonly IUserProfileClient _userProfileClient;
        private readonly IPersonalPhotoClient _photoClient;
        public FriendShipController(IFriendShipClient friendShipClient,
            IUserProfileClient userProfileClient,
            IPersonalPhotoClient photoClient)
        {
            _friendShipClient = friendShipClient 
                ?? throw new ArgumentNullException(nameof(friendShipClient));
            _userProfileClient = userProfileClient ?? throw new ArgumentNullException(nameof(userProfileClient));
            _photoClient = photoClient ?? throw new ArgumentNullException(nameof(photoClient));
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("[action]")]
        public async Task DeleteFriendAsync([FromBody] FriendRequest request, CancellationToken cancellationToken)
        {
            await _friendShipClient.DeleteFriendAsync(request, cancellationToken);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("[action]")]
        public async Task<IEnumerable<FriendResponse>> BySearchAsync([FromQuery] Guid userId, CancellationToken cancellationToken)
        {
            return await _friendShipClient.BySearchAsync(userId, cancellationToken);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("[action]")]
        public async Task<IEnumerable<FriendsInfoResponse>> GetSentRequestAsync([FromQuery] Guid userId, CancellationToken cancellationToken)
        {
            //Транзакция
            var requests = await _friendShipClient.GetSentRequestAsync(userId, cancellationToken);
            return await GetFriendsInfoAsync(requests, cancellationToken);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("[action]")]
        public async Task<IEnumerable<FriendsInfoResponse>> GetReceivedRequestAsync([FromQuery] Guid userId, CancellationToken cancellationToken)
        {
            //Транзакция
            var requests = await _friendShipClient.GetReceivedRequestAsync(userId, cancellationToken);
            return await GetFriendsInfoAsync(requests, cancellationToken);         
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("[action]")]
        public async Task SendFriendRequestAsync([FromBody] FriendRequest request, CancellationToken cancellationToken)
        {
            await _friendShipClient.SendFriendRequestAsync(request, cancellationToken);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(FriendShipNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("[action]")]
        public async Task AcceptFriendAsync([FromBody] FriendRequest request, CancellationToken cancellationToken)
        {
            await _friendShipClient.AcceptFriendAsync(request, cancellationToken);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(FriendShipNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("[action]")]
        public async Task RejectFriendAsync([FromBody] FriendRequest request, CancellationToken cancellationToken)
        {
            await _friendShipClient.RejectFriendAsync(request, cancellationToken);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(FriendShipNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("[action]")]
        public async Task<IEnumerable<FriendsInfoResponse>> GetFriendsWithInfoAsync([FromQuery] Guid userId, CancellationToken cancellationToken)
        {
            //Транзакция
            var friends = await _friendShipClient.BySearchAsync(userId, cancellationToken);
            return await GetFriendsInfoAsync(friends, cancellationToken);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("[action]")]
        public async Task<bool> HasSentRequestAsync([FromQuery] Guid userId, [FromQuery] Guid friendId, CancellationToken cancellationToken)
        {
            return await _friendShipClient.HasSentRequestAsync(userId, friendId, cancellationToken);
        }

        private async Task<IEnumerable<FriendsInfoResponse>> GetFriendsInfoAsync(ICollection<FriendResponse>? friends, CancellationToken cancellationToken)
        {
            if (friends is null || friends.Count == 0)
            {
                return new List<FriendsInfoResponse>();
            }

            var friendIds = friends.Select(f => f.FriendId).ToList();
            var profilesTask = _userProfileClient.GetUserProfilesAsync(friendIds, cancellationToken);
            var photosTask = _photoClient.GetMainPersonalPhotoByIdsAsync(friendIds, cancellationToken);

            await Task.WhenAll(profilesTask, photosTask);

            var profiles = await profilesTask;
            var photos = await photosTask;

            var photoDictionary = photos?.ToDictionary(p => p.ProfileId, p => p.FilePath) ?? new Dictionary<Guid, string>();

            Dictionary<Guid, UserProfileResponse> profileDictionary = null;
            if (profiles is not null && profiles.Count > 0)
            {
                profileDictionary = profiles.ToDictionary(p => p.Id, p => p);
            }

            var result = friends.Select(friend =>
            {
                UserProfileResponse profile = null;
                profileDictionary?.TryGetValue(friend.FriendId, out profile);

                string photoUrl = photoDictionary.GetValueOrDefault(friend.FriendId);

                return new FriendsInfoResponse
                {
                    Id = friend.FriendId,
                    FirstName = profile.FirstName,
                    LastName = profile.LastName,
                    PhotoUrl = photoUrl
                };
            }).ToList();

            return result;
        }
    }
}
