using ComposerCore.Attributes;

namespace ComposerCore.Tests.CompositionByConstructor.Components
{
    [Contract, Component, Transient]
    public class ManyConstructors
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
        public ISampleContractA ContractA { get; }
        public ISampleContractB ContractB { get; }
        
        public ManyConstructors()
        {
            Integer = 1;
            String = string.Empty;
            
            InvokedConstructor = DefaultConstructor;
        }
        
        public ManyConstructors(int i) : this()
        {
            Integer = i;
            InvokedConstructor = IntegerConstructor;
        }
        
        public ManyConstructors(string s) : this()
        {
            String = s;
            InvokedConstructor = StringConstructor;
        }
        
        public ManyConstructors(ISampleContractA a) : this()
        {
            ContractA = a;
            InvokedConstructor = ContractAConstructor;
        }
        
        public ManyConstructors(ISampleContractB b) : this()
        {
            ContractB = b;
            InvokedConstructor = ContractBConstructor;
        }
        
        public ManyConstructors(ISampleContractA a, ISampleContractB b) : this()
        {
            ContractA = a;
            ContractB = b;
            InvokedConstructor = ContractAAndBConstructor;
        }
        
        public ManyConstructors(ISampleContractA a, ISampleContractB b, int i) : this()
        {
            ContractA = a;
            ContractB = b;
            Integer = i;
            InvokedConstructor = ContractAAndBAndIntegerConstructor;
        }
        
        public ManyConstructors(ISampleContractA a, ISampleContractB b, int i, string s) : this()
        {
            ContractA = a;
            ContractB = b;
            Integer = i;
            String = s;
            InvokedConstructor = ContractAAndBAndIntegerAndStringConstructor;
        }
    }
}