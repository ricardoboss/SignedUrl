using SignedUrl.Abstractions;

namespace SignedUrl.Tests;

public class DigestSignatureGeneratorTests
{
    [Theory]
    [ClassData(typeof(GenerateSignatureDataProvider))]
    public void GenerateSignatureTest(Dictionary<string, string?> data, string expected)
    {
        var protector = MockProtector();
        var generator = new DigestSignatureGenerator(protector.Object);
        var actual = generator.GenerateSignature(data);

        Assert.Equal(expected, actual);
    }

    private static Mock<ISignatureProtector> MockProtector()
    {
        var protectorMock = new Mock<ISignatureProtector>();

        protectorMock
            .Setup(p => p.Protect(It.IsAny<byte[]>()))
            .Returns((byte[] bytes) => bytes);

        return protectorMock;
    }

    private sealed class GenerateSignatureDataProvider : TheoryData<Dictionary<string, string?>, string>
    {
        public GenerateSignatureDataProvider()
        {
            Add(new(), "47DEQpj8HBSa+/TImW+5JCeuQeRkm5NMpJWZG3hSuFU=");
            Add(new() { { "a", "b" } }, "QhRPOTnD/7vwv4sfEq/7XCOkxb1B4P9nLVSldU8GIFg=");
            Add(new() { { "a", "b" }, { "c", "d" } }, "cDggzKzLYLt6FWPW5nNrzCYcjAYfjYiWEDzM9r/kHjM=");
        }
    }
}
