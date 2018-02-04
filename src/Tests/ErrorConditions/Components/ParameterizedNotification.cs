using ComposerCore.Definitions;

namespace ComposerCore.Tests.ErrorConditions.Components
{
	[Contract]
	[Component]
	public class ParameterizedNotification
	{
		[OnCompositionComplete]
		public void OnCompositionComplete(string wrongArg)
		{
		}
	}
}
