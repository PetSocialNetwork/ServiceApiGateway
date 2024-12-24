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

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("[action]")]
        public async Task SendEmail([FromBody] EmailRequest request, CancellationToken cancellationToken)
        {
            await _notificationClient.SendEmailAsync(request, cancellationToken);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("[action]")]
        public async Task<int> SendCodeToEmail([FromBody] EmailRequest request, CancellationToken cancellationToken)
        {
            return await _notificationClient.SendCodeToEmailAsync(request, cancellationToken);
        }
    }
}
