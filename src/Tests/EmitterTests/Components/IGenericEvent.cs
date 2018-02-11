using System;

namespace ComposerCore.Tests.EmitterTests.Components
{
	public interface IGenericEvent<T> where T : EventArgs
	{
		event EventHandler<T> SomeEvent;
	}
}
