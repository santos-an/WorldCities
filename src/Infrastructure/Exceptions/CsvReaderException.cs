using ApplicationException = Domain.Exceptions.ApplicationException;

namespace Infrastructure.Exceptions;

public class CsvReaderException : ApplicationException
{
    public CsvReaderException(string title, string message) : base(title, message)
    {
    }

    public CsvReaderException(string title, string message, Exception inner) : base(title, message, inner)
    {
    }
}