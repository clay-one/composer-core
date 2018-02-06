using System;

namespace ComposerCore.Aop.Matching
{
	public interface ITypeFilter
	{
		bool Match(Type type);
	}

	public delegate bool TypeFilterMatch(Type type);
}