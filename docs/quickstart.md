
# ComposerCore Quick Start Guide

## Installing ComposerCore

ComposerCore is published on 
[nuget.org](https://www.nuget.org/packages/ComposerCore)
and doesn't have any external dependencies, so you can easily add it to your project.

* **Package Manager:**
  * `Install-Package ComposerCore`

* **.NET CLI:**
  * `dotnet add package ComposerCore`

* **Paket CLI:**
  * `paket add ComposerCore`



## Using ComposerCore

See these examples to get to up and running fast with ComposerCore:

* [Quick start sample 1](samples/quickstart1.md) - minimal use of composer to create a component instance
* [Quick start sample 2](samples/quickstart2.md) - adding dependency injection to the first sample



## Further reading

Check out the following topics in the documentation to develop a mindset about what ComposerCore can do, and
make sure you know what you're doing when adding ComposerCore to your mix:

* [Concepts](concepts.md) - basic concepts introduced or used by ComposerCore
* [IComponentContext](api-ref/icomponentcontext.md) - APIs for registering components
* [IComposer](api-ref/icomposer.md) - APIs for querying for components
* [Fluent API](api-ref/fluent.md) - Fluent APIs for registering and configuring components

To check-out all documentation topics, see
[Table of Contents](TOC.md) 