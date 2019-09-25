using ComposerCore.Attributes;

namespace ComposerCore.Tests.RequiredAndOptionalInitPoint.Components
{
	[Contract, Component, Transient]
	public class ComponentWithRequiredNamedPlug
	{
		[ComponentPlug("contractName", true)]
		public IPluggedContract PluggedContract { get; set; }
	}
}
