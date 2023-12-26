namespace SignedUrl.Abstractions;

/// <summary>
/// Represents a service that can protect data by encrypting, scrambling, or otherwise obfuscating it but still being
/// able to decrypt, unscramble, or otherwise deobfuscate it.
///
/// In any case, it MUST NOT return the bytes as-is.
/// </summary>
public interface ISignatureProtector
{
    /// <summary>
    /// Protects the give data using implementation specific means and returns the result.
    ///
    /// The result is then used as the signature for the URL (base64 encoded).
    /// </summary>
    /// <param name="data">The data to protect.</param>
    /// <returns>The protected data.</returns>
    /// <throws><see cref="ProtectorException"/> if the data could not be protected.</throws>
    public byte[] Protect(byte[] data);

    /// <summary>
    /// Unprotects the given data using implementation specific means and returns the result.
    ///
    /// The result is compared to the signature given in the URL.
    /// </summary>
    /// <param name="data">The data to unprotect.</param>
    /// <returns>The unprotected data.</returns>
    /// <throws><see cref="ProtectorException"/> if the data could not be unprotected.</throws>
    public byte[] Unprotect(byte[] data);
}
