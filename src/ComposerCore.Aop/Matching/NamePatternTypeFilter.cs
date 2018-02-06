using System;
using System.Text.RegularExpressions;

namespace ComposerCore.Aop.Matching
{
	public class NamePatternTypeFilter : ITypeFilter
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

		#region ITypeFilter implementation

		public bool Match(Type type)
		{
			return _regex != null && _regex.IsMatch(type.Name);
		}

		#endregion
	}
}