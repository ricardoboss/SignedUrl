namespace SignedUrl.Abstractions;

/// <summary>
/// Generates a signature for a dictionary of data.
/// </summary>
public interface ISignatureGenerator
{
    /// <summary>
    /// Generates a signature for the given data.
    /// </summary>
    /// <param name="data">The parameters to generate a signature for.</param>
    /// <returns>The generated signature.</returns>
    string GenerateSignature(IDictionary<string, string?> data);
}
