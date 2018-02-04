
# Migration guide

There are breaking changes for every major release relative to its previous release. 
This is a detailed documentation on what's changed in each major version, and guidelines for how to adapt your code.

## Migration from Composer 1.4 to ComposerCore 2.x

* Package name changes
  * 'Appson.Composer.Implementation' is changed to 'ComposerCore'
  * 'Appson.Composer.Base' is changed to 'ComposerCore.Definitions'
* Package 'Appson.Composer.Web' is no longer available / supported
* Root namespace changed to 'ComposerCore'
* `ComposerLocalThreadBinder` is removed
* Emitter interfaces and utilities removed from the ComposerCore.Definitions assembly

