using System;
using System.Collections.Generic;
using System.Linq;

using RI.Utilities.Collections;




namespace RI.Utilities.ComponentModel
{
    /// <summary>
    ///     Implements a wrapper for <see cref="IDependencyResolver" /> and <see cref="IServiceProvider" /> which also allows modification/interception of resolved instances.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         <see cref="DependencyResolverWrapper" /> can also be used to wrap a <see cref="IDependencyResolver" /> as a <see cref="IServiceProvider" /> and vice-versa.
    ///     </para>
    /// </remarks>
    /// <threadsafety static="false" instance="false" />
    /// TODO: Make thread-safe
    public class DependencyResolverWrapper : IDependencyResolver, IServiceProvider
    {
        #region Instance Constructor/Destructor

        /// <summary>
        ///     Creates a new instance of <see cref="DependencyResolverWrapper" />.
        /// </summary>
        /// <param name="dependencyResolver"> The dependency resolver. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="dependencyResolver" /> is null. </exception>
        public DependencyResolverWrapper (IDependencyResolver dependencyResolver)
        {
            if (dependencyResolver == null)
            {
                throw new ArgumentNullException(nameof(dependencyResolver));
            }

            this.DependencyResolver = dependencyResolver;
        }

        /// <summary>
        ///     Creates a new instance of <see cref="DependencyResolverWrapper" />.
        /// </summary>
        /// <param name="serviceProvider"> The service provider. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="serviceProvider" /> is null. </exception>
        public DependencyResolverWrapper (IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            this.ServiceProvider = serviceProvider;
        }

        #endregion




        #region Instance Properties/Indexer

        /// <summary>
        ///     Gets the used dependency resolver.
        /// </summary>
        /// <value>
        ///     The used dependency resolver or null if no dependency resolver is used.
        /// </value>
        public IDependencyResolver DependencyResolver { get; }

        /// <summary>
        ///     Gets the used service provider.
        /// </summary>
        /// <value>
        ///     The used service provider or null if no service provider is used.
        /// </value>
        public IServiceProvider ServiceProvider { get; }

        #endregion




        #region Instance Methods

        private List<object> GetListFromServiceProvider (Type type)
        {
            List<object> instances = new List<object>();
            object instance = this.ServiceProvider?.GetService(type);

            if (instance != null)
            {
                instances.Add(instance);
            }

            return instances;
        }

        private List<T> InterceptInternal <T> (List<T> list)
            where T : class
        {
            return this.InterceptInternal(typeof(T), list.OfType<object>()
                                                         .ToList())
                       .OfType<T>()
                       .ToList();
        }

        private List<object> InterceptInternal (string name, List<object> list)
        {
            this.Intercept(name, list);
            return list;
        }

        private List<object> InterceptInternal (Type type, List<object> list)
        {
            this.Intercept(type, list);
            return list;
        }

        #endregion




        #region Virtuals

        /// <summary>
        ///     Intercepts instance resolving by name.
        /// </summary>
        /// <param name="name"> The name to resolve. </param>
        /// <param name="instances"> The list of instances already resolved by <see cref="DependencyResolver" /> or <see cref="ServiceProvider" /> which can be modified to perform the interception. </param>
        /// <remarks>
        ///     <para>
        ///         The default implementation does nothing and so passes through all instances already in <paramref name="instances" />.
        ///     </para>
        /// </remarks>
        protected virtual void Intercept (string name, List<object> instances) { }

        /// <summary>
        ///     Intercepts instance resolving by type.
        /// </summary>
        /// <param name="type"> The type to resolve. </param>
        /// <param name="instances"> The list of instances already resolved by <see cref="DependencyResolver" /> or <see cref="ServiceProvider" /> which can be modified to perform the interception. </param>
        /// <remarks>
        ///     <para>
        ///         The default implementation does nothing and so passes through all instances already in <paramref name="instances" />.
        ///     </para>
        /// </remarks>
        protected virtual void Intercept (Type type, List<object> instances) { }

        #endregion




        #region Interface: IDependencyResolver

        /// <inheritdoc />
        public object GetInstance (Type type)
        {
            return this.InterceptInternal(type, this.DependencyResolver == null ? this.GetListFromServiceProvider(type) : this.DependencyResolver.GetInstances(type))
                       .AsList()
                       .GetIndexOrDefault(0);
        }

        /// <inheritdoc />
        public object GetInstance (string name)
        {
            return this.InterceptInternal(name, this.DependencyResolver == null ? new List<object>() : this.DependencyResolver.GetInstances(name))
                       .AsList()
                       .GetIndexOrDefault(0);
        }

        /// <inheritdoc />
        public T GetInstance <T> ()
            where T : class
        {
            return this.InterceptInternal(this.DependencyResolver == null ? this.GetListFromServiceProvider(typeof(T))
                                                                                .OfType<T>()
                                                                                .ToList() : this.DependencyResolver.GetInstances<T>())
                       .AsList()
                       .GetIndexOrDefault(0);
        }

        /// <inheritdoc />
        public List<object> GetInstances (Type type)
        {
            return this.InterceptInternal(type, this.DependencyResolver == null ? this.GetListFromServiceProvider(type) : this.DependencyResolver.GetInstances(type));
        }

        /// <inheritdoc />
        public List<object> GetInstances (string name)
        {
            return this.InterceptInternal(name, this.DependencyResolver == null ? new List<object>() : this.DependencyResolver.GetInstances(name));
        }

        /// <inheritdoc />
        public List<T> GetInstances <T> ()
            where T : class
        {
            return this.InterceptInternal(this.DependencyResolver == null ? this.GetListFromServiceProvider(typeof(T))
                                                                                .OfType<T>()
                                                                                .ToList() : this.DependencyResolver.GetInstances<T>());
        }

        #endregion




        #region Interface: IServiceProvider

        /// <inheritdoc />
        public object GetService (Type serviceType)
        {
            return this.GetInstance(serviceType);
        }

        #endregion
    }
}
