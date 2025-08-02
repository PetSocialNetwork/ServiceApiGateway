using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Service_ApiGateway.Configurations;
using Service_ApiGateway.Extensions;
using Service_ApiGateway.Filters;
using Service_ApiGateway.Services.Implementations;
using Service_ApiGateway.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
var jwtConfig = builder.Configuration
               .GetRequiredSection("JwtConfig")
               .Get<JwtConfig>();

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);


if (jwtConfig is null)
{
    throw new InvalidOperationException("JwtConfig is not configured");
}

builder.Services.AddSingleton(jwtConfig);
builder.Services.AddOcelot();
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
    c.CustomSchemaIds(type => type.FullName);
});

builder.Services.AddCors();
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
                     ValidAudiences = [jwtConfig.Audience],
                     ValidIssuer = jwtConfig.Issuer
                 };
             });

builder.Services.AddAuthorizationBuilder()
    .SetDefaultPolicy(new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
    .RequireAuthenticatedUser()
    .Build());

builder.Services.AddHttpClient();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IFriendShipService, FriendShipService>();
builder.Services.AddScoped<IPetService, PetService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IPersonalPhotoService, PersonalPhotoService>();
builder.Services.AddScoped<IPetPhotoService, PetPhotoService>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

app.UseCors(policy =>
{
    policy
        .WithOrigins("http://localhost:3000")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
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
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

await app.UseOcelot();

app.Run();
