using ComposerCore.Attributes;

namespace ComposerCore.Tests.CompositionByConstructor.Components
{
	[Contract, Component]
	public class SingleNonDefaultConstructor
	{
		public SingleNonDefaultConstructor(ISampleContractA a, ISampleContractB b)
		{
			A = a;
			B = b;
			InvokedConstructor = 3;
		}

		public ISampleContractA A { get; }

		public ISampleContractB B { get; }

		public int InvokedConstructor { get; }
	}
}
