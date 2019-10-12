using ComposerCore.Attributes;
using ComposerCore.Extensibility;

namespace ComposerCore.Cache
{
	[Contract, Component, ComponentCache(null), ConstructorResolutionPolicy(null)]
	public class ContractAgnosticComponentCache
	{
		[CompositionConstructor]
		public ContractAgnosticComponentCache()
		{
		}
	}
}