using Microsoft.AspNetCore.Mvc;
using PetSocialNetwork.ServiceFriend;
using Service_ApiGateway.Models.Responses;
using Service_ApiGateway.Services.Interfaces;

namespace Service_ApiGateway.Controllers
{
    [ProfileCompletionFilter]
    [Route("api/[controller]")]
    [ApiController]
    public class FriendShipController : ControllerBase
    {
        private readonly IFriendShipService _friendShipService;
        public FriendShipController(IFriendShipService friendShipService)
        {
            _friendShipService = friendShipService 
                ?? throw new ArgumentNullException(nameof(friendShipService));
        }

        /// <summary>
        /// Удаляет пользователя из друзей
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task DeleteFriendAsync
            ([FromBody] FriendRequest request, CancellationToken cancellationToken)
        {
            await _friendShipService.DeleteFriendAsync(request, cancellationToken);
        }

        /// <summary>
        /// Возвращает отправленные заявки в друзья
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IEnumerable<FriendsInfoResponse>> GetSentRequestAsync([FromBody] FriendBySearchRequest request, CancellationToken cancellationToken)
        {
            return await _friendShipService.GetSentRequestAsync(request, cancellationToken);
        }

        /// <summary>
        /// Возвращает полученные заявки в друзья
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IEnumerable<FriendsInfoResponse>> GetReceivedRequestAsync
            ([FromBody] FriendBySearchRequest request, CancellationToken cancellationToken)
        {
            return await _friendShipService.GetReceivedRequestAsync(request, cancellationToken);  
        }

        /// <summary>
        /// Отправляет заявку на дружбу
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task SendFriendRequestAsync
            ([FromBody] FriendRequest request, CancellationToken cancellationToken)
        {
            await _friendShipService.SendFriendRequestAsync(request, cancellationToken);
        }

        /// <summary>
        /// Принимает заявку в друзья
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPut("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task AcceptFriendAsync
            ([FromBody] FriendRequest request, CancellationToken cancellationToken)
        {
            await _friendShipService.AcceptFriendAsync(request, cancellationToken);
        }

        /// <summary>
        /// Отклоняет заявку в друзья
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPut("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task RejectFriendAsync
            ([FromBody] FriendRequest request, CancellationToken cancellationToken)
        {
            await _friendShipService.RejectFriendAsync(request, cancellationToken);
        }

        /// <summary>
        /// Проверяет, есть ли уже отправленная заявку на дружбу
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="friendId">Идентификатор друга</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpGet("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<bool> HasSentRequestAsync
            ([FromBody] FriendRequest request, CancellationToken cancellationToken)
        {
            return await _friendShipService.HasSentRequestAsync(request, cancellationToken);
        }

        /// <summary>
        /// Возвращает всех друзей с информацией
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("[action]")]
        public async Task<IEnumerable<FriendsInfoResponse>> GetFriendsWithInfoAsync
            ([FromBody] FriendBySearchRequest request, CancellationToken cancellationToken)
        {
            return await _friendShipService.GetFriendsWithInfoAsync(request, cancellationToken);
        }
    }
}
