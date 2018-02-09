using ComposerCore.Attributes;

namespace ComposerCore.Tests.ErrorConditions.Components
{
	[Contract]
	[Component]
	public class ResourcePlugWithWrongType
	{
		[ResourceManagerPlug("someId")]
		public string WrongTypeResourcePlug { get; set; }
	}
}
