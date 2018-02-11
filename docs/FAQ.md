
# Frequently Asked Questions



## Supported runtimes and dependencies

#### Q: Does ComposerCore have any dependencies?
> The main assembly, `ComposerCore`, is not dependent to any packages and only relies on
> the API provided by net-standard 2. Any code that required additional dependencies is separated 
> into additional projects/packages (like CompoerCore.Aop) 

#### Q: Does ComposerCore support .NET Core / .NET Framework?
> ComposerCore is built on net-standard 2, so it can be run on any runtime that supports it.
> This includes .NET Core 2.0+ and .NET Framework 4.7.1+.



## Design

#### Compositional Architecture? What?!
> ComposerCore is meant to be more than just IoC/DI. Although very similar in features, it's rather built to promote
> and facilitate designing software that is formed from well-defined, replaceable or composable parts. What happens in
> action is very similar to IoC/DI containers with Auto Wiring turned on, and there's little difference visible (if any).

#### What is IoC/DI?
> It's a relatively-popular design pattern used in Object Oriented programming. 
> The basic ideas behind the pattern is best described by Martin Fowler here: https://martinfowler.com/articles/injection.html

#### Why using attributes are mandatory?
> Actually, from version 1.4+ they are not. You can disable attribute checking using a setting in
> [Configuration](api-ref/configuration.md), but it's not recommended. Because the attributes are used to
> differentiate components and contracts from regular classes and interfaces, and make it explicit both
> for the coder and the reader. It's somewhat similar to type checking in a static language like C#.
> Read more about it in [Attributes](api-ref/attributes.md) pages.



## Features

#### Which injection style does ComposerCore support? Property-injection or Constructor-injection?
> Both.
>
> It's recommended to use property-injection, but ComposerCore can also identify constructor arguments and inject
> them as well, when creating a component instance. You can even use them together at the same time, having both
> constructor arguments and injection properties on a single component at the same time.

#### Does ComposerCore support cyclic dependencies
> Yes.
>
> Also it might be a sign that you have a flaw in your design, Composer can handle cyclic dependencies between
> components and will *not* throw `StackOverflowException` or anything when there's a loop 
> instantiating the components.
> Although this is only true when you're using property injection. For constructor injection, there's no way of
> instantiating an object with arguments that will be created later!



## Usage

#### Why does ComposerCore have several separate assemblies?
> The ComposerCore itself is independent of any other library except the
> net-standard version 2. Many APIs are not present in the net-standard yet.
> To keep the core composition functionality independent of any external libraries
> and not enforcing any additional references for usages, we've determined to
> separate projects/packages that need additional references. As an example,
> ComposerCore.Aop is dependent to Reflection.Emit.
