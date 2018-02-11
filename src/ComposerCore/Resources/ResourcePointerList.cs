using System;
using System.Collections.Generic;
using System.IO;
using System.Resources;

namespace ComposerCore.Resources
{
	public class ResourcePointerList : IResourcePointer
	{
		#region Private fields

		#endregion

		#region Public properties

		public List<IResourcePointer> Pointers { get; set; }

		#endregion

		#region IResourcePointer implementation

		public void FillRepository(ResourceRepository repository)
		{
			if (Pointers == null)
				return;

			foreach (var pointer in Pointers)
				pointer.FillRepository(repository);
		}

		public ResourceManager GetResourceManager()
		{
			throw new InvalidOperationException("Calling GetResourceManager on a ResourcePointerList object is not permitted.");
		}

		public Stream GetStream()
		{
			throw new InvalidOperationException("Calling GetStream on a ResourcePointerList object is not permitted.");
		}

		#endregion
	}
}