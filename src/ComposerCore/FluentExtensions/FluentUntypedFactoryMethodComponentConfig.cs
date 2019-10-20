using System;
using System.Collections.Generic;
using ComposerCore.Cache;
using ComposerCore.CompositionalQueries;
using ComposerCore.Factories;
using ComposerCore.Implementation;

namespace ComposerCore.FluentExtensions
{
    public class FluentUntypedFactoryMethodComponentConfig : FluentComponentConfigBase<FluentUntypedFactoryMethodComponentConfig>
    {
        protected readonly UntypedFactoryMethodComponentFactory Factory;

        #region Constructors

        public FluentUntypedFactoryMethodComponentConfig(ComponentContext context, Func<IComposer, object> factoryMethod)
            : base(context)
        {
            Factory = new UntypedFactoryMethodComponentFactory(factoryMethod);
            Registration = new ComponentRegistration(Factory);
        }

        #endregion
    }
}