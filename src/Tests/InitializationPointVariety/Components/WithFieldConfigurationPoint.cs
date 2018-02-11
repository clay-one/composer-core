using ComposerCore.Attributes;

namespace ComposerCore.Tests.InitializationPointVariety.Components
{
	[Contract]
	[Component]
	public class WithFieldConfigurationPoint
	{
		[ConfigurationPoint("SomeConfigurationPoint")]
		public string SomeConfigurationPoint;
	}
}
