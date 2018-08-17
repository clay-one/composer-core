﻿using ComposerCore.Attributes;

namespace ComposerCore.Tests.InitializationPointVariety.Components
{
	[Contract]
	[Component]
	public class WithPropertyConfigurationPoint
	{
		[ConfigurationPoint("SomeConfigurationPoint")]
		public string SomeConfigurationPoint { get; set; }
	}
}
