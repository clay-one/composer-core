using ComposerCore.Attributes;

namespace ComposerCore.Tests.RequiredAndOptionalInitPoint.Components
{
	[Contract]
	[Component]
	public class ComponentWithRequiredNamedConfig
	{
		[ConfigurationPoint("someVariable", true)]
		public string SomeConfig { get; set; }
	}
}
