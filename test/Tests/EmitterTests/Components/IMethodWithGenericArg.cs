namespace ComposerCore.Tests.EmitterTests.Components
{
	public interface IMethodWithGenericArg
	{
		void SomeMethod<T>(T t);
	}
}
