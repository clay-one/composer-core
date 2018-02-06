using System;

namespace ComposerCore.Aop.Interception
{
	/// <summary>
	/// Encapsulates an exception that has occured during interception of a method call.
	/// </summary>
	public class AdaptedException : Exception
	{
		public AdaptedException(Exception innerException, string methodName, object[] arguments,
			bool beforeCall, bool duringCall, bool afterCall)
			: this(
				"An exception is thrown while adapting a call to " + methodName,
				innerException, methodName, arguments, beforeCall, duringCall, afterCall)
		{
		}


		public AdaptedException(string message, Exception innerException, string methodName, object[] arguments,
			bool beforeCall, bool duringCall, bool afterCall)
			: base(message, innerException)
		{
			MethodName = methodName;
			Arguments = arguments;

			BeforeCall = beforeCall;
			DuringCall = duringCall;
			AfterCall = afterCall;
		}

		public string MethodName { get; }

		public object[] Arguments { get; }

		public bool BeforeCall { get; }

		public bool DuringCall { get; }

		public bool AfterCall { get; }
	}
}
