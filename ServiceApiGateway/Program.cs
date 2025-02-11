using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Service_ApiGateway.Configurations;
using Service_ApiGateway.Extensions;
using Service_ApiGateway.Filters;

var builder = WebApplication.CreateBuilder(args);
JwtConfig? jwtConfig = builder.Configuration
               .GetRequiredSection("JwtConfig")
               .Get<JwtConfig>();
if (jwtConfig is null)
{
    throw new InvalidOperationException("JwtConfig is not configured");
}
builder.Services.AddSingleton(jwtConfig);
builder.Services.AddControllers(options =>
{
    options.Filters.Add<CentralizedExceptionHandlingFilter>();
});
builder.Services.AddScoped<ProfileCompletionFilter>();

builder.Services.AddClientServices();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiGateway", Version = "v1" });
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
             .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     IssuerSigningKey = new SymmetricSecurityKey(jwtConfig.SigningKeyBytes),
                     ValidateIssuerSigningKey = true,
                     ValidateLifetime = true,
                     RequireExpirationTime = true,
                     RequireSignedTokens = true,
                     ValidateAudience = true,
                     ValidateIssuer = true,
                     ValidAudiences = new[] { jwtConfig.Audience },
                     ValidIssuer = jwtConfig.Issuer
                 };
             });

builder.Services.AddAuthorizationBuilder()
    .SetDefaultPolicy(new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
    .RequireAuthenticatedUser()
    .Build());

builder.Services.AddHttpClient();

var app = builder.Build();

app.UseCors(policy =>
{
    policy
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
