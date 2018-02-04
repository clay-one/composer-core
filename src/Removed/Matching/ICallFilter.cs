using ComposerCore.Interceptor;


namespace ComposerCore.Utility.Matching
{
	public interface ICallFilter
	{
		bool Match(CallInfo callInfo);
	}

	public delegate bool CallFilterMatch(CallInfo callInfo);
}
