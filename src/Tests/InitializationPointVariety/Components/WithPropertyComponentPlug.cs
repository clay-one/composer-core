using ComposerCore.Attributes;

namespace ComposerCore.Tests.InitializationPointVariety.Components
{
	[Contract]
	[Component]
	public class WithPropertyComponentPlug
	{
		[ComponentPlug]
		public ISampleContract SampleContract { get; set; }
	}
}
