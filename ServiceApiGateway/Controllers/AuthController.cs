﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetSocialNetwork.ServiceAuth;

namespace Service_ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthClient _authClient;
        public AuthController(IAuthClient authClient)
        {
            _authClient = authClient ?? throw new ArgumentNullException(nameof(authClient));
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InvalidOperationException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("[action]")]
        public async Task<ActionResult<RegisterResponse>> Register
            (RegisterRequest request, CancellationToken cancellationToken)
        {
            try
            {
                return await _authClient.RegisterAsync(request, cancellationToken);
            }
            catch (ApiException ex)
            {                
                return BadRequest(ex.Response);
            }
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InvalidPasswordException))]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(AccountNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("[action]")]
        public async Task<ActionResult<LoginResponse>> LoginByPassword(LoginRequest request, CancellationToken cancellationToken)
        {
            return await _authClient.LoginByPasswordAsync(request, cancellationToken);
        }

        //[Authorize]
        [HttpDelete("[action]")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(AccountNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task DeleteAccount(Guid id, CancellationToken cancellationToken)
        {
            await _authClient.DeleteAccountAsync(id, cancellationToken);
        }

        [Authorize]
        [ProfileCompletionFilter]
        [HttpPut("[action]")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(PasswordNotChangedException))]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(AccountNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InvalidPasswordException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task UpdatePassword(UpdatePasswordRequest request, CancellationToken cancellationToken)
        {
            await _authClient.UpdatePasswordAsync(request, cancellationToken);
        }

        [Authorize]
        [HttpPut("[action]")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InvalidPasswordException))]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(AccountNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task ResetPassword(ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            await _authClient.ResetPasswordAsync(request, cancellationToken);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InvalidPasswordException))]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(AccountNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("[action]")]
        public async Task<bool> IsRegisterUserAsync([FromBody] ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            return await _authClient.IsRegisterUserAsync(request, cancellationToken);
        }
    }
}
