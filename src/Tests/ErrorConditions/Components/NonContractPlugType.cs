using ComposerCore.Attributes;

namespace ComposerCore.Tests.ErrorConditions.Components
{
	[Contract]
	[Component]
	public class NonContractPlugType
	{
		[ComponentPlug]
		public string WrongPlug { get; set; }
	}
}
