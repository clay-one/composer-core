using System;
using ComposerCore.Extensibility;
using ComposerCore.Factories;
using ComposerCore.Implementation;

namespace ComposerCore.FluentExtensions
{
    public class FluentFactoryMethodComponentConfig<TComponent> 
        : FluentComponentConfigBase<FluentFactoryMethodComponentConfig<TComponent>> where TComponent : class
    {
        private readonly FactoryMethodRegistration<TComponent> _factoryMethodRegistration;

        protected override IComponentRegistration Registration => _factoryMethodRegistration;

        #region Constructors

        public FluentFactoryMethodComponentConfig(ComponentContext context, Func<IComposer, TComponent> factoryMethod)
            : base(context)
        {
            _factoryMethodRegistration = new FactoryMethodRegistration<TComponent>(factoryMethod);
        }

        #endregion
    }
}