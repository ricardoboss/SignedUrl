namespace SignedUrl.Abstractions;

/// <summary>
/// Thrown when the protector fails to protect or unprotect data.
/// </summary>
public abstract class ProtectorException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProtectorException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    protected ProtectorException(string? message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProtectorException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    protected ProtectorException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}