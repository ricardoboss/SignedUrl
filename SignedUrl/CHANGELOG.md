1.1.0
-----

* Split `DigestQuerySigner` into `DigestSignatureGenerator` and `QueryStringUrlSigner`

1.0.2
-----

* No changes

1.0.1
-----

* Handle `FormatException`s and `ProtectorException`s in `DigestQuerySigner::ValidateSignature`
* Forward exceptions thrown in `DigestQuerySigner::GenerateSignature` as `SignatureGenerationException`s

1.0.0
-----

* Initial release
