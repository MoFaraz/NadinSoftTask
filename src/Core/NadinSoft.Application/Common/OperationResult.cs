namespace NadinSoft.Application.Common;

public interface IOperationResult
{
    bool IsSuccess { get; set; }
    bool IsNotFound { get; set; }
    List<KeyValuePair<string, string>> ErrorMessages { get; set; }
}

public class OperationResult<TResult>: IOperationResult
{
    public TResult? Result { get; set; }
    public bool IsSuccess { get; set; }
    public bool IsNotFound { get; set; }
    public List<KeyValuePair<string, string>> ErrorMessages { get; set; } = [];

    public static OperationResult<TResult> SuccessResult(TResult result)
    {
        return new OperationResult<TResult>
        {
            Result = result,
            IsSuccess = true
        };
    }

    public static OperationResult<TResult> FailureResult(string propertyName, string message)
    {
        var result = new OperationResult<TResult>
        {
            Result = default,
            IsSuccess = false,
        };
        result.ErrorMessages.Add(new KeyValuePair<string, string>(propertyName, message));
        return result;
    }
    
    public static OperationResult<TResult> FailureResult(List<KeyValuePair<string, string>> errorMessages)
    {
        return new OperationResult<TResult>
        {
            Result = default,
            IsSuccess = false,
            ErrorMessages = errorMessages
        };
    }


    public static OperationResult<TResult> NotFoundResult(string propertyName, string message)
    {
        var result = new OperationResult<TResult>
        {
            Result = default,
            IsSuccess = false,
            IsNotFound = true,
        };
        result.ErrorMessages.Add(new KeyValuePair<string, string>(propertyName, message));
        return result;
    }
}