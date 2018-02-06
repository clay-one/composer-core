namespace ComposerCore.Aop.Matching
{
	public interface INameFilter
	{
		bool Match(string name);
	}

	public delegate bool NameFilterMatch(string name);
}