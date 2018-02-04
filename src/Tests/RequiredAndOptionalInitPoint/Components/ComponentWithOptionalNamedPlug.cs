using ComposerCore.Definitions;

namespace ComposerCore.Tests.RequiredAndOptionalInitPoint.Components
{
	[Contract]
	[Component]
	public class ComponentWithOptionalNamedPlug
	{
		[ComponentPlug("contractName", false)]
		public IPluggedContract PluggedContract { get; set; }
	}
}
