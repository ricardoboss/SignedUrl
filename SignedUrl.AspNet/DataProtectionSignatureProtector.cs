using Microsoft.AspNetCore.DataProtection;
using SignedUrl.Abstractions;

namespace SignedUrl.AspNet;

/// <summary>
/// Implements <see cref="ISignatureProtector"/> using <see cref="IDataProtectionProvider"/>.
/// </summary>
/// <param name="dataProtectionProvider">The <see cref="IDataProtectionProvider"/> to use.</param>
public class DataProtectionSignatureProtector(IDataProtectionProvider dataProtectionProvider) : ISignatureProtector
{
    private readonly Lazy<IDataProtector> lazyProtector =
        new(() => dataProtectionProvider.CreateProtector(nameof(DataProtectionSignatureProtector)));

    private IDataProtector Protector => lazyProtector.Value;

    /// <inheritdoc />
    public byte[] Protect(byte[] data)
    {
        try
        {
            return Protector.Protect(data);
        }
        catch (Exception e)
        {
            throw new ProtectorException("Failed to protect data.", e);
        }
    }

    /// <inheritdoc />
    public byte[] Unprotect(byte[] data)
    {
        try
        {
            return Protector.Unprotect(data);
        }
        catch (Exception e)
        {
            throw new ProtectorException("Failed to unprotect data.", e);
        }
    }
}