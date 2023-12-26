1.1.0
-----

* Removed `QuerySignerExtensions` class

1.0.2
-----

* Wrap exceptions thrown by `IDataProtector` when protecting/unprotecting data in `ProtectorException`s
* Only generate `IDataProtector` once lazily as opposed to every time `Protect`/`Unprotect` is called

1.0.1
-----

* No changes

1.0.0
-----

* Initial release
