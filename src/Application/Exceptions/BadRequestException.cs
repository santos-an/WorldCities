using ApplicationException = Domain.Exceptions.ApplicationException;

namespace Application.Exceptions;

public class BadRequestException : ApplicationException
{
    public BadRequestException(string title, string message, Exception inner) : base(title, message, inner) { }
}