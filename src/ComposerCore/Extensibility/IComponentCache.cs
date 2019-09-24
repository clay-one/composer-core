using ComposerCore.Attributes;

namespace ComposerCore.Extensibility
{
	[Contract]
	public interface IComponentCache
	{
		ComponentCacheEntry GetFromCache(ContractIdentity contract);
		void PutInCache(ContractIdentity contract, ComponentCacheEntry entry);

		object SynchronizationObject { get; }
	}

	public class ComponentCacheEntry
	{
		public object ComponentInstance;
		public object OriginalComponentInstance;
	}
}