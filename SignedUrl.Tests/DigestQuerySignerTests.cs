using System.Globalization;
using SignedUrl.Abstractions;

namespace SignedUrl.Tests;

public class DigestQuerySignerTests
{
    [Theory]
    [ClassData(typeof(SignEndpointDataProvider))]
    public void TestGenerateSignature(Dictionary<string, string?>? queryParams, string expected)
    {
        var protector = MockProtector(protect: true);
        var signer = new DigestQuerySigner(protector.Object);
        var actual = signer.GenerateSignature(queryParams);

        Assert.Equal(expected, actual);
        protector.VerifyAll();
    }

    private sealed class SignEndpointDataProvider : TheoryData<Dictionary<string, string?>?, string>
    {
        public SignEndpointDataProvider()
        {
            Add(null, "s=47DEQpj8HBSa%2b%2fTImW%2b5JCeuQeRkm5NMpJWZG3hSuFU%3d");
            Add(new() { { "a", "b" } }, "a=b&s=QhRPOTnD%2f7vwv4sfEq%2f7XCOkxb1B4P9nLVSldU8GIFg%3d");
            Add(new() { { "a", "b" }, { "c", "d" } }, "a=b&c=d&s=cDggzKzLYLt6FWPW5nNrzCYcjAYfjYiWEDzM9r%2fkHjM%3d");
        }
    }

    [Theory]
    [ClassData(typeof(ValidateSignatureDataProvider))]
    public void TestValidateSignature(string query, bool expected)
    {
        var protector = MockProtector(unprotect: true);
        var signer = new DigestQuerySigner(protector.Object);
        var actual = signer.ValidateSignature(query);

        Assert.Equal(expected, actual);
        protector.VerifyAll();
    }

    private sealed class ValidateSignatureDataProvider : TheoryData<string, bool>
    {
        public ValidateSignatureDataProvider()
        {
            Add("s=47DEQpj8HBSa%2b%2fTImW%2b5JCeuQeRkm5NMpJWZG3hSuFU%3d", true);
            Add("a=b&s=QhRPOTnD%2f7vwv4sfEq%2f7XCOkxb1B4P9nLVSldU8GIFg%3d", true);
            Add("a=b&c=d&s=cDggzKzLYLt6FWPW5nNrzCYcjAYfjYiWEDzM9r%2fkHjM%3d", true);
        }
    }

    private static Mock<ISignatureProtector> MockProtector(bool protect = false, bool unprotect = false)
    {
        var protectorMock = new Mock<ISignatureProtector>();

        if (protect)
        {
            protectorMock
                .Setup(p => p.Protect(It.IsAny<byte[]>()))
                .Returns((byte[] bytes) => bytes);
        }

        if (unprotect)
        {
            protectorMock
                .Setup(p => p.Unprotect(It.IsAny<byte[]>()))
                .Returns((byte[] bytes) => bytes);
        }

        return protectorMock;
    }
}
