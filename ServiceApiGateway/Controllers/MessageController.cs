﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetSocialNetwork.ServiceChat;
using System.Runtime.CompilerServices;

namespace Service_ApiGateway.Controllers
{
    [Authorize]
    [ProfileCompletionFilter]
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageClient _messageClient;
        public MessageController(IMessageClient messageClient)
        {
            _messageClient = messageClient ?? throw new ArgumentNullException(nameof(messageClient));
        }

        [HttpDelete("[action]")]
        public async Task DeleteMessageAsync([FromQuery] Guid id, CancellationToken cancellationToken)
        {
            await _messageClient.DeleteMessageAsync(id, cancellationToken);
        }

        [HttpGet("[action]")]
        public async IAsyncEnumerable<MessageResponse> BySearchAsync([FromQuery] Guid chatId, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            foreach (var messageResult in await _messageClient.BySearchAsync(chatId, cancellationToken))
                yield return messageResult;
        }

        [HttpGet("[action]")]
        public async Task<MessageResponse> GetMessageByIdAsync([FromQuery] Guid id, CancellationToken cancellationToken)
        {
            return await _messageClient.GetMessageByIdAsync(id, cancellationToken);
        }

        [HttpPut("[action]")]
        public async Task UpdatetUserProfileAsync([FromBody] UpdateMessageRequest request, CancellationToken cancellationToken)
        {
            await _messageClient.UpdateMessageAsync(request, cancellationToken);
        }

        [HttpGet("[action]")]
        public async Task<LastMessageResponse?> GetLastMessageByChatIdAsync([FromQuery] Guid chatId, CancellationToken cancellationToken)
        {
            return await _messageClient.GetLastMessageByChatIdAsync(chatId, cancellationToken);
        }
    }
}
