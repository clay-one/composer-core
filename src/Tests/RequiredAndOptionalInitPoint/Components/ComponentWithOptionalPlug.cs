﻿using ComposerCore.Definitions;

namespace ComposerCore.Tests.RequiredAndOptionalInitPoint.Components
{
	[Contract]
	[Component]
	public class ComponentWithOptionalPlug
	{
		[ComponentPlug(false)]
		public IPluggedContract PluggedContract { get; set; }
	}
}
