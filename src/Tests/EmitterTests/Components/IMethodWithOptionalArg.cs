namespace ComposerCore.Tests.EmitterTests.Components
{
	public interface IMethodWithOptionalArg
	{
		void SomeMethod(string s = "default", int i = 1);
	}
}
