namespace SignedUrl.Abstractions;

/// <summary>
/// Represents a service that can protect data by encrypting, scrambling, or otherwise obfuscating it.
///
/// In any case, it MUST NOT return the bytes as-is.
/// </summary>
public interface ISignatureProtector
{
    /// <summary>
    /// Protects the give data using implementation specific means and returns the result.
    /// </summary>
    /// <param name="data">The data to protect.</param>
    /// <returns>The protected data.</returns>
    /// <throws><see cref="ProtectorException"/> if the data could not be protected.</throws>
    public byte[] Protect(byte[] data);
}
