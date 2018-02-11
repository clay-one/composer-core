using ComposerCore.Attributes;

namespace ComposerCore.Tests.RequiredAndOptionalInitPoint.Components
{
	[Contract]
	[Component]
	public class ComponentWithOptionalNamedConfig
	{
		[ConfigurationPoint("someVariable", false)]
		public string SomeConfig { get; set; }
	}
}
