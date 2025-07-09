using FluentValidation;
using Mediator;
using NadinSoft.Application.Common;
using NadinSoft.Application.Common.Validation;
using NadinSoft.Application.Contracts.User.Models;

namespace NadinSoft.Application.Features.User.Commands.Queries.PasswordLogin;

public record UserPasswordLoginQuery(string UserNameOrEmail, string Password): IRequest<OperationResult<JwtAccessTokenModel>>, IValidatableModel<UserPasswordLoginQuery>
{
    public IValidator<UserPasswordLoginQuery> Validate(ValidationModelBase<UserPasswordLoginQuery> validator)
    {
      validator.RuleFor(c => c.UserNameOrEmail).NotEmpty();
      validator.RuleFor(c => c.Password).NotEmpty();
      return validator;
    }
}