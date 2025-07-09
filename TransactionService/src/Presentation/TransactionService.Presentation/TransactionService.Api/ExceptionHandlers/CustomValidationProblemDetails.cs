using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using TransactionService.Application.Utils.Exceptions;

namespace TransactionService.Api.ExceptionHandlers;

public class CustomValidationProblemDetails : ValidationProblemDetails
{
    internal CustomValidationProblemDetails()
        : this(new List<ValidationError>())
    {
    }

    internal CustomValidationProblemDetails(ValidationError validationError)
        : this(new List<ValidationError> { validationError })
    {
    }

    internal CustomValidationProblemDetails(IEnumerable<ValidationError> errors)
    {
        Errors = errors;
    }

    [JsonPropertyName("errors")]
    public new IEnumerable<ValidationError> Errors { get; } = new List<ValidationError>();
}
