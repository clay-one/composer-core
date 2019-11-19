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
        private readonly IComposer _parent;

        public ChildComponentContext(IComposer parent)
            : base(false)
        {
            _parent = parent ?? throw new ArgumentNullException(nameof(parent));
        }

        public override bool IsResolvable(Type contract, string name = null)
        {
            return base.IsResolvable(contract, name) || _parent.IsResolvable(contract, name);
        }

        public override object GetComponent(Type contract, string name = null)
        {
            return base.GetComponent(contract, name) ?? _parent.GetComponent(contract, name);
        }

        public override IEnumerable<object> GetAllComponents(Type contract, string name = null)
        {
            return (base.GetAllComponents(contract, name) ?? Enumerable.Empty<object>())
                .Concat(_parent.GetAllComponents(contract, name) ?? Enumerable.Empty<object>())
                .CastToRuntimeType(contract);
        }

        public override IEnumerable<object> GetComponentFamily(Type contract)
        {
            return (base.GetComponentFamily(contract) ?? Enumerable.Empty<object>())
                .Concat(_parent.GetComponentFamily(contract) ?? Enumerable.Empty<object>())
                .CastToRuntimeType(contract);
        }

        public override bool HasVariable(string name)
        {
            return base.HasVariable(name) || _parent.HasVariable(name);
        }

        public override object GetVariable(string name)
        {
            return base.GetVariable(name) ?? _parent.GetVariable(name);
        }
    }
}