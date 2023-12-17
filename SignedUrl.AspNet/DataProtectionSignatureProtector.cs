using Microsoft.AspNetCore.DataProtection;
using SignedUrl.Abstractions;

namespace SignedUrl.AspNet;

/// <summary>
/// Implements <see cref="ISignatureProtector"/> using <see cref="IDataProtectionProvider"/>.
/// </summary>
/// <param name="dataProtectionProvider">The <see cref="IDataProtectionProvider"/> to use.</param>
public class DataProtectionSignatureProtector(IDataProtectionProvider dataProtectionProvider) : ISignatureProtector
{
    private IDataProtector Protector => dataProtectionProvider.CreateProtector(nameof(DataProtectionSignatureProtector));

    /// <inheritdoc />
    public byte[] Protect(byte[] data) => Protector.Protect(data);

    /// <inheritdoc />
    public byte[] Unprotect(byte[] data) => Protector.Unprotect(data);
}
