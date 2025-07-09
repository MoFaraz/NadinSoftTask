namespace NadinSoft.Application.Contracts.User.Models;

public record JwtAccessTokenModel(string AccessToken, int ExpiresSeconds, string TokenType = "Bearer");