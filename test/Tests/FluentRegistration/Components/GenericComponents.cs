namespace ComposerCore.Tests.FluentRegistration.Components
{
    public interface IGenericContract<T>
    {
        
    }

    public interface IAnotherGenericContract<T>
    {
        
    }

    public interface IDoubleGenericContract<T1, T2>
    {
        
    }

    public class OpenGenericComponent<T> : IGenericContract<T>, IAnotherGenericContract<T>
    {
        
    }

    public class OpenGenericComponentWithDifferentName<TSomething> : IGenericContract<TSomething>,
        IAnotherGenericContract<TSomething>
    {
        
    }

    public class DoubleOpenGenericComponent<T1, T2> : IDoubleGenericContract<T1, T2>, IGenericContract<T2>
    {
        
    }

    public class ReverseDoubleOpenGenericComponent<T1, T2> : IDoubleGenericContract<T2, T1>
    {
        
    }

    public class ReverseDoubleOpenGenericComponentWithDifferentNames<T4, T3> : IDoubleGenericContract<T3, T4>
    {
        
    }
    
    public class ClosedGenericComponent : IGenericContract<string>
    {
        
    }
}