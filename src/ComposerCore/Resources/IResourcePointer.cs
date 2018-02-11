using System.IO;
using System.Resources;

namespace ComposerCore.Resources
{
	public interface IResourcePointer
	{
		void FillRepository(ResourceRepository repository);

		ResourceManager GetResourceManager();
		Stream GetStream();
	}
}