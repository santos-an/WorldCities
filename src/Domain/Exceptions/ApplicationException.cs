namespace Domain.Exceptions;

public abstract class ApplicationException : Exception
{
    protected ApplicationException(string title, string message) : base(message)
    {
        Title = title;
        Message = message;
    }

    protected ApplicationException(string title, string message, Exception inner) : base(message, inner)
    {
        Title = title;
        Message = message;
    }

    public string Title { get; }
    public string Message { get; }
}