namespace cheluan.Models;

// these are softer, less harsh exceptions :3

// only true/false
public record Result(bool Success, string? Error = null)
{
    public static Result Ok() => new(true);
    public static Result Fail(string Error) => new(false, Error);

    public bool Failed => !Success;
}

public record Result<T>(bool Success, T? Value, string? Error = null)
{
    public static Result<T> Ok(T Value) => new(true, Value);
    public static Result<T> Fail(string Error) => new(false, default, Error);

    public bool Failed => !Success; // example: if (result.failed) ...
}
