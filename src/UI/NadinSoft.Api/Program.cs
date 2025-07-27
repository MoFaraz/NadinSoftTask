using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Mvc;
using NadinSoft.Application.Common.Abstractions;
using NadinSoft.Application.Extensions;
using NadinSoft.Infrastructure.CrossCutting.Logging;
using NadinSoft.Infrastructure.Identity.Extensions;
using NadinSoft.Infrastructure.Persistence.Extensions;
using NadinSoft.WebFramework.Extensions;
using NadinSoft.WebFramework.Filters;
using NadinSoft.WebFramework.Models;
using NadinSoft.WebFramework.Services;
using NadinSoft.WebFramework.Swagger;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(LoggingConfiguration.ConfigureLogger);

builder
    .AddSwagger("v1")
    .AddVersioning();

builder.Services
    .AddIdentityServices(builder.Configuration)
    .AddApplicationAutoMapper()
    .AddApplicationMediatorServices()
    .RegisterApplicationValidator()
    .AddPersistenceDbContext(builder.Configuration);

builder.ConfigureAuthenticationAndAuthorization();


builder.Services.AddControllers(opt =>
{
    opt.Filters.Add(typeof(OkResultAttribute));
    opt.Filters.Add(typeof(NotFoundAttribute));
    opt.Filters.Add(typeof(BadResultAttribute));
    opt.Filters.Add(typeof(ForbiddenResultAttribute));
    opt.Filters.Add(typeof(ModelStateValidationAttribute));
    opt.Filters.Add(new ProducesResponseTypeAttribute(typeof(ApiResult<Dictionary<string, List<string>>>),
        StatusCodes.Status400BadRequest));

    opt.Filters.Add(new ProducesResponseTypeAttribute(typeof(ApiResult),
        StatusCodes.Status401Unauthorized));

    opt.Filters.Add(new ProducesResponseTypeAttribute(typeof(ApiResult),
        StatusCodes.Status403Forbidden));

    opt.Filters.Add(new ProducesResponseTypeAttribute(typeof(ApiResult),
        StatusCodes.Status500InternalServerError));
}).ConfigureApiBehaviorOptions(opt =>
{
    opt.SuppressModelStateInvalidFilter = true;
    opt.SuppressMapClientErrors = true;
});
builder.Services.AddRateLimiting(builder.Configuration);
builder.Services.AddScoped<ILinkService, LinkService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    await app.ApplyMigrationsAsync();
}

app.UseIpRateLimiting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();