using FluentValidation;

namespace NadinSoft.Application.Common.Validation;

public class ValidationModelBase<TRequest>: AbstractValidator<TRequest> where TRequest : class
{
    
    
}