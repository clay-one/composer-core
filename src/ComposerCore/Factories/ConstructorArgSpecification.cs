using ComposerCore.Definitions;

namespace ComposerCore.Factories
{
	public class ConstructorArgSpecification
	{
		public ConstructorArgSpecification(bool required = true, ICompositionalQuery query = null)
		{
			Required = required;
			Query = query;
		}

		public bool Required { get; }
		public ICompositionalQuery Query { get; set; }
	}
}
