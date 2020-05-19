using ComposerCore.Attributes;

namespace ComposerCore.Tests.ErrorConditions.Components
{
	[Contract]
	[Component]
	public class NonContractPlugType
	{
		[ComponentPlug(false)]
		public string WrongPlug { get; set; }
	}
}
