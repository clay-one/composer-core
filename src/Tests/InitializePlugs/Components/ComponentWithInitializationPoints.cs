using ComposerCore.Attributes;

namespace ComposerCore.Tests.InitializePlugs.Components
{
	[Contract]
	[Component]
	public class ComponentWithInitializationPoints
	{
		[ComponentPlug]
		public ISampleContract SampleContract { get; set; }

		[ConfigurationPoint("InitPointVariable")]
		public int InitPoint { get; set; }
	}
}
