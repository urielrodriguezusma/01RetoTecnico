namespace AntifraudService.Domain;
public static class Globals
{
    #region MassTranssit
    public const string ServiceName = "AntifraudService";
    public const char Separator = '.';
    #endregion
    #region Transactions
    public const decimal LimitPerDay = 20_000;
    public const decimal LimitTransactionValue = 2_000;
    #endregion


}
