
# Migration guide

There are breaking changes for every major release relative to its previous release. 
This is a detailed documentation on what's changed in each major version, and guidelines for how to adapt your code.

## Migration from Composer 1.4 to ComposerCore 2.x

* Package name changed and unified
  * 'Appson.Composer.Base' is changed to 'ComposerCore'
  * 'Appson.Composer.Implementation' is also included in 'ComposerCore'
* Package 'Appson.Composer.Web' is no longer available / supported
* Namespace changes
  * Root namespace changed to 'ComposerCore'
  * Attributes are moved to `ComposerCore.Attributes` (eg. `ComponentAttribute`, etc.)
  * Main implementation and related implementations are moved to `ComposerCore.Implementation`
    * `ComponentContext`, `ComposerConfiguration`, `ChildComponentContext`, `ComponentContextUtils`
  * Interfaces that support Composer's extensibility are moved to `ComposerCore.Extensibility`
    * `ICompositionListener`, `IComponentFactory`, `IComponentCache`, `ICompositionalQuery`, ...
* `ComposerLocalThreadBinder` is removed
* Emitter interfaces and utilities moved to ???
* Interceptor functionality is moved to ???
* Support for Windows Registry is removed from XML Configuration schema
* Support for Remoting is removed (both Component Factory and XML configuration)
* app.config / web.config support is moved to ???

