using System.Reflection;
using System.Reflection.Emit;
using ComposerCore.Attributes;

namespace ComposerCore.Aop.Emitter
{
	[Contract]
	public interface IEventEmitter
	{
		EventInfo EmitEvent(TypeBuilder typeBuilder,
		                    EventInfo eventInfo);
	}
}