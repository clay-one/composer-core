﻿using ComposerCore.Definitions;

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

		[ComponentPlug]
		public ISampleContract SampleContract { get; private set; }
	}
}