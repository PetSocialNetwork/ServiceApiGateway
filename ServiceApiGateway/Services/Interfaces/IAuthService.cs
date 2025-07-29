using Microsoft.AspNetCore.Mvc;
using PetSocialNetwork.ServiceAuth;

namespace Service_ApiGateway.Services.Interfaces
{
    public interface IAuthService
    {
        Task<RegisterResponse> Register(RegisterRequest request, IFormFile file, CancellationToken cancellationToken);
        Task<ActionResult<LoginResponse>> LoginByPassword(LoginRequest request, CancellationToken cancellationToken);
        Task UpdatePassword(UpdatePasswordRequest request, CancellationToken cancellationToken);
        Task ResetPassword(ResetPasswordRequest request, CancellationToken cancellationToken);
        Task<bool> IsRegisterUserAsync(ResetPasswordRequest request, CancellationToken cancellationToken);
        Task<bool> IsTheSameUserPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken);
    }
}
