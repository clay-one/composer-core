using ComposerCore.Definitions;

namespace ComposerCore.Tests.RequiredAndOptionalInitPoint.Components
{
	[Contract]
	[Component]
	public class ComponentWithRequiredPlug
	{
		[ComponentPlug(true)]
		public IPluggedContract PluggedContract { get; set; }
	}
}
