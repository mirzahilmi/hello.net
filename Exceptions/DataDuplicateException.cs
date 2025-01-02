namespace Hello.NET.Exceptions;

public sealed class DataConflictException : Exception
{
    public DataConflictException() { }

    public DataConflictException(string? message)
        : base(message) { }

    public DataConflictException(string? message, Exception? innerException)
        : base(message, innerException) { }
}
