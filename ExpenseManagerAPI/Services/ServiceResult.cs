namespace ExpenseManagerAPI.Services;

public enum ResultCode
{
    Ok = 200,
    NoContent = 204,
    BadRequest = 400,
    Unauthorized = 401,
    NotFound = 404,
    InternalServerError = 500
}

public class ServiceResult<T>
{
    public T? Data { get; set; }
    public string? ErrorMessage { get; set; }
    public ResultCode ResultCode { get; set; }
    
    public static ServiceResult<T> Success(T data, ResultCode resultCode = ResultCode.Ok)
    {
        return new ServiceResult<T>
        {
            Data = data,
            ErrorMessage = null,
            ResultCode = resultCode
        };
    }

    public static ServiceResult<T> Failure(string errorMessage, ResultCode resultCode)
    {
        return new ServiceResult<T>
        {
            Data = default(T),
            ErrorMessage = errorMessage,
            ResultCode = resultCode
        };
    }
}