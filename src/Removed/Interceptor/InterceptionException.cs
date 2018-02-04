using System;

namespace ComposerCore.Interceptor
{
	/// <summary>
	/// Encapsulates an exception that has occured during interception of a method call.
	/// </summary>
	public class InterceptionException : Exception
	{
		public InterceptionException(Exception innerException, string methodName, object[] arguments, bool beforeCall,
		                             bool afterCall)
			: this(
				"An exception is thrown while intercepting a call to " + methodName, innerException, methodName,
				arguments, beforeCall, afterCall)
		{
		}


		public InterceptionException(string message, Exception innerException, string methodName, object[] arguments,
		                             bool beforeCall, bool afterCall)
			: base(message, innerException)
		{
			MethodName = methodName;
			Arguments = arguments;
			BeforeCall = beforeCall;
			AfterCall = afterCall;
		}

		public string MethodName { get; }

		public object[] Arguments { get; }

		public bool BeforeCall { get; }

		public bool AfterCall { get; }
	}
}