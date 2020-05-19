using ComposerCore.Attributes;

namespace ComposerCore.Cache
{
	[Component(nameof(ContractAgnosticComponentCache)), Transient, ConstructorResolutionPolicy(null)]
	public class ContractAgnosticComponentCache : PerRegistrationComponentCache
	{
		[CompositionConstructor]
		public ContractAgnosticComponentCache()
		{
		}
	}
}