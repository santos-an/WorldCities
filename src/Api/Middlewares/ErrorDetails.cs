namespace Api.Middlewares;

public class ErrorDetails
{
    public string Title { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public IReadOnlyDictionary<string, string[]> Errors { get; set; }
}