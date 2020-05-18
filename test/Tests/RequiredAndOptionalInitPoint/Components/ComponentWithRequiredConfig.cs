using ComposerCore.Attributes;

namespace ComposerCore.Tests.RequiredAndOptionalInitPoint.Components
{
	[Contract, Component, Transient]
	public class ComponentWithRequiredConfig
	{
		[ConfigurationPoint(true)]
		public string SomeConfig { get; set; }
	}
}
