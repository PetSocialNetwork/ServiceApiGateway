using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using PetSocialNetwork.ServiceUser;

public class ProfileCompletionFilter : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var authorizationHeader = context.HttpContext.Request.Headers.Authorization.ToString();
        string token = null;
        if (authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            token = authorizationHeader.Substring("Bearer ".Length);
        }

        if (!string.IsNullOrEmpty(token))
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var accountIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid");

                if (accountIdClaim == null)
                {
                    context.Result = new BadRequestResult();
                    return;
                }

                if (!Guid.TryParse(accountIdClaim.Value, out Guid accountId))
                {
                    context.Result = new BadRequestResult();
                    return;
                }

                var accountService = context.HttpContext.RequestServices.GetRequiredService<IUserProfileClient>();

                var profile = await accountService.GetUserProfileByAccountIdAsync(accountId);

                if (profile == null || !profile.IsProfileCompleted)
                {
                    context.Result = new ObjectResult(new { message = "Profile is not completed" })
                    {
                        StatusCode = StatusCodes.Status403Forbidden
                    };
                    return;
                }
            }
            catch (SecurityTokenException ex)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            catch (Exception ex)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
                return;
            }
        }

        await next();
    }
}