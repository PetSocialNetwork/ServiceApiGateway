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
            _notificationClient = notificationClient ?? throw new ArgumentException(nameof(notificationClient));
        }

        [HttpPost("[action]")]
        public async Task SendEmail([FromBody] EmailRequest request, CancellationToken cancellationToken)
        {
            await _notificationClient.SendEmailAsync(request, cancellationToken);
        }

        [HttpPost("[action]")]
        public async Task<int> SendCodeToEmail([FromBody] EmailRequest request, CancellationToken cancellationToken)
        {
            return await _notificationClient.SendCodeToEmailAsync(request, cancellationToken);
        }
    }
}
