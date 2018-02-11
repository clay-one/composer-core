namespace ComposerCore.Tests.EmitterTests.Components
{
	public interface IGenericInMethodArg<in T>
	{
		void SomeMethod(T t);
	}
}
