using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSwag;
using NSwag.Generation.Processors.Security;

namespace NadinSoft.WebFramework.Swagger;

public static class SwaggerConfigurationExtensions
{
    public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder, params string[] versions)
    {
        foreach (var version in versions)
        {
            builder.Services.AddOpenApiDocument(opt =>
            {
                opt.Title = "NadinSoft API";
                opt.Version = version;
                opt.DocumentName = version;

                opt.AddSecurity("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "Enter JWT token",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Type = OpenApiSecuritySchemeType.Http,
                    Scheme = "Bearer"
                });

                opt.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Bearer"));
                opt.DocumentProcessors.Add(new ApiVersionDocumentProcessor());
            });
        }

        return builder;
    }

    public static WebApplication UseSwagger(this WebApplication app)
    {
        if (app.Environment.IsProduction())
            return app;

        app.UseOpenApi();
        app.UseSwaggerUi(opt =>
        {
            opt.PersistAuthorization = true;
            opt.EnableTryItOut = true;
            opt.Path = "/swagger";
        });

        app.UseReDoc(settings =>
        {
            settings.Path = "/api-docs/{documentName}";
            settings.DocumentTitle = "NadinSoft API";
        });

        return app;
    }
}