namespace cheluan.Models;

public record Result(bool Success, string? Error = null)
{
    public static Result Ok() => new(true);
    public static Result Fail(string error) => new(false, error);

    public bool Failed => !Success;
}

public record Result<T>(bool Success, T? Value, string? Error = null)
    : Result(Success, Error)
{
    public static Result<T> Ok(T value) => new(true, value);

    // We use 'new' here to hide the base Fail and return the correct type
    public new static Result<T> Fail(string error) => new(false, default, error);
}