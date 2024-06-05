using UniPass.Infrastructure.Models;

namespace UniPass.Infrastructure.ViewModels;

public class Operation<T>
{
    private Operation(string message, bool success)
    {
        Message = message;
        Success = success;
    }

    private Operation(string message, T result, bool success)
    {
        Message = message;
        Value = result;
        Success = success;
    }

    public Operation()
    {
    }

    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Value { get; set; }

    public static Operation<T> Error(string message)
    {
        return new Operation<T>(message, false);
    }
    
    public static Operation<T> Error(T value, string message)
    {
        return new Operation<T>
        {
            Value = value,
            Message = message
        };
    }

    public static Operation<T> Result(T result, string message = "", bool success = true)
    {
        return new Operation<T>(message, result, success);
    }
    
    // public static Operation Result(Key result, string message = "", bool success = true)
    // {
    //     return new Operation<Key>(message, result, success);
    // }
    //
    public static Operation<T> Result(string message = "", bool success = true)
    {
        return new Operation<T>(message, success);
    }
}