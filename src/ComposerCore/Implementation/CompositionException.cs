﻿using System;
using System.Runtime.Serialization;

namespace ComposerCore.Implementation
{
	[Serializable]
	public class CompositionException : Exception
	{
		public CompositionException()
		{
		}

		public CompositionException(string message)
			: base(message)
		{
		}

		public CompositionException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected CompositionException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
