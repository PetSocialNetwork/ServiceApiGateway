using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetSocialNetwork.ServiceChat;
using Service_ApiGateway.Models.Responses;
using Service_ApiGateway.Services.Interfaces;

namespace Service_ApiGateway.Controllers
{
    [Authorize]
    [ProfileCompletionFilter]
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        public ChatController(IChatService chatService)
        {
            _chatService = chatService 
                ?? throw new ArgumentNullException(nameof(chatService));
        }

        /// <summary>
        /// Удаляет чат по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор чата</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpDelete("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task DeleteChatAsync([FromQuery] Guid id, CancellationToken cancellationToken)
        {
            await _chatService.DeleteChatAsync(id, cancellationToken);
        }

        /// <summary>
        /// Добавляет новый чат
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<AddChatResponse> AddChatAsync([FromBody] AddChatRequest request, CancellationToken cancellationToken)
        {
           return await _chatService.AddChatAsync(request, cancellationToken);
        }

        /// <summary>
        /// Получает имеющийся чат, либо создает
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<AddChatResponse> GetOrCreateChatAsync([FromBody] AddChatRequest request, CancellationToken cancellationToken)
        {
            return await _chatService.GetOrCreateChatAsync(request, cancellationToken);
        }

        /// <summary>
        /// Возвращает чат по идентифкатору
        /// </summary>
        /// <param name="id">Идентификатор чата</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpGet("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<AddChatResponse> GetChatByIdAsync([FromQuery] Guid id, CancellationToken cancellationToken)
        {
            return await _chatService.GetChatByIdAsync(id, cancellationToken);
        }

        /// <summary>
        /// Возвращает все чаты по идентификатору пользователя
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IEnumerable<ChatBySearchResponse>> BySearchAsync([FromBody] ChatRequest request, CancellationToken cancellationToken)
        {
           return await _chatService.BySearchAsync(request, cancellationToken);
        }
    }
}
