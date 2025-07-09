namespace TransactionService.Application.Utils.Exceptions;
public class ApplicationException : Exception
{
    public ApplicationException(string businessMessage) : base(businessMessage)
    {

    }
}
