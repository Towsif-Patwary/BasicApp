namespace Basic.Core.Common;

public class Result<T>
{
    public bool Success { get; set; }
    public T Data { get; set; }
    public string Message { get; set; }
    public List<string> Errors { get; set; }

    public Result()
    {
        Errors = new List<string>();
    }

    public static Result<T> SuccessResult(T data, string message = "")
    {
        return new Result<T>
        {
            Success = true,
            Data = data,
            Message = message
        };
    }

    public static Result<T> FailureResult(List<string> errors, string message = "")
    {
        return new Result<T>
        {
            Success = false,
            Errors = errors,
            Message = message
        };
    }

    public static Result<T> FailureResult(string error, string message = "")
    {
        return new Result<T>
        {
            Success = false,
            Errors = new List<string> { error },
            Message = message
        };
    }
}
