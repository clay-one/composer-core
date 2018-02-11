using System;
using System.Reflection;
using System.Reflection.Emit;
using ComposerCore.Attributes;

namespace ComposerCore.Aop.Emitter
{
	[Component]
	[ComponentCache(null)]
	public class DefaultPropertyEmitter : IPropertyEmitter
	{
		#region Implementation of IPropertyEmitter

		public PropertyInfo EmitProperty(TypeBuilder typeBuilder,
		                                         PropertyInfo propertyInfo)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}