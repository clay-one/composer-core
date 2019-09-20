namespace ComposerCore.Tests.EmitterTests.Components
{
	public interface IMethodWithGenericResult
	{
		T SomeMethod<T>();
	}
}
