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
        private readonly UntypedFactoryMethodRegistration _factoryMethodRegistration;

        protected override IComponentRegistration Registration => _factoryMethodRegistration;

        #region Constructors

        public FluentUntypedFactoryMethodComponentConfig(ComponentContext context, Func<IComposer, object> factoryMethod)
            : base(context)
        {
            _factoryMethodRegistration = new UntypedFactoryMethodRegistration(factoryMethod);
        }

        #endregion
    }
}