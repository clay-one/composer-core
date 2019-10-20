using System;
using ComposerCore.Factories;
using ComposerCore.Implementation;

namespace ComposerCore.FluentExtensions
{
    public class FluentFactoryMethodComponentConfig<TComponent> 
        : FluentComponentConfigBase<FluentFactoryMethodComponentConfig<TComponent>> where TComponent : class
    {
        protected readonly FactoryMethodComponentFactory<TComponent> Factory;

        #region Constructors

        public FluentFactoryMethodComponentConfig(ComponentContext context, Func<IComposer, TComponent> factoryMethod)
            : base(context)
        {
            Factory = new FactoryMethodComponentFactory<TComponent>(
                factoryMethod ?? throw new ArgumentNullException(nameof(factoryMethod)));

            Registration = new ComponentRegistration(Factory);
        }

        #endregion
    }
}