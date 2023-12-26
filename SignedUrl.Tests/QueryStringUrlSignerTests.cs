using System.Security.Cryptography;
using System.Text;
using SignedUrl.Abstractions;

namespace SignedUrl.Tests;

public class QueryStringUrlSignerTests
{
    [Theory]
    [ClassData(typeof(SignDataProvider))]
    public void TestSign(string url, string expected)
    {
        var generator = MockGenerator();
        var protector = MockProtector(protect: true);
        var signer = new QueryStringUrlSigner(generator.Object, protector.Object);
        var actual = signer.Sign(url);

        Assert.Equal(expected, actual);
        generator.VerifyAll();
        protector.VerifyAll();
    }

    private sealed class SignDataProvider : TheoryData<string, string>
    {
        public SignDataProvider()
        {
            Add("https://example.com", "https://example.com:443/?s=47DEQpj8HBSa%2b%2fTImW%2b5JCeuQeRkm5NMpJWZG3hSuFU%3d");
            Add("https://example.com?a=b", "https://example.com:443/?a=b&s=QhRPOTnD%2f7vwv4sfEq%2f7XCOkxb1B4P9nLVSldU8GIFg%3d");
            Add("https://example.com?a=b&c=d", "https://example.com:443/?a=b&c=d&s=cDggzKzLYLt6FWPW5nNrzCYcjAYfjYiWEDzM9r%2fkHjM%3d");
        }
    }

    [Theory]
    [ClassData(typeof(ValidateDataProvider))]
    public void TestValidate(string url, bool expected)
    {
        var generator = MockGenerator();
        var protector = MockProtector(unprotect: true);
        var signer = new QueryStringUrlSigner(generator.Object, protector.Object);
        var actual = signer.Validate(url);

        Assert.Equal(expected, actual);
    }

    private sealed class ValidateDataProvider : TheoryData<string, bool>
    {
        public ValidateDataProvider()
        {
            Add("https://example.com?s=47DEQpj8HBSa%2b%2fTImW%2b5JCeuQeRkm5NMpJWZG3hSuFU%3d", true);
            Add("https://example.com?a=b&s=QhRPOTnD%2f7vwv4sfEq%2f7XCOkxb1B4P9nLVSldU8GIFg%3d", true);
            Add("https://example.com?a=b&c=d&s=cDggzKzLYLt6FWPW5nNrzCYcjAYfjYiWEDzM9r%2fkHjM%3d", true);
            Add("https://example.com", false);
            Add("https://example.com?d=47DEQpj8HBSa%2b%2fTImW%2b5JCeuQeRkm5NMpJWZG3hSuFU%3d", false);
            Add("https://example.com?s=57DEQpj8HBSa%2b%2fTImW%2b5JCeuQeRkm5NMpJWZG3hSuFU%3d", false);
            Add("https://example.com?a=c&s=QhRPOTnD%2f7vwv4sfEq%2f7XCOkxb1B4P9nLVSldU8GIFg%3d", false);
            Add("https://example.com?a=c&c=e&s=cDggzKzLYLt6FWPW5nNrzCYcjAYfjYiWEDzM9r%2fkHjM%3d", false);
        }
    }

    private static Mock<ISignatureGenerator> MockGenerator()
    {
        var generatorMock = new Mock<ISignatureGenerator>();

        generatorMock
            .Setup(g => g.GenerateSignature(It.IsAny<IDictionary<string, string?>>()))
            .Returns((IDictionary<string, string?> d) =>
            {
                var builder = new StringBuilder();
                foreach (var (key, value) in d.OrderBy(kvp => kvp.Key, StringComparer.Ordinal))
                {
                    builder.Append(key);
                    builder.Append('=');
                    builder.Append(value);
                    builder.Append('&');
                }

                if (builder.Length > 0)
                    builder.Length--;

                var queryString = builder.ToString();
                var queryStringBytes = Encoding.UTF8.GetBytes(queryString);
                return SHA256.HashData(queryStringBytes);
            });

        return generatorMock;
    }

    private static Mock<ISignatureProtector> MockProtector(bool protect = false, bool unprotect = false)
    {
        var protectorMock = new Mock<ISignatureProtector>();

        if (protect)
        {
            protectorMock
                .Setup(p => p.Protect(It.IsAny<byte[]>()))
                .Returns((byte[] data) => data);
        }

        if (unprotect)
        {
            protectorMock
                .Setup(p => p.Unprotect(It.IsAny<byte[]>()))
                .Returns((byte[] data) => data);
        }

        return protectorMock;
    }
}
