namespace ComposerCore.Tests.EmitterTests.Components
{
	public interface IDoubleParamIndexer
	{
		string this[string s, int i] { get; set; }
	}
}
