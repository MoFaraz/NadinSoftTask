using System.Diagnostics.CodeAnalysis;
using FluentValidation.Results;

namespace NadinSoft.Application.Extensions;

public static class ApplicationValidationExtensions
{
    public static List<KeyValuePair<string, string>> ConvertToKeyValuePairs(
        [NotNull]this List<ValidationFailure> failures)
    {
      return failures.Select(f => new KeyValuePair<string, string>(f.PropertyName, f.ErrorMessage)).ToList();  
    }
}