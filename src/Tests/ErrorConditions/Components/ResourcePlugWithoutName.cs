using System.Resources;
using ComposerCore.Attributes;

namespace ComposerCore.Tests.ErrorConditions.Components
{
	[Contract]
	[Component]
	public class ResourcePlugWithoutName
	{
		[ResourceManagerPlug(null)]
		public ResourceManager SomeResource { get; set; }
	}
}
