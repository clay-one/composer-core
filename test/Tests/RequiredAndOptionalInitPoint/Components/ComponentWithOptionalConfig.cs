﻿using ComposerCore.Attributes;

namespace ComposerCore.Tests.RequiredAndOptionalInitPoint.Components
{
	[Contract]
	[Component]
	public class ComponentWithOptionalConfig
	{
		[ConfigurationPoint(false)]
		public string SomeConfig { get; set; }
	}
}
