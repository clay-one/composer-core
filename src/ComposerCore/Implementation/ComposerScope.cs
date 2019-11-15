using System;
using System.Collections.Generic;

namespace ComposerCore.Implementation
{
    public class ComposerScope : IComposer
    {
        private readonly IComposer _parent;

        public ComposerScope(IComposer parent)
        {
            _parent = parent;
        }

        public void Dispose()
        {
            _parent.Dispose();
        }

        public ComposerConfiguration Configuration => _parent.Configuration;
        public bool IsResolvable(Type contract, string name = null)
        {
            return _parent.IsResolvable(contract, name);
        }

        public object GetComponent(Type contract, string name = null)
        {
            return _parent.GetComponent(contract, name);
        }

        public IEnumerable<object> GetAllComponents(Type contract, string name = null)
        {
            return _parent.GetAllComponents(contract, name);
        }

        public IEnumerable<object> GetComponentFamily(Type contract)
        {
            return _parent.GetComponentFamily(contract);
        }

        public bool HasVariable(string name)
        {
            return _parent.HasVariable(name);
        }

        public object GetVariable(string name)
        {
            return _parent.GetVariable(name);
        }

        public void InitializePlugs(object componentInstance, Type componentType)
        {
            _parent.InitializePlugs(componentInstance, componentType);
        }
    }
}