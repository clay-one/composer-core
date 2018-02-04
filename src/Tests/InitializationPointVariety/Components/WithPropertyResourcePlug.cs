using System.Resources;
using ComposerCore.Definitions;

namespace ComposerCore.Tests.InitializationPointVariety.Components
{
	[Contract]
	[Component]
	public class WithPropertyResourcePlug
	{
		[ResourceManagerPlug("resourceId")]
		public ResourceManager ResourcePlug { get; set; }
	}
}
