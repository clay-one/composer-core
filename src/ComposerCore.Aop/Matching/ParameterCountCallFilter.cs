

using ComposerCore.Aop.Interception;

namespace ComposerCore.Aop.Matching
{
	public class ParameterCountCallFilter : ICallFilter
	{
		public int Count { get; set; }

		public ParameterCountCallFilter(int count)
		{
			Count = count;
		}

		public ParameterCountCallFilter()
		{
		}

		#region ICriteria Members

		public bool Match(CallInfo callInfo)
		{
			return Count == callInfo.Arguments.Length;
		}

		#endregion
	}
}
