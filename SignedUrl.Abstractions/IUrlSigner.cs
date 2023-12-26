namespace SignedUrl.Abstractions;

/// <summary>
/// Provides methods for signing and validating signed URLs.
/// </summary>
public interface IUrlSigner
{
    /// <summary>
    /// Adds a signature to a given url.
    /// </summary>
    /// <param name="url">The key-value-pairs to include in the query (apart from the signature).</param>
    /// <returns>
    /// The given <paramref name="url"/> appended with a signature.
    /// </returns>
    /// <throws cref="SignatureGenerationException">If the signature could not be generated.</throws>
    string Sign(string url);

    /// <summary>
    /// Validates the signature of a url.
    /// </summary>
    /// <param name="url">The url to validate.</param>
    /// <returns><c>true</c> if the signature is valid; otherwise, <c>false</c>.</returns>
    bool Validate(string url);
}
