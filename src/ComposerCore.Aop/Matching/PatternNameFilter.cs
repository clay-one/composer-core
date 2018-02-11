using System.Text.RegularExpressions;

namespace ComposerCore.Aop.Matching
{
	public class PatternNameFilter : INameFilter
	{
		private string _pattern;
		private Regex _regex;

		public string Pattern
		{
			get => _pattern;
			set
			{
				_pattern = value;
				_regex = new Regex(_pattern);
			}
		}

		#region INameFilter implementation

		public bool Match(string methodName)
		{
			return _regex != null && _regex.IsMatch(methodName);
		}

		#endregion
	}
}