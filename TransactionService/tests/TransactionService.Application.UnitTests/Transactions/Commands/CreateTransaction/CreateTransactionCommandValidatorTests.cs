using FluentValidation.TestHelper;
using TransactionService.Application.Transactions.Commands.CreateTransaction;
using TransactionService.Application.Transactions.Models;

namespace TransactionService.Application.UnitTests.Transactions.Commands.CreateTransaction;
public class CreateTransactionCommandValidatorTests
{
    private readonly CreateTransactionCommandValidator _validator;
    public CreateTransactionCommandValidatorTests()
    {
        _validator = new CreateTransactionCommandValidator();
    }

    [Fact]
    public async Task Transaction_WhenAllValueAreCorrect_ShouldSucceed()
    {
        // Arrange
        var command = new CreateTransactionCommand(new CreateTransactionRequest
        {
            SourceAccountId = Guid.NewGuid(),
            TargetAccountId = Guid.NewGuid(),
            Value = 100.00m
        });

        // Act
        var result = await _validator.TestValidateAsync(command, opt => opt.IncludeProperties(p => p.Transaction.Value));

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task Transaction_WhenTransactionValueIsWrong_ShouldReturnError()
    {
        // Arrange
        var command = new CreateTransactionCommand(new CreateTransactionRequest
        {
            SourceAccountId = Guid.NewGuid(),
            TargetAccountId = Guid.NewGuid(),
            Value = 0.00m
        });

        // Act
        var result = await _validator.TestValidateAsync(command, opt => opt.IncludeProperties(p => p.Transaction.Value));

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.Transaction.Value)
            .WithErrorCode($"{nameof(CreateTransactionCommand)}-{nameof(CreateTransactionCommand.Transaction.Value)}")
            .WithErrorMessage("Transaction value should be greater than 0.");
    }

    [Fact]
    public async Task Transaction_WhenTransactionSourceAccountIdIsEmptyGuid_ShouldReturnError()
    {
        // Arrange
        var command = new CreateTransactionCommand(new CreateTransactionRequest
        {
            SourceAccountId = Guid.Empty,
            TargetAccountId = Guid.NewGuid(),
            Value = 100.00m
        });

        // Act
        var result = await _validator.TestValidateAsync(command, opt => opt.IncludeProperties(p => p.Transaction.SourceAccountId));

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.Transaction.SourceAccountId)
            .WithErrorCode($"{nameof(CreateTransactionCommand)}-{nameof(CreateTransactionCommand.Transaction.SourceAccountId)}")
            .WithErrorMessage("Source Accound Id should not be empty.");
    }

    [Fact]
    public async Task Transaction_WhenTransactionTargetAccountIdEmptyGuid_ShouldReturnError()
    {
        // Arrange
        var command = new CreateTransactionCommand(new CreateTransactionRequest
        {
            SourceAccountId = Guid.NewGuid(),
            TargetAccountId = Guid.Empty,
            Value = 100.00m
        });

        // Act
        var result = await _validator.TestValidateAsync(command, opt => opt.IncludeProperties(p => p.Transaction.TargetAccountId));

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.Transaction.TargetAccountId)
            .WithErrorCode($"{nameof(CreateTransactionCommand)}-{nameof(CreateTransactionCommand.Transaction.SourceAccountId)}")
            .WithErrorMessage("Target Account Id should not be empty.");
    }
}
