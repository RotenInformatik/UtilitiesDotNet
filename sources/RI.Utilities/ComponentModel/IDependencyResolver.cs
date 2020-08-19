using System;
using System.Collections.Generic;

using RI.Utilities.Exceptions;




namespace RI.Utilities.ComponentModel
{
    /// <summary>
    ///     Defines the interface for a dependency resolver which can be used to obtain instances of required types.
    /// </summary>
    /// <preliminary />
    public interface IDependencyResolver
    {
        /// <summary>
        ///     Gets an instance of the specified type.
        /// </summary>
        /// <param name="type"> The type. </param>
        /// <returns>
        ///     The instance of <paramref name="type" /> or null if no instance is available.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="type" /> is null. </exception>
        /// <exception cref="InvalidTypeArgumentException"> <paramref name="type" /> is not a class type. </exception>
        object GetInstance (Type type);

        /// <summary>
        ///     Gets an instance of the specified name.
        /// </summary>
        /// <param name="name"> The name. </param>
        /// <returns>
        ///     The instance associated with <paramref name="name" /> or null if no instance is available.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="name" /> is null. </exception>
        /// <exception cref="EmptyStringArgumentException"> <paramref name="name" /> is an empty string. </exception>
        object GetInstance (string name);

        /// <summary>
        ///     Gets an instance of the specified type.
        /// </summary>
        /// <typeparam name="T"> The type. </typeparam>
        /// <returns>
        ///     The instance of <typeparamref name="T" /> or null if no instance is available.
        /// </returns>
        T GetInstance <T> ()
            where T : class;

        /// <summary>
        ///     Gets all instances of a specified type.
        /// </summary>
        /// <param name="type"> The type. </param>
        /// <returns>
        ///     The list which contains all instances of <paramref name="type" />.
        ///     If no instances are available, an empty list is returned.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="type" /> is null. </exception>
        /// <exception cref="InvalidTypeArgumentException"> <paramref name="type" /> is not a class type. </exception>
        List<object> GetInstances (Type type);

        /// <summary>
        ///     Gets all instances of a specified name.
        /// </summary>
        /// <param name="name"> The name. </param>
        /// <returns>
        ///     The list which contains all instances associated with <paramref name="name" />.
        ///     If no instances are available, an empty list is returned.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="name" /> is null. </exception>
        /// <exception cref="EmptyStringArgumentException"> <paramref name="name" /> is an empty string. </exception>
        List<object> GetInstances (string name);

        /// <summary>
        ///     Gets all instances of a specified type.
        /// </summary>
        /// <typeparam name="T"> The type. </typeparam>
        /// <returns>
        ///     The list which contains all instances of <typeparamref name="T" />.
        ///     If no instances are available, an empty list is returned.
        /// </returns>
        List<T> GetInstances <T> ()
            where T : class;
    }
}
