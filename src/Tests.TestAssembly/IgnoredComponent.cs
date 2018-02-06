using ComposerCore.Attributes;

namespace ComposerCore.Tests.TestAssembly
{
	[Component]
	[IgnoredOnAssemblyRegistration]
	public class IgnoredComponent : IIgnoredContract
	{
	}
}
