using System;
using System.Collections.Generic;
using ComposerCore.Cache;
using ComposerCore.CompositionalQueries;
using ComposerCore.Extensibility;
using ComposerCore.Factories;
using ComposerCore.Implementation;

namespace ComposerCore.FluentExtensions
{
    public class FluentUntypedFactoryMethodComponentConfig : FluentComponentConfigBase<FluentUntypedFactoryMethodComponentConfig>
    {
        private readonly FactoryMethodRegistration _factoryMethodRegistration;

        protected override IComponentRegistration Registration => _factoryMethodRegistration;

        protected readonly UntypedFactoryMethodComponentFactory Factory;

        #region Constructors

        public FluentUntypedFactoryMethodComponentConfig(ComponentContext context, Func<IComposer, object> factoryMethod)
            : base(context)
        {
            Factory = new UntypedFactoryMethodComponentFactory(factoryMethod);
            _factoryMethodRegistration = new FactoryMethodRegistration(typeof(object));
        }

        #endregion
    }
}