﻿using System;
using ComposerCore.Aop.Emitter;

namespace ComposerCore.Tests.EmitterTests.Components
{
	internal class ReferenceMethodWithoutArgs : EmittedClassBase, IMethodWithoutArgs
	{
		#region Implementation of IMethodWithoutArgs

		public void SomeMethod()
		{
			Type reflectedType;
			object[] arguments;
			Type[] argumentTypes;
			Type resultType;

			reflectedType = typeof (IMethodWithoutArgs);
			arguments = new object[0];
			argumentTypes = new Type[0];
			resultType = typeof (void);

			DispatchCall(reflectedType, "SomeMethod", arguments, argumentTypes, resultType);
		}

		#endregion
	}
}
