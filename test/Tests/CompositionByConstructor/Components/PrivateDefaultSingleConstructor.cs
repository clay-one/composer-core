using ComposerCore.Attributes;

namespace ComposerCore.Tests.CompositionByConstructor.Components
{
	[Contract, Component]
	public class PrivateDefaultSingleConstructor
	{
// ReSharper disable UnusedMember.Local
		private PrivateDefaultSingleConstructor()
		{
			A = null;
			B = null;
			InvokedConstructor = 1;
		}
// ReSharper restore UnusedMember.Local

		public ISampleContractA A { get; }

		public ISampleContractB B { get; }

		public int InvokedConstructor { get; }
	}
}
