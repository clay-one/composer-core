using System;
using System.Reflection;
using System.Reflection.Emit;
using ComposerCore.Attributes;

namespace ComposerCore.Aop.Emitter
{
	[Contract]
	public interface IMethodEmitter
	{
		MethodBuilder EmitMethod(TypeBuilder targetTypeBuilder,
		                         string methodName,
		                         Type[] parameterTypes,
		                         bool[] isParameterOut,
		                         Type resultType,
		                         Type reflectedType,
		                         MethodAttributes methodAttributes);
	}
}