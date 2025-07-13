using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NadinSoft.WebFramework.Models;

namespace NadinSoft.WebFramework.Extensions;

public static class WebApplicationBuilderExtensions
{
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
                }
            };
        });

        return builder;
    }
}