using System.Threading;
using ComposerCore.Attributes;

namespace ComposerCore.Tests.ComponentFactories.Components
{
    [Component, Transient]
    public class SampleComponentOne : ISampleContractOne
    {
        public string ConstructorArg { get; }
        public string PublicProperty { get; set; }

        private static int _timesInstantiated; 
        public static int TimesInstantiated => _timesInstantiated;

        public SampleComponentOne()
        {
            Interlocked.Increment(ref _timesInstantiated);
        }

        public SampleComponentOne(string constructorArg) : this()
        {
            ConstructorArg = constructorArg;
        }
    }
}