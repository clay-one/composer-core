namespace ComposerCore.Tests.FluentRegistration.Components
{
    public class GenericComponentWithManyConstructors<T> : IGenericContract<T>, IAnotherGenericContract<T>
    {
        public const string DefaultConstructor = nameof(DefaultConstructor);
        public const string IntegerConstructor = nameof(IntegerConstructor);
        public const string StringConstructor = nameof(StringConstructor);
        public const string ContractAConstructor = nameof(ContractAConstructor);
        public const string ContractBConstructor = nameof(ContractBConstructor);
        public const string ContractAAndBConstructor = nameof(ContractAAndBConstructor);
        public const string ContractAAndBAndIntegerConstructor = nameof(ContractAAndBAndIntegerConstructor);
        public const string ContractAAndBAndIntegerAndStringConstructor = 
            nameof(ContractAAndBAndIntegerAndStringConstructor);
        
        public string InvokedConstructor { get; }
        
        public int Integer { get; }
        public string String { get; }
        public IComponentOne One { get; }
        public IComponentTwo Two { get; }
        
        public GenericComponentWithManyConstructors()
        {
            Integer = 1;
            String = string.Empty;
            
            InvokedConstructor = DefaultConstructor;
        }
        
        public GenericComponentWithManyConstructors(int i) : this()
        {
            Integer = i;
            InvokedConstructor = IntegerConstructor;
        }
        
        public GenericComponentWithManyConstructors(string s) : this()
        {
            String = s;
            InvokedConstructor = StringConstructor;
        }
        
        public GenericComponentWithManyConstructors(IComponentOne one) : this()
        {
            One = one;
            InvokedConstructor = ContractAConstructor;
        }
        
        public GenericComponentWithManyConstructors(IComponentTwo two) : this()
        {
            Two = two;
            InvokedConstructor = ContractBConstructor;
        }
        
        public GenericComponentWithManyConstructors(IComponentOne one, IComponentTwo two) : this()
        {
            One = one;
            Two = two;
            InvokedConstructor = ContractAAndBConstructor;
        }
        
        public GenericComponentWithManyConstructors(IComponentOne one, IComponentTwo two, int i) : this()
        {
            One = one;
            Two = two;
            Integer = i;
            InvokedConstructor = ContractAAndBAndIntegerConstructor;
        }
        
        public GenericComponentWithManyConstructors(IComponentOne one, IComponentTwo two, int i, string s) : this()
        {
            One = one;
            Two = two;
            Integer = i;
            String = s;
            InvokedConstructor = ContractAAndBAndIntegerAndStringConstructor;
        }
    }
}