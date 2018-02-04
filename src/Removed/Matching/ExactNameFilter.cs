namespace ComposerCore.Utility.Matching
{
	public class ExactNameFilter : INameFilter
	{
		public string MethodName { get; set; }

		#region INameFilter implementation

		public bool Match(string methodNameToMatch)
		{
			return (methodNameToMatch == MethodName);
		}

		#endregion
	}
}