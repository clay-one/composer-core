using ComposerCore.Attributes;

namespace ComposerCore.Tests.ErrorConditions.Components
{
	[Contract]
	[Component]
	public class PlugWithoutSetter
	{
		private readonly ISampleContract _sampleContract;

		public PlugWithoutSetter()
		{
			_sampleContract = null;
		}

		[ComponentPlug(false)]
		public ISampleContract SampleContract
		{
			get
			{
				return _sampleContract;
			}
		}
	}
}
