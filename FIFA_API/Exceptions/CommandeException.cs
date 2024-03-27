using System.Runtime.Serialization;

namespace FIFA_API.Exceptions
{
    public enum CommandeExceptionCause
    {
        NoVariante = 1,
        NoTaille = 2,
        NoStocks = 3,
        StocksEmpty = 4
    }

    public class CommandeException : Exception
    {
        public readonly CommandeExceptionCause Cause;

        public CommandeException(CommandeExceptionCause cause, string? message) : base(message)
        {
            Cause = cause;
        }
    }
}
