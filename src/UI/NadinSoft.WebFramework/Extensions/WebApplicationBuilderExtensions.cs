using System.Text;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NadinSoft.WebFramework.Models;

namespace NadinSoft.WebFramework.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddVersioning(this WebApplicationBuilder builder)
    {
        builder.Services.AddApiVersioning(opt =>
            {
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ReportApiVersions = true;
                opt.ApiVersionReader = new UrlSegmentApiVersionReader();
            }).AddMvc()
            .AddApiExplorer(opt =>
            {
                opt.GroupNameFormat = "'v'V";
                opt.SubstituteApiVersionInUrl = true;
            });
        return builder;
    }

    public static WebApplicationBuilder ConfigureAuthenticationAndAuthorization(this WebApplicationBuilder builder)
    {
        var signInKey = builder.Configuration.GetSection("JwtConfiguration")["SignInKey"];
        var encryptionKey = builder.Configuration.GetSection("JwtConfiguration")["EncryptionKey"];
        var issuer = builder.Configuration.GetSection("JwtConfiguration")["Issuer"];
        var audience = builder.Configuration.GetSection("JwtConfiguration")["Audience"];

        builder.Services.AddAuthorization();
        builder.Services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(opt =>
        {
            var validationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signInKey!)),

                ClockSkew = TimeSpan.Zero,
                RequireSignedTokens = true,
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                TokenDecryptionKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(encryptionKey!)),
            };
            opt.TokenValidationParameters = validationParameters;
            opt.Events = new JwtBearerEvents()
            {
                OnForbidden = async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsJsonAsync(new ApiResult(false, "Forbidden",
                        ApiResultStatusCode.Forbidden));
                },
                OnAuthenticationFailed = context =>
                {
                    context.HttpContext.Items["AuthFailed"] = context.Exception;
                    return Task.CompletedTask;
                },

                OnChallenge = async context =>
                {
                    context.HandleResponse();

                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";

                    var ex = context.HttpContext.Items["AuthFailed"] as Exception;

                    var message = ex is SecurityTokenExpiredException
                        ? "Token Expired"
                        : "Unauthorized";

                    var result = new ApiResult(false, message, ApiResultStatusCode.Unauthorized);

                    await context.Response.WriteAsJsonAsync(result);
                }
            };
        });


        return builder;
    }
}