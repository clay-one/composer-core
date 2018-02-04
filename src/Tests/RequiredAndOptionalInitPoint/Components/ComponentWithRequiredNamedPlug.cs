using ComposerCore.Definitions;

namespace ComposerCore.Tests.RequiredAndOptionalInitPoint.Components
{
	[Contract]
	[Component]
	public class ComponentWithRequiredNamedPlug
	{
		[ComponentPlug("contractName", true)]
		public IPluggedContract PluggedContract { get; set; }
	}
}
