using FluentValidation.Results;

namespace TransactionService.Application.Utils.Exceptions;
public class ValidationException : ApplicationException
{
    internal ValidationException() : base("One or more validation failures have occurred.")
    {
        Errors = new List<ValidationError>();
    }

    public ValidationException(IEnumerable<ValidationFailure> failures) : this()
    {
        Errors = failures
            .Select(failure => new ValidationError(failure.ErrorMessage, failure.ErrorCode, failure.PropertyName));
    }

    public IEnumerable<ValidationError> Errors { get; }
}
