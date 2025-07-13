namespace NadinSoft.Application.Contracts.User.Models;

public record JwtAccessTokenModel(string AccessToken, double ExpiresSeconds, string TokenType = "Bearer");