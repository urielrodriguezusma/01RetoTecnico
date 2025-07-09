namespace TransactionService.Domain.UniTests.Transaction.Entities;
public class TransactionTests
{
    [Fact]
    public void CreateTransaction_WhenDataIsValid_ShouldSucceed()
    {
        // Arrange
        Guid sourceAccountId = Guid.NewGuid();
        Guid targetAccountId = Guid.NewGuid();
        decimal value = 10_000;

        var transaction = Domain.Transaction.Entities.Transaction.Create(
            sourceAccountId,
            targetAccountId,
            value);

        // assert
        transaction.SourceAccountId.Should().Be(sourceAccountId);
        transaction.TargetAccountId.Should().Be(targetAccountId);
        transaction.Value.Should().Be(value);
    }

    [Fact]
    public void CreateTransaction_WhenValueIsWrong_ShouldThrowArgumentException()
    {
        // Arrange
        Guid sourceAccountId = Guid.NewGuid();
        Guid targetAccountId = Guid.NewGuid();
        decimal value = 0;

        var exception = Record.Exception(() => Domain.Transaction.Entities.Transaction.Create(
            sourceAccountId,
            targetAccountId,
            value));

        // assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentException>()
            .Which.Message.Should().Contain("Value must be greater than zero.");
    }
}
