﻿namespace ComposerCore.Tests.FluentRegistration.Components
{
    public interface IGenericContract<T>
    {
        
    }

    public class OpenGenericComponent<T> : IGenericContract<T>
    {
        
    }

    public class ClosedGenericComponent : IGenericContract<string>
    {
        
    }
}