using Microsoft.AspNetCore.Mvc;
using PetSocialNetwork.ServiceNotification;

namespace Service_ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationClient _notificationClient;
        public NotificationController(INotificationClient notificationClient)
        {
            _notificationClient = notificationClient 
                ?? throw new ArgumentException(nameof(notificationClient));
        }

        /// <summary>
        /// Отправляет сообщение по email
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task SendEmail
            ([FromBody] EmailRequest request, CancellationToken cancellationToken)
        {
            await _notificationClient.SendEmailAsync(request, cancellationToken);
        }

        /// <summary>
        /// Отправляет код на email
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<int> SendCodeToEmail
            ([FromBody] EmailRequest request, CancellationToken cancellationToken)
        {
            return await _notificationClient.SendCodeToEmailAsync(request, cancellationToken);
        }
    }
}
