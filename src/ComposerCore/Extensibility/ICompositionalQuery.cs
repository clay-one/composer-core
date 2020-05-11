namespace ComposerCore.Extensibility
{
	public interface ICompositionalQuery
	{
		bool IsResolvable(IComposer composer);
		object Query(IComposer composer, IComposer scope = null);
	}
}
