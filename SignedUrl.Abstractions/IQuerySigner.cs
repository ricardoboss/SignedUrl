namespace SignedUrl.Abstractions;

/// <summary>
/// Provides methods for signing and validating signed URLs.
/// </summary>
public interface IQuerySigner
{
    /// <summary>
    /// The default key to append to the query string to hold the signature.
    /// </summary>
    const string DefaultSignatureKey = "s";

    /// <summary>
    /// Generates a query string for the given query parameters, including a digest signature that can be verified using
    /// <see cref="ValidateSignature"/>.
    /// </summary>
    /// <param name="queryParams">The key-value-pairs to include in the query (apart from the digest).</param>
    /// <param name="signatureKey">The key to use for the digest in the query string.</param>
    /// <returns>
    /// A fully qualified URL for the given query and query parameters including a digest signature.
    /// </returns>
    string GenerateSignature(IDictionary<string, string?>? queryParams = null, string signatureKey = DefaultSignatureKey);

    /// <summary>
    /// Validates the signature of a query string.
    /// </summary>
    /// <param name="query">The query string to validate.</param>
    /// <param name="signatureKey">The key to use for the digest in the query string.</param>
    /// <returns><c>true</c> if the signature is valid; otherwise, <c>false</c>.</returns>
    bool ValidateSignature(string query, string signatureKey = DefaultSignatureKey);
}
