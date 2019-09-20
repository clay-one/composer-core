using System;
using ComposerCore.Aop.Interception;

namespace ComposerCore.Tests.InterceptorTests.Components
{
	internal class RecordingInterceptor : ICallInterceptor
	{
		public CallInfo BeforeCallInfo;
		public CallInfo AfterCallInfo;

		public Action<CallInfo> BeforeCallAction;
		public Action<CallInfo> AfterCallAction;

		#region Implementation of ICallInterceptor

		public void BeforeCall(CallInfo callInfo)
		{
			BeforeCallInfo = new CallInfo(callInfo);

			BeforeCallAction?.Invoke(callInfo);
		}

		public void AfterCall(CallInfo callInfo)
		{
			AfterCallInfo = new CallInfo(callInfo);

			AfterCallAction?.Invoke(callInfo);
		}

		#endregion
	}
}
