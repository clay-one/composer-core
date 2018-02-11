

using ComposerCore.Aop.Interception;

namespace ComposerCore.Aop.Matching
{
	public interface ICallFilter
	{
		bool Match(CallInfo callInfo);
	}

	public delegate bool CallFilterMatch(CallInfo callInfo);
}
