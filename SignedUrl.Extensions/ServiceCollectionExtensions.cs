using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SignedUrl.Abstractions;
using SignedUrl.AspNet;

namespace SignedUrl.Extensions;

/// <summary>
/// Extension methods for <see cref="IServiceCollection" />.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// <para>
    /// Adds signed URL services to the specified <see cref="IServiceCollection" />.
    /// </para>
    /// <list type="bullet">
    /// <item><description><see cref="ISignatureProtector" /> is registered as a singleton and uses <see cref="DataProtectionSignatureProtector" />.</description></item>
    /// <item><description><see cref="ISignatureGenerator" /> is registered as a singleton and uses <see cref="DigestSignatureGenerator" />.</description></item>
    /// <item><description><see cref="IUrlSigner" /> is registered as a singleton and uses <see cref="QueryStringUrlSigner" />.</description></item>
    /// </list>
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <param name="queryKeyProvider">An optional function that returns the query key to use for the signature.</param>
    /// <returns>The <see cref="IServiceCollection" /> so that additional calls can be chained.</returns>
    public static IServiceCollection AddSignedUrl(this IServiceCollection services, Func<string>? queryKeyProvider = null)
    {
        services.TryAddSingleton<ISignatureProtector, DataProtectionSignatureProtector>();
        services.TryAddSingleton<ISignatureGenerator, DigestSignatureGenerator>();

        services.AddSingleton<IUrlSigner, QueryStringUrlSigner>(sp =>
        {
            var generator = sp.GetRequiredService<ISignatureGenerator>();
            var protector = sp.GetRequiredService<ISignatureProtector>();

            return new(generator, protector, queryKeyProvider?.Invoke() ?? "s");
        });

        return services;
    }
}
