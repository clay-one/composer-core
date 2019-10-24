using System;
using ComposerCore.Extensibility;
using ComposerCore.Factories;
using ComposerCore.Implementation;

namespace ComposerCore.FluentExtensions
{
    public class FluentFactoryMethodComponentConfig<TComponent> 
        : FluentComponentConfigBase<FluentFactoryMethodComponentConfig<TComponent>> where TComponent : class
    {
        protected readonly FactoryMethodComponentFactory<TComponent> Factory;
        private readonly FactoryMethodRegistration _factoryMethodRegistration;

        protected override IComponentRegistration Registration => _factoryMethodRegistration;

        #region Constructors

        public FluentFactoryMethodComponentConfig(ComponentContext context, Func<IComposer, TComponent> factoryMethod)
            : base(context)
        {
            Factory = new FactoryMethodComponentFactory<TComponent>(
                factoryMethod ?? throw new ArgumentNullException(nameof(factoryMethod)));

            _factoryMethodRegistration = new FactoryMethodRegistration(typeof(TComponent));
        }

        #endregion
    }
}