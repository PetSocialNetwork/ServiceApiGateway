using Microsoft.AspNetCore.Mvc;
using PetSocialNetwork.ServiceFriend;
using System.Runtime.CompilerServices;

namespace Service_ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendShipController : ControllerBase
    {
        private readonly IFriendShipClient _friendShipClient;
        public FriendShipController(IFriendShipClient friendShipClient)
        {
            _friendShipClient = friendShipClient ?? throw new ArgumentNullException(nameof(friendShipClient));
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("[action]")]
        public async Task DeleteFriendAsync([FromBody] FriendRequest request, CancellationToken cancellationToken)
        {
            await _friendShipClient.DeleteFriendAsync(request, cancellationToken);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("[action]")]
        public async IAsyncEnumerable<FriendResponse> BySearchAsync([FromQuery] Guid userId, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            foreach (var friendShip in await _friendShipClient.BySearchAsync(userId, cancellationToken))
                yield return friendShip;
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("[action]")]
        public async Task<IEnumerable<FriendResponse>> GetSentRequestAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _friendShipClient.GetSentRequestAsync(userId, cancellationToken);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("[action]")]
        public async Task<IEnumerable<FriendResponse>> GetReceivedRequestAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _friendShipClient.GetReceivedRequestAsync(userId, cancellationToken);
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
    }
}
