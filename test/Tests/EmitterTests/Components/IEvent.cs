using System;

namespace ComposerCore.Tests.EmitterTests.Components
{
	public interface IEvent
	{
		event EventHandler SomeEvent;
	}
}
