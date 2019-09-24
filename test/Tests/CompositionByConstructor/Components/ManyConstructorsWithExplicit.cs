using ComposerCore.Attributes;

namespace ComposerCore.Tests.CompositionByConstructor.Components
{
    [Contract, Component, Transient]
    public class ManyConstructorsWithExplicit
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
        
        public ManyConstructorsWithExplicit()
        {
            Integer = 1;
            String = string.Empty;
            
            InvokedConstructor = DefaultConstructor;
        }
        
        public ManyConstructorsWithExplicit(int i) : this()
        {
            Integer = i;
            InvokedConstructor = IntegerConstructor;
        }
        
        public ManyConstructorsWithExplicit(string s)
        {
            String = s;
            InvokedConstructor = StringConstructor;
        }
        
        public ManyConstructorsWithExplicit(ISampleContractA a)
        {
            ContractA = a;
            InvokedConstructor = ContractAConstructor;
        }
        
        public ManyConstructorsWithExplicit(ISampleContractB b)
        {
            ContractB = b;
            InvokedConstructor = ContractBConstructor;
        }
        
        public ManyConstructorsWithExplicit(ISampleContractA a, ISampleContractB b)
        {
            ContractA = a;
            ContractB = b;
            InvokedConstructor = ContractAAndBConstructor;
        }
        
        [CompositionConstructor]
        public ManyConstructorsWithExplicit(ISampleContractA a, ISampleContractB b, int i)
        {
            ContractA = a;
            ContractB = b;
            Integer = i;
            InvokedConstructor = ContractAAndBAndIntegerConstructor;
        }
        
        public ManyConstructorsWithExplicit(ISampleContractA a, ISampleContractB b, int i, string s)
        {
            ContractA = a;
            ContractB = b;
            Integer = i;
            String = s;
            InvokedConstructor = ContractAAndBAndIntegerAndStringConstructor;
        }
    }
}