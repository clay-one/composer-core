using ComposerCore.Attributes;

namespace ComposerCore.Tests.RequiredAndOptionalInitPoint.Components
{
	[Contract]
	[Component]
	public class ComponentWithRequiredConfig
	{
		[ConfigurationPoint(true)]
		public string SomeConfig { get; set; }
	}
}
