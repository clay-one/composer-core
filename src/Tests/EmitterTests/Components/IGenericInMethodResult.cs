namespace ComposerCore.Tests.EmitterTests.Components
{
	public interface IGenericInMethodResult<out T>
	{
		T SomeMethod();
	}
}
