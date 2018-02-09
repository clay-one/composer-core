using System.Reflection;
using System.Reflection.Emit;
using ComposerCore.Attributes;

namespace ComposerCore.Aop.Emitter
{
	[Contract]
	public interface IPropertyEmitter
	{
		PropertyInfo EmitProperty(TypeBuilder typeBuilder,
		                          PropertyInfo propertyInfo);
	}
}