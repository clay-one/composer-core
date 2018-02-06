

using ComposerCore.Aop.Interception;

namespace ComposerCore.Aop.Matching
{
	public class DelegateCallFilter : ICallFilter
	{
		public CallFilterMatch CallFilterMatchMethod { get; set; }

		public DelegateCallFilter(CallFilterMatch method)
		{
			CallFilterMatchMethod = method;
		}

		public DelegateCallFilter()
		{
		}

		#region ICallFilter Members

		public bool Match(CallInfo callInfo)
		{
			return CallFilterMatchMethod?.Invoke(callInfo) ?? false;
		}

		#endregion

	}
}
