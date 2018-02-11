using System.Configuration;

namespace ComposerCore.Configuration
{
	public class CompositionConfiguration : ConfigurationSection
	{
		[ConfigurationProperty("setupCompositionXmls", IsDefaultCollection = false)]
		public SetupCompositionXmlsCollection SetupCompositionXmls => (SetupCompositionXmlsCollection)base["setupCompositionXmls"];
	}
}