using System.Resources;
using ComposerCore.Definitions;

namespace ComposerCore.Tests.InitializationPointVariety.Components
{
	[Contract]
	[Component]
	public class WithFieldResourcePlug
	{
		[ResourceManagerPlug("resourceId")]
		public ResourceManager ResourcePlug;
	}
}
