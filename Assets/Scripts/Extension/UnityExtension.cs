namespace DPlay.Extension
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    ///     Adds extension methods for Unity classes.
    /// </summary>
    public static class UnityExtension
    {
        /// <summary>
        ///     Finds a component and assigns it to the reference.
        /// </summary>
        /// <typeparam name="T">The type of the component</typeparam>
        /// <param name="self">The GameObject to find the Component in</param>
        /// <param name="component">The reference to assign to</param>
        /// <returns>Whether a component was found</returns>
        public static bool FindComponent<T>(this GameObject self, out T component)
        {
            component = self.GetComponent<T>();
            return component != null;
        }

        /// <summary>
        ///     Finds a component and assigns it to the reference.
        /// </summary>
        /// <typeparam name="T">The type of the component</typeparam>
        /// <param name="self">The Component to find the Component in</param>
        /// <param name="component">The reference to assign to</param>
        /// <returns>Whether a component was found</returns>
        public static bool FindComponent<T>(this Component self, out T component)
        {
            return UnityExtension.FindComponent(self.gameObject, out component);
        }
    }
}