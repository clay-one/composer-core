namespace ComposerCore.Tests.FluentRegistration.Components
{
    public interface IGenericContract<T>
    {
        
    }

    public interface IAnotherGenericContract<T>
    {
        
    }

    public class OpenGenericComponent<T> : IGenericContract<T>, IAnotherGenericContract<T>
    {
        
    }

    public class ClosedGenericComponent : IGenericContract<string>
    {
        
    }
}