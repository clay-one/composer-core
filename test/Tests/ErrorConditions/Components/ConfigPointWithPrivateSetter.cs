using ComposerCore.Attributes;

namespace ComposerCore.Tests.ErrorConditions.Components
{
	[Contract]
	[Component]
	public class ConfigPointWithPrivateSetter
	{
		public ConfigPointWithPrivateSetter()
		{
			SomeConfig = null;
		}

		[ConfigurationPoint("variableName", false)]
		public string SomeConfig { get; private set; }
	}
}
