using System.Text.Json.Serialization;

namespace TransactionService.Domain.Transaction.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TransactionStatus
{
    Pending,
    Approved,
    Rejected
}
