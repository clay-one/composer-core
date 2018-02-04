using System;

namespace ComposerCore.Utility.Matching
{
	public interface ITypeFilter
	{
		bool Match(Type type);
	}

	public delegate bool TypeFilterMatch(Type type);
}