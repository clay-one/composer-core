
# ComposerCore Sample - Quick Start scenario one

In this sample, you see the minimal way of using ComposerCore to instantiate a component
without any dependency injection complexities.

[Complete source of this sample](../../samples/quickstart-1)

#### Declare a contract type

Declare a `class` or `interface` to be a contract for a component. You use such contracts
to look up a component from ComposerCore, that is, when you're looking for some component to *provide* the
services specified by the contract. Use `[Contract]` meta data attribute on a type to do so.

```csharp
    [Contract]
    public interface ILogger
    {
        void Log(string log);
    }
```

#### Write a component

Implement a concrete class that implements / extends the contract type

```csharp
    [Component]
    public class DefaultLogger : ILogger
    {
        public void Log(string log)
        {
            Console.WriteLine(log);
        }
    }
```

#### Create an instance of ComposerCore and register the component type

ComposerCore is a simple class library, and you can simply instantiate the `ComponentContext` class
to have a new instance of ComposerCore ready to use. Then use the `Register` method on the instance to
introduce component types to ComposerCore.

```csharp
    var composer = new ComponentContext();
    composer.Register(typeof(DefaultLogger));
```

ComposerCore will reflect the component type, and discover the *provided contracts*. After doing so, 
ComposerCore **knows** someone who can *provide* the mentioned contract, so you can ask for it.

#### Query for the component

Ask ComposerCore that "I want some object who can provide this service" using `GetComponent` method.

```csharp
    composer.GetComponent<ILogger>().Log("Hello, compositional world!");
```

ComposerCore will find the appropriate component type (which is previously registered), instantiate it,
initialize it and return it.

ComposerCore will also keep a reference to the component, so that it can respond to the later requests
with the same instance. So if you ask for the component again, the same instance is returned. (You can
change this behavior by specifying [component cache settings](api-ref/component-cache.md).)




