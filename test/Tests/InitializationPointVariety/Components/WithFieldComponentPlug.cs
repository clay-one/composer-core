﻿using ComposerCore.Attributes;

namespace ComposerCore.Tests.InitializationPointVariety.Components
{
	[Contract]
	[Component]
	public class WithFieldComponentPlug
	{
		[ComponentPlug] public ISampleContract SampleContract;
	}
}
