namespace TransactionService.Application.Utils.Exceptions;
public class ValidationError(string message)
{
    public string Message { get; } = message;
    public string? Code { get; }
    public string? Property { get; }
    public ValidationError(string message, string? code) : this(message)
    {
        Code = code;
    }

    public ValidationError(string message, string? code, string? property)
        : this(message, code)
    {
        Property = property;
    }
}
