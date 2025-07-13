using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NadinSoft.Application.Contracts.User;
using NadinSoft.Application.Contracts.User.Models;
using NadinSoft.Domain.Entities.User;
using NadinSoft.Infrastructure.Identity.Services.Models;

namespace NadinSoft.Infrastructure.Identity.Services.Implementations;

internal class JwtServiceImplementation(IUserClaimsPrincipalFactory<UserEntity> claimPrincipalFactory, IOptions<JwtConfiguration> jwtConfiguration): IJwtService
{
    public async Task<JwtAccessTokenModel> GenerateTokenAsync(UserEntity user, CancellationToken cancellationToken = default)
    {
        var claims = await claimPrincipalFactory.CreateAsync(user);
        var secretKey = Encoding.UTF8.GetBytes(jwtConfiguration.Value.SignInKey);
        var encryptionKey = Encoding.UTF8.GetBytes(jwtConfiguration.Value.EncryptionKey);
        var signInCredential = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);

        var encryptionCredentials = new EncryptingCredentials(

            new SymmetricSecurityKey(encryptionKey), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256
        );

        var descriptor = new SecurityTokenDescriptor()
        {
            Issuer = jwtConfiguration.Value.Issuer,
            Audience = jwtConfiguration.Value.Audience,
            IssuedAt = DateTime.Now,
            SigningCredentials = signInCredential,
            NotBefore = DateTime.Now.AddMinutes(0),
            Expires = DateTime.Now.AddMinutes(jwtConfiguration.Value.ExpirationMinutes),
            Subject = new ClaimsIdentity(claims.Claims),
            EncryptingCredentials = encryptionCredentials,
            // TokenType = "JWE",
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        
        var token = tokenHandler.CreateToken(descriptor);
        
        return new JwtAccessTokenModel(tokenHandler.WriteToken(token), (token.ValidTo-DateTime.UtcNow).TotalSeconds);
    }
}