namespace NadinSoft.Infrastructure.Identity.Services.Models;

internal class JwtConfiguration
{
   public string SignInKey { get; set; } = string.Empty;
   public string Audience { get; set; } = string.Empty;
   public string Issuer { get; set; } = string.Empty;
   public int ExpirationMinutes { get; set; } = 60;
   
   public string EncryptionKey { get; set; } = string.Empty;
}