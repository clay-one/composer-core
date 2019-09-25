using ComposerCore.Attributes;

namespace ComposerCore.Tests.RequiredAndOptionalInitPoint.Components
{
	[Contract, Component, Transient]
	public class ComponentWithOptionalNamedPlug
	{
		[ComponentPlug("contractName", false)]
		public IPluggedContract PluggedContract { get; set; }
	}
}
