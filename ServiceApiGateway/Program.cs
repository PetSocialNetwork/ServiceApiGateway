using Microsoft.OpenApi.Models;
using Service_ApiGateway.Extensions;
using Service_ApiGateway.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<CentralizedExceptionHandlingFilter>();
});

builder.Services.AddClientServices();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiGateway", Version = "v1" });
    c.MapType<IFormFile>(() => new OpenApiSchema { Type = "string", Format = "binary" });
});
////builder.Services.AddSwaggerGen(c =>
////{
////    var apiInfo = new OpenApiInfo
////    {
////        Title = "Api Gateway",
////        Version = "v1.0",
////        Description = "Версия сборки: " + Assembly.GetEntryAssembly()!.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
////    };
////    c.SwaggerDoc(apiInfo.Version, apiInfo);

////    c.UseAllOfToExtendReferenceSchemas();

////    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, AppDomain.CurrentDomain.FriendlyName + ".xml"));
////});

builder.Services.AddHttpClient();

var app = builder.Build();

app.UseCors(policy =>
{
    policy
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin();
});
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
