using ComposerCore.Attributes;

namespace ComposerCore.Tests.CompositionByConstructor.Components
{
    [Contract, Component]
    public class MultipleLeastAndMostParams
    {
        public int Integer { get; }
        public string String { get; }
        public ISampleContractA ContractA { get; }
        public ISampleContractB ContractB { get; }

        public MultipleLeastAndMostParams(ISampleContractA contractA, ISampleContractB contractB, int integer, string s)
        {
            Integer = integer;
            String = s;
            ContractA = contractA;
            ContractB = contractB;
        }
        
        public MultipleLeastAndMostParams(int integer, string s, ISampleContractA contractA, ISampleContractB contractB)
        {
            Integer = integer;
            String = s;
            ContractA = contractA;
            ContractB = contractB;
        }
        
        public MultipleLeastAndMostParams(ISampleContractA contractA)
        {
            ContractA = contractA;
        }
        
        public MultipleLeastAndMostParams(ISampleContractB contractB)
        {
            ContractB = contractB;
        }
    }
}