using System;
using System.IO;
using System.Reflection;
using System.Resources;

namespace ComposerCore.Resources
{
	public class AssemblyResourcePointer : IResourcePointer
	{
		#region Private fields

		#endregion

		#region Public properties

		public string Id { get; set; }

		public Assembly Assembly { get; set; }

		public string ResourceName { get; set; }

		#endregion

		public void FillRepository(ResourceRepository repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");

			if (string.IsNullOrEmpty(Id))
				throw new InvalidOperationException(
					"Id should be assigned to a non-empty string before filling the resource repository by an instance of AssemblyResourcePointer.");

			if (string.IsNullOrEmpty(ResourceName))
				throw new InvalidOperationException(
					"ResourceName should be assigned to a non-empty string before filling the resource repository by an instance of AssemblyResourcePointer.");

			if (Assembly == null)
				throw new InvalidOperationException(
					"Assembly should be assigned to a non-empty string before filling the resource repository by an instance of AssemblyResourcePointer.");

			repository.Register(Id, this);
		}

		public ResourceManager GetResourceManager()
		{
			if (Assembly == null)
				throw new InvalidOperationException("AssemblyResourcePointer instance is not initialized. Assembly cannot be null.");
			if (string.IsNullOrEmpty(ResourceName))
				throw new InvalidOperationException(
					"AssemblyResourcePointer instance is not initialized. ResourceName cannot be null or empty.");

			return new ResourceManager(ResourceName, Assembly);
		}

		public Stream GetStream()
		{
			return Assembly.GetManifestResourceStream(ResourceName);
		}
	}
}