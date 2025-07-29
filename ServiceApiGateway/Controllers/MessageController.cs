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
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        public MessageController(IMessageService messageService)
        {
            _messageService = messageService
                ?? throw new ArgumentNullException(nameof(messageService));
        }

        /// <summary>
        /// Возвращает последнее сообщение в чате
        /// </summary>
        /// <param name="chatId">Идентификатор чата</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpGet("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<MessageBySearchResponse?> GetLastMessageByChatIdAsync([FromQuery] Guid chatId, CancellationToken cancellationToken)
        {
            return await _messageService.GetLastMessageByChatIdAsync(chatId, cancellationToken);
        }

        /// <summary>
        /// Удаляет сообщение по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор сообщения</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpDelete("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task DeleteMessageAsync([FromQuery] Guid id, CancellationToken cancellationToken)
        {
            await _messageService.DeleteMessageAsync(id, cancellationToken);
        }

        /// <summary>
        /// Возвращает все сообщения из чата
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IEnumerable<MessageBySearchResponse>> BySearchAsync([FromBody] MessageRequest request, CancellationToken cancellationToken)
        {
            return await _messageService.BySearchAsync(request, cancellationToken);          
        }

        /// <summary>
        /// Возвращает сообщение по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор сообщения</param>
        /// <param name="cancellationToken"></param>
        [HttpGet("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<MessageBySearchResponse> GetMessageByIdAsync([FromQuery] Guid id, CancellationToken cancellationToken)
        {
            return await _messageService.GetMessageByIdAsync(id, cancellationToken);
        }

        /// <summary>
        /// Обновляет сообщение по идентификатору
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPut("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task UpdatetMessageAsync([FromBody] UpdateMessageRequest request, CancellationToken cancellationToken)
        {
            await _messageService.UpdateMessageAsync(request, cancellationToken);
        }
    }
}
