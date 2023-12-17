# SignedUrl

A NuGet package that generates signed query strings.

[![NuGet](https://img.shields.io/nuget/v/SignedUrl.svg)](https://www.nuget.org/packages/SignedUrl/)

## Usage

> Currently, only ASP.NET projects are supported. If you can't use, you need to implement `ISignatureProtector`
> yourself.  
> The implementation MUST NOT return the bytes as is, as this would make your signature prone to brute force attacks.

Install two NuGet packages:
- `SignedUrl` and
- `SignedUrl.AspNet`

```csharp
// use dependency injection
builder.Services.AddSingleton<ISignatureProtector, DataProtectionSignatureProtector>();
builder.Services.AddSingleton<IQuerySigner, DigestQuerySigner>();
```

Then consume the `IQuerySigner`:

```csharp
public class MyUrlService(IQuerySigner signer) {
    public string GenerateSomeUrl() {
        var query = new Dictionary<string, string?>
        {
            { "time", CurrentUnixTimestamp().ToString() },
        };

        return "https://example.com/?" + signer.GenerateSignature(query);
    }
}
```

The above class will generate a URL like this:

```
https://example.com/?time=1702855023&s=vJ89aMd%2bkkZaLamPOKuCgGAiuDDRqn7XtIdM%2bXpCYZw%3d
```

Observations:
- The dictionary containing a key `time` and a value of the current Unix timestamp is appended to the query string
- The `s` parameter is the signature of the query string

And to verify the signature:

```csharp
public class MyUrlService(IQuerySigner signer) {
    // [...omitted for brevity...]

    public bool VerifySomeUrl(string url) {
        var query = HttpUtility.ParseQueryString(new Uri(url).Query);

        return signer.VerifySignature(query);
    }
}
```

## Development

### Pre-requisites

- .NET 8

### Extensions

If you want to write extensions, you can use the `SignedUrl.Abstractions` package, which contains the interfaces used
by the `SignedUrl` implementations.

### Testing

The project uses `xUnit` for testing. To run the tests, run `dotnet test` in the root directory.

### Publishing

There are 3 NuGet packages that need to be published:

- `SignedUrl`
- `SignedUrl.Abstractions`
- `SignedUrl.AspNet`

Make sure all the versions are the same. Then, run `dotnet pack` in each project directory.

## License

Licensed under [MIT](LICENSE).
