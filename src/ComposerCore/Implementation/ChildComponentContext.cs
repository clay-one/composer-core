using System;
using System.Collections.Generic;
using System.Linq;
using ComposerCore.Attributes;
using ComposerCore.Cache;
using ComposerCore.Utility;

namespace ComposerCore.Implementation
{
    [Component, ComponentCache(typeof(NoComponentCache))]
    public class ChildComponentContext : ComponentContext
    {
        private readonly ComponentContext _parent;

        public ChildComponentContext(ComponentContext parent)
            : base(false)
        {
            _parent = parent ?? throw new ArgumentNullException(nameof(parent));
        }

        public override bool IsResolvable(Type contract, string name = null)
        {
            return base.IsResolvable(contract, name) || _parent.IsResolvable(contract, name);
        }

        public override bool HasVariable(string name)
        {
            return base.HasVariable(name) || _parent.HasVariable(name);
        }

        public override object GetVariable(string name)
        {
            return base.GetVariable(name) ?? _parent.GetVariable(name);
        }

        protected internal override object GetComponent(Type contract, string name, IComposer dependencyResolver)
        {
            return base.GetComponent(contract, name, dependencyResolver) ?? _parent.GetComponent(contract, name, dependencyResolver);
        }

        protected internal override IEnumerable<object> GetAllComponents(Type contract, string name, IComposer dependencyResolver)
        {
            return (base.GetAllComponents(contract, name, dependencyResolver) ?? Enumerable.Empty<object>())
                .Concat(_parent.GetAllComponents(contract, name, dependencyResolver) ?? Enumerable.Empty<object>())
                .CastToRuntimeType(contract);
        }

        protected internal override IEnumerable<object> GetComponentFamily(Type contract, IComposer dependencyResolver)
        {
            return (base.GetComponentFamily(contract, dependencyResolver) ?? Enumerable.Empty<object>())
                .Concat(_parent.GetComponentFamily(contract, dependencyResolver) ?? Enumerable.Empty<object>())
                .CastToRuntimeType(contract);
        }
    }
}