using System.Linq;
using ComposerCore.Interceptor;


namespace ComposerCore.Utility.Matching
{
	public class ConjunctionCallFilter : ICallFilter
	{
		public ICallFilter[] Filters { get; set; }

		public ConjunctionCallFilter(ICallFilter[] filters)
		{
			Filters = filters;
		}

		public ConjunctionCallFilter()
		{
		}

		#region ICallFilter Members

		public bool Match(CallInfo callInfo)
		{
			return Filters.All(filter => filter.Match(callInfo));
		}

		#endregion
	}
}
