using ComposerCore.Attributes;

namespace ComposerCore.Tests.RequiredAndOptionalInitPoint.Components
{
	[Contract, Component, Transient]
	public class ComponentWithOptionalPlug
	{
		[ComponentPlug(false)]
		public IPluggedContract PluggedContract { get; set; }
	}
}
