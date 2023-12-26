using System.Security.Cryptography;
using System.Text;
using System.Web;
using SignedUrl.Abstractions;

namespace SignedUrl;

/// <summary>
/// Uses <see cref="SHA256"/> to generate a digest of the query string and then uses <see cref="ISignatureProtector"/>
/// to protect the digest against brute force attacks.
///
/// The <see cref="ISignatureProtector"/> MUST NOT return the bytes as-is, but rather encrypt them.
/// </summary>
/// <param name="signatureProtector">The <see cref="ISignatureProtector"/> to use.</param>
public class DigestQuerySigner(ISignatureProtector signatureProtector) : IQuerySigner
{
	/// <inheritdoc />
	public string GenerateSignature(IDictionary<string, string?>? queryParams = null, string signatureKey = IQuerySigner.DefaultSignatureKey)
	{
		// kinda hacky, but MS doesn't publish the HttpUtility.HttpQSCollection class that contains the ToString() method
		var queryCollection = HttpUtility.ParseQueryString(string.Empty);

		string queryString;
		if (queryParams is { Count: > 0 })
		{
			foreach (var (key, value) in queryParams)
				queryCollection[key] = value;

			queryString = queryCollection.ToString() ?? throw new InvalidOperationException();
		}
		else
			queryString = string.Empty;

		var queryStringBytes = Encoding.UTF8.GetBytes(queryString);
		var digestBytes = SHA256.HashData(queryStringBytes);

		try
		{
			var securedQueryStringBytes = signatureProtector.Protect(digestBytes);
			var digestString = Convert.ToBase64String(securedQueryStringBytes);
			queryCollection[signatureKey] = digestString;

			return queryCollection.ToString() ?? throw new InvalidOperationException();
		}
		catch (ProtectorException e)
		{
			throw new SignatureGenerationException("Failed to generate signature.", e);
		}
	}

	/// <inheritdoc />
	public bool ValidateSignature(string query, string signatureKey = IQuerySigner.DefaultSignatureKey)
	{
		var collection = HttpUtility.ParseQueryString(query);
		if (collection[signatureKey] == null)
			return false;

		var digestString = collection[signatureKey];
		if (digestString is null)
			return false;

		collection.Remove(signatureKey);

		var actualQueryString = collection.ToString()!;
		var actualQueryStringBytes = Encoding.UTF8.GetBytes(actualQueryString);
		var actualDigestBytes = SHA256.HashData(actualQueryStringBytes);

		try
		{
			var givenSecuredDigestBytes = Convert.FromBase64String(digestString);
			var givenDigestBytes = signatureProtector.Unprotect(givenSecuredDigestBytes);

			return actualDigestBytes.SequenceEqual(givenDigestBytes);
		}
		catch (Exception e) when (e is ProtectorException or FormatException)
		{
			return false;
		}
	}
}
