
[![Build status](https://ci.appveyor.com/api/projects/status/usp33tkarr0twxt3/branch/master?svg=true&passingText=master:%20pass&pendingText=master:%20pend&failingText=master:%20fail)](https://ci.appveyor.com/project/iravanchi/composer-core/branch/master)
[![Build status](https://ci.appveyor.com/api/projects/status/usp33tkarr0twxt3/branch/develop?svg=true&passingText=develop:%20pass&pendingText=develop:%20pend&failingText=develop:%20fail)](https://ci.appveyor.com/project/iravanchi/composer-core/branch/develop)
[![Build status](https://ci.appveyor.com/api/projects/status/usp33tkarr0twxt3?svg=true&passingText=latest:%20pass&pendingText=latest:%20pend&failingText=latest:%20fail)](https://ci.appveyor.com/project/iravanchi/composer-core)


# ComposerCore

[Documentation](docs/TOC.md) -
[Quick Start](docs/quickstart.md) -
[Concepts](docs/concepts.md) -
[Frequently Asked Questions](docs/FAQ.md)

ComposerCore is an extensible Compositional Architecture framework for .NET, providing a set of functionality such as Inversion of Control container (IoC), Dependency Injection (DI), Plug-in framework, Aspect Oriented Programming (AOP), Configurability and Composability for components. See [Documentation](docs/TOC.md) for more details.

It enables software to be developed in a **Compositional** manner. A complete software system can be formed by composing various smaller re-usable parts together. It simplifies the software development process, and help developers and architects meet design goals such as Configurability, Extensibility, Reusability and Customizability.
[More...](docs/FAQ.md)

# What problems are addressed by ComposerCore?

The items below are high-level benefits of building software in Compositional architecture, which is provided by ComposerCore.

* **Inversion of Control**: ComposerCore can act as a container for Inversion of Control.
* **Dependency Injection**: ComposerCore can inject dependencies of components either created by itself, or created outside and handed over to it.
* **Interception and Filtering**: ComposerCore facilitates means for intercepting calls between components, filtering them, or redirecting them.
* **Dynamic Mocking**: ComposerCore has tools for dynamically creating implementations of interfaces.
* **Aspect Oriented Programming**: ComposerCore facilitates easy creation of Aspects between layers.
* **Extensibility**: ComposerCore provides means for building extensible software components quickly and easily.
* **Plug-in framework**: ComposerCore makes it easy to incorporate plug-in functionality in an application.
* **Configuration**: ComposerCore allows software components declare configuration options, which can be provided from flexible sources without the knowledge of the component itself.
* **Customizability**: ComposerCore makes customizing software for different customer much easier.
* **Reusability**: ComposerCore enables easier re-use of software components across different products, and across customizations of the same product.
* **Substitutability**: ComposerCore makes individual components easily substitutable, without affecting other parts of the system.
* **Testability**: ComposerCore makes it easy to isolate a single component or a group of components for testing.
* **Instance Sharing**: ComposerCore can keep track of instantiated components, and share the instance for different uses to prevent unneeded object creations.
* **Lazy Initialization** ComposerCore makes it easy to defer instantiation of components until when they are actually needed.
* **XML Based composition**: ComposerCore has utilities to parse XML documents that specify the composition and registration of components.
* **Extend Composition Logic**: ComposerCore is extensible itself, allowing you to customize and add your own logic to the composition flow in several ways.

[Documentation](docs/TOC.md) -
[Quick Start](docs/quickstart.md) -
[Concepts](docs/concepts.md) -
[Frequently Asked Questions](docs/FAQ.md)

# Migrating from Composer

ComposerCore is based on, and adapted from, the [Composer](https://github.com/appson/composer) project. It is restructured and ported to .NET Core.

If you're already using Composer, see [Migration Guild](docs/migration.md) for a comprehensive list of changes.
