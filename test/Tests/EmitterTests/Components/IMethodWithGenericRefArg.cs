namespace ComposerCore.Tests.EmitterTests.Components
{
	public interface IMethodWithGenericRefArg
	{
		void SomeMethod<T>(ref T t);
	}
}
