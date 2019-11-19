using System;
using System.Collections.Generic;
using System.Linq;
using ComposerCore.Cache;
using ComposerCore.Implementation;

namespace ComposerCore
{
    public static class ComposerExtensions
    {
        #region Query utilities
        
        public static bool IsResolvable<TContract>(this IComposer composer, string name = null) where TContract : class
        {
            return composer.IsResolvable(typeof(TContract), name);
        }
        
        public static TContract GetComponent<TContract>(this IComposer composer, string name = null)
            where TContract : class
        {
            return (TContract) composer.GetComponent(typeof (TContract), name);
        }

        public static IEnumerable<TContract> GetAllComponents<TContract>(this IComposer composer, string name = null)
            where TContract : class
        {
            return composer.GetAllComponents(typeof (TContract), name).Cast<TContract>();
        }

        public static IEnumerable<TContract> GetComponentFamily<TContract>(this IComposer composer)
        {
            return composer.GetComponentFamily(typeof (TContract)).Cast<TContract>();
        }

        public static void InitializePlugs<T>(this IComposer composer, T componentInstance)
        {
            composer.InitializePlugs(componentInstance, typeof (T));
        }
        
        #endregion

        #region Lazy query utilities

        // GetComponent overloads
		
        public static Lazy<TContract> LazyGetComponent<TContract>(this IComposer composer, string name = null) 
            where TContract : class
        {
            return new Lazy<TContract>(() => composer.GetComponent<TContract>(name));
        }
		
        public static Lazy<object> LazyGetComponent(this IComposer composer, Type contract, string name = null)
        {
            return new Lazy<object>(() => composer.GetComponent(contract, name));
        }

        // GetAllComponents overloads
		
        public static Lazy<IEnumerable<TContract>> LazyGetAllComponents<TContract>(this IComposer composer, string name = null)
            where TContract : class
        {
            return new Lazy<IEnumerable<TContract>>(() => composer.GetAllComponents<TContract>(name));
        }
		
        public static Lazy<IEnumerable<object>> LazyGetAllComponents(this IComposer composer, Type contract, string name = null)
        {
            return new Lazy<IEnumerable<object>>(() => composer.GetAllComponents(contract, name));
        }

        // GetComponentFamily overloads

        public static Lazy<IEnumerable<TContract>> LazyGetComponentFamily<TContract>(this IComposer composer)
        {
            return new Lazy<IEnumerable<TContract>>(composer.GetComponentFamily<TContract>);
        }

        public static Lazy<IEnumerable<object>> LazyGetComponentFamily(this IComposer composer, Type contract)
        {
            return new Lazy<IEnumerable<object>>(() => composer.GetComponentFamily(contract));
        }

        // GetVariable overloads

        public static Lazy<object> LazyGetVariable(this IComposer composer, string name)
        {
            return new Lazy<object>(() => composer.GetVariable(name));
        }

        #endregion
        
        #region Scope utilities

        public static IComposer CreateScope(this IComposer composer)
        {
            var result = new ChildComponentContext(composer);
            result.Register(typeof(ScopedComponentCacheStore));
            
            return result;
        }
        
        #endregion
    }
}