using NadinSoft.Application.Common;
using Xunit.Abstractions;

namespace NadinSoft.Application.Test.Extensions;

public static class ApplicationTestsExtensions
{
    public static void WriteLineOperationResultErrors<TResult>(this ITestOutputHelper output,
        OperationResult<TResult> operationResult)
    {
        foreach (var error in operationResult.ErrorMessages)
        {
            output.WriteLine($"Property Name: {error.Key}, Message: {error.Value}");
        }
    }
}