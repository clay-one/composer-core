using System.Linq;
using ComposerCore.Aop.Interception;


namespace ComposerCore.Aop.Matching
{
	public class DisjunctionCallFilter : ICallFilter
	{
		public ICallFilter[] Filters { get; set; }

		public DisjunctionCallFilter(ICallFilter[] filters)
		{
			Filters = filters;
		}

		public DisjunctionCallFilter()
		{
		}

		#region ICallFilter Members

		public bool Match(CallInfo callInfo)
		{
			return Filters.Any(filter => filter.Match(callInfo));
		}

		#endregion
	}
}
