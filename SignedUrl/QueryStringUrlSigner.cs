using System.Web;
using SignedUrl.Abstractions;

namespace SignedUrl;

public class QueryStringUrlSigner(ISignatureGenerator generator, ISignatureProtector protector, string queryKey = "s") : IUrlSigner
{
    /// <inheritdoc />
    public string Sign(string url)
    {
        var builder = new UriBuilder(url);
        var queryCollection = HttpUtility.ParseQueryString(builder.Query);

        var queryDictionary = queryCollection.Keys.Cast<string>().ToDictionary(key => key, key => queryCollection[key]);
        var signature = generator.GenerateSignature(queryDictionary);
        var protectedSignature = protector.Protect(signature);

        queryCollection[queryKey] = Convert.ToBase64String(protectedSignature);

        builder.Query = queryCollection.ToString() ?? throw new InvalidOperationException();

        return builder.ToString();
    }

    /// <inheritdoc />
    public bool Validate(string url)
    {
        var builder = new UriBuilder(url);
        var queryCollection = HttpUtility.ParseQueryString(builder.Query);

        var signatureString = queryCollection[queryKey];
        if (string.IsNullOrWhiteSpace(signatureString))
            return false;

        queryCollection.Remove(queryKey);

        var queryDictionary = queryCollection.Keys.Cast<string>().ToDictionary(key => key, key => queryCollection[key]);
        var expectedSignature = generator.GenerateSignature(queryDictionary);

        var actualProtectedSignature = Convert.FromBase64String(signatureString);
        var actualSignature = protector.Unprotect(actualProtectedSignature);

        return expectedSignature.SequenceEqual(actualSignature);
    }
}
