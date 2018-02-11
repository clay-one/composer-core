using ComposerCore.Aop.Interception;


namespace ComposerCore.Aop.Matching
{
	public class ArgumentValueCallFilter : ICallFilter
	{
		public int ArgumentIndex { get; set; }
		public object ArgumentValue { get; set; }

		public ArgumentValueCallFilter(int argumentIndex, object argumentValue)
		{
			ArgumentIndex = argumentIndex;
			ArgumentValue = argumentValue;
		}

		public ArgumentValueCallFilter()
		{
		}
        
		#region ICriteria Members

		public bool Match(CallInfo callInfo)
		{
			if (ArgumentIndex >= callInfo.Arguments.Length)
				return false;

			if (ArgumentValue == null)
				return (callInfo.Arguments[ArgumentIndex] == null);

			return ArgumentValue.Equals(callInfo.Arguments[ArgumentIndex]);
		}

		#endregion
	}
}
