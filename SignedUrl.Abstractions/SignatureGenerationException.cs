namespace SignedUrl.Abstractions;

/// <summary>
/// Thrown when an error occurs while generating a signature.
/// </summary>
public class SignatureGenerationException : Exception
{
    /// <summary>
    /// Creates a new <see cref="SignatureGenerationException"/> with the given message.
    /// </summary>
    /// <param name="message">The message to use.</param>
    public SignatureGenerationException(string message) : base(message)
    {
    }

    /// <summary>
    /// Creates a new <see cref="SignatureGenerationException"/> with the given message and inner exception.
    /// </summary>
    /// <param name="message">The message to use.</param>
    /// <param name="innerException">The inner exception to use.</param>
    public SignatureGenerationException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
