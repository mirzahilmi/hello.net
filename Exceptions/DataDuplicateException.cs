namespace Hello.NET.Exceptions;

public sealed class DataDuplicateException : Exception
{
    public DataDuplicateException() { }

    public DataDuplicateException(string? message)
        : base(message) { }

    public DataDuplicateException(string? message, Exception? innerException)
        : base(message, innerException) { }
}
