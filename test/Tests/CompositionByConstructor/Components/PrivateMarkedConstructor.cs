using ComposerCore.Attributes;

namespace ComposerCore.Tests.CompositionByConstructor.Components
{
	[Contract, Component]
	public class PrivateMarkedConstructor
	{
// ReSharper disable UnusedMember.Local
		[CompositionConstructor]
		private PrivateMarkedConstructor(ISampleContractA a, ISampleContractB b)
		{
			A = a;
			B = b;
			InvokedConstructor = 3;
		}
// ReSharper restore UnusedMember.Local

		public ISampleContractA A { get; }

		public ISampleContractB B { get; }

		public int InvokedConstructor { get; }
	}
}
