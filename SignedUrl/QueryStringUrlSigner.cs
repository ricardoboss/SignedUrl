using System.Web;
using SignedUrl.Abstractions;

namespace SignedUrl;

public class QueryStringUrlSigner(ISignatureGenerator generator, string queryKey = "s") : IUrlSigner
{
    /// <inheritdoc />
    public string Sign(string url)
    {
        var builder = new UriBuilder(url);
        var queryCollection = HttpUtility.ParseQueryString(builder.Query);

        var queryDictionary = queryCollection.Keys.Cast<string>().ToDictionary(key => key, key => queryCollection[key]);
        var signature = generator.GenerateSignature(queryDictionary);

        queryCollection[queryKey] = signature;

        builder.Query = queryCollection.ToString() ?? throw new InvalidOperationException();

        return builder.ToString();
    }

    /// <inheritdoc />
    public bool Validate(string url)
    {
        var builder = new UriBuilder(url);
        var queryCollection = HttpUtility.ParseQueryString(builder.Query);
        var signature = queryCollection[queryKey];
        if (string.IsNullOrWhiteSpace(signature))
            return false;

        queryCollection.Remove(queryKey);

        var queryDictionary = queryCollection.Keys.Cast<string>().ToDictionary(key => key, key => queryCollection[key]);
        var expectedSignature = generator.GenerateSignature(queryDictionary);

        return string.Equals(expectedSignature, signature, StringComparison.Ordinal);
    }
}