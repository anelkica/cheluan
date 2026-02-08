namespace cheluan.Models;

// these are softer, less harsh exceptions :3

// only true/false
public record Result(bool success, string? error = null)
{
    public static Result Ok() => new(true);
    public static Result Fail(string error) => new(false, error);

    public bool Failed => !success;
}

public record Result<T>(bool success, T? value, string? error = null)
{
    public static Result<T> Ok(T value) => new(true, value);
    public static Result<T> Fail(string error) => new(false, default, error);

    public bool Failed => !success; // example: if (result.failed) ...
}
