using ComposerCore.Attributes;

namespace ComposerCore.Tests.ErrorConditions.Components
{
	[Contract]
	[Component]
	public class PlugWithPrivateSetter
	{
		public PlugWithPrivateSetter()
		{
			SampleContract = null;
		}

		[ComponentPlug(false)]
		public ISampleContract SampleContract { get; private set; }
	}
}
