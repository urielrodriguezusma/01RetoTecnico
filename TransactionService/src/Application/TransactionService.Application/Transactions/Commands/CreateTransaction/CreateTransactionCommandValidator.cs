using FluentValidation;

namespace TransactionService.Application.Transactions.Commands.CreateTransaction;
public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        RuleFor(d => d.Transaction.SourceAccountId)
            .NotEmpty()
            .WithErrorCode($"{nameof(CreateTransactionCommand)}-{nameof(CreateTransactionCommand.Transaction.SourceAccountId)}")
            .WithMessage("Source Accound Id should not be empty.");

        RuleFor(d => d.Transaction.TargetAccountId)
            .NotEmpty()
            .WithErrorCode($"{nameof(CreateTransactionCommand)}-{nameof(CreateTransactionCommand.Transaction.SourceAccountId)}")
            .WithMessage("Target Account Id should not be empty.");

        RuleFor(d => d.Transaction.Value)
            .GreaterThan(0)
            .WithErrorCode($"{nameof(CreateTransactionCommand)}-{nameof(CreateTransactionCommand.Transaction.Value)}")
            .WithMessage("Transaction value should be greater than 0.");
    }
}
