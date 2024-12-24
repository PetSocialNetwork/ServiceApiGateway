using Microsoft.OpenApi.Models;
using Service_ApiGateway.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddClientServices();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
