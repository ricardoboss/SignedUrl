namespace SignedUrl.Tests;

public class DigestSignatureGeneratorTests
{
    [Theory]
    [ClassData(typeof(GenerateSignatureDataProvider))]
    public void GenerateSignatureTest(Dictionary<string, string?> data, string expected)
    {
        var generator = new DigestSignatureGenerator();
        var actual = generator.GenerateSignature(data);

        Assert.Equal(expected, Convert.ToBase64String(actual));
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
