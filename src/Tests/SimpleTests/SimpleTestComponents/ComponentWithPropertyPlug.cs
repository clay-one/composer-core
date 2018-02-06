using ComposerCore.Attributes;

namespace ComposerCore.Tests.SimpleTests.SimpleTestComponents
{
	[Contract]
	[Component]
	public class ComponentWithPropertyPlug
	{
		[ComponentPlug]
		public EmptyComponentAndContract Plug { get; set; }
	}
}