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

        protected internal override object GetComponentInternal(Type contract, string name, IComposer scope)
        {
            return base.GetComponentInternal(contract, name, scope) ?? _parent.GetComponentInternal(contract, name, scope);
        }

        protected internal override IEnumerable<object> GetAllComponentsInternal(Type contract, string name, IComposer scope)
        {
            return (base.GetAllComponentsInternal(contract, name, scope) ?? Enumerable.Empty<object>())
                .Concat(_parent.GetAllComponentsInternal(contract, name, scope) ?? Enumerable.Empty<object>())
                .CastToRuntimeType(contract);
        }

        protected internal override IEnumerable<object> GetComponentFamilyInternal(Type contract, IComposer scope)
        {
            return (base.GetComponentFamilyInternal(contract, scope) ?? Enumerable.Empty<object>())
                .Concat(_parent.GetComponentFamilyInternal(contract, scope) ?? Enumerable.Empty<object>())
                .CastToRuntimeType(contract);
        }
    }
}