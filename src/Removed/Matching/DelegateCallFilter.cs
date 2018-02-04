using ComposerCore.Interceptor;


namespace ComposerCore.Utility.Matching
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
