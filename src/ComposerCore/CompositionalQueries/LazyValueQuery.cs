using System;
using ComposerCore.Extensibility;

namespace ComposerCore.CompositionalQueries
{
	public class LazyValueQuery : ICompositionalQuery
	{
		public LazyValueQuery(Lazy<object> value)
		{
		    Value = value ?? throw new ArgumentNullException(nameof(value));
		}

		#region Implementation of ICompositionalQuery

		public bool IsResolvable(IComposer composer)
		{
			return true;
		}
        
		public object Query(IComposer composer, IComposer scope = null)
		{
			return Value.Value;
		}

		#endregion

		public override string ToString()
		{
			return $"LazyValueQuery('{Value}')";
		}

		public Lazy<object> Value { get; }
	}
}
