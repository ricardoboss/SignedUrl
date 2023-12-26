using System.Security.Cryptography;
using System.Text;
using System.Web;
using SignedUrl.Abstractions;

namespace SignedUrl;

/// <summary>
/// Uses <see cref="SHA256"/> to generate a digest of the supplied data.
/// </summary>
public class DigestSignatureGenerator() : ISignatureGenerator
{
    /// <inheritdoc />
    public byte[] GenerateSignature(IDictionary<string, string?> data)
    {
        // kinda hacky, but MS doesn't publish the HttpUtility.HttpQSCollection class that contains the ToString() method
        var queryCollection = HttpUtility.ParseQueryString(string.Empty);

        string queryString;
        if (data is { Count: > 0 })
        {
            foreach (var (key, value) in data)
                queryCollection[key] = value;

            queryString = queryCollection.ToString() ?? throw new InvalidOperationException();
        }
        else
            queryString = string.Empty;

        var queryStringBytes = Encoding.UTF8.GetBytes(queryString);

        return SHA256.HashData(queryStringBytes);
    }
}
