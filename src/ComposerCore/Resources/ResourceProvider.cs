using System.Collections.Generic;
using System.IO;
using System.Resources;
using System.Text.RegularExpressions;
using ComposerCore.Attributes;
using ComposerCore.Extensibility;

namespace ComposerCore.Resources
{
	[Component]
	public class ResourceProvider : IResourceProvider
	{
		#region Fields

		private readonly ResourceRepository _repository;

		#endregion

		#region Constructors and Initializers

		public ResourceProvider()
		{
			_repository = new ResourceRepository();
		}

		[OnCompositionComplete]
		public void AfterComposition()
		{
			ResourcePointer.FillRepository(_repository);
		}

		#endregion

		#region Configuration points

		[ConfigurationPoint]
		public IResourcePointer ResourcePointer { get; set; }

		#endregion

		#region IResourceProvider implementation

		public ResourceManager GetResourceManager(string id)
		{
			var pointer = _repository.GetPointer(id);

			return pointer?.GetResourceManager();
		}

		public Stream GetStream(string id)
		{
			var pointer = _repository.GetPointer(id);

			return pointer?.GetStream();
		}

		public IEnumerable<string> GetList()
		{
			return _repository.GetIds();
		}

		public IEnumerable<string> GetList(Regex idRegex)
		{
			return _repository.FindMatches(idRegex);
		}

		#endregion
	}
}