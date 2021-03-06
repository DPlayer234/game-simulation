namespace DPlay.Extension
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    ///     Adds extension methods for Lists and Collections
    /// </summary>
    public static class ListExtension
    {
        /// <summary>
        ///     Returns an array of all components in a given collection of <seealso cref="GameObject"/>s
        /// </summary>
        /// <typeparam name="T">The type of component to get</typeparam>
        /// <param name="gameObjects">The collection of <seealso cref="GameObject"/>s</param>
        /// <returns>An array of all components</returns>
        public static T[] GetComponentsInCollection<T>(this ICollection<GameObject> gameObjects)
        {
            T[] components = new T[gameObjects.Count];

            int i = 0;
            foreach (GameObject gameObject in gameObjects)
            {
                components[i++] = gameObject.GetComponent<T>();
            }

            return components;
        }

        /// <summary>
        ///     Splits the original array into an array of equal lenght with each element being another array,
        ///     containing solely one item of the original array.
        /// </summary>
        /// <typeparam name="T">The type of items of the original array</typeparam>
        /// <param name="originalArray">The original array (or IList)</param>
        /// <returns>A new array</returns>
        public static T[][] SplitIntoArrayOfLengthOneArrays<T>(this IList<T> originalArray)
        {
            T[][] newArray = new T[originalArray.Count][];

            for (int i = 0; i < originalArray.Count; i++)
            {
                newArray[i] = new T[]
                {
                    originalArray[i]
                };
            }

            return newArray;
        }

        /// <summary>
        ///     Returns a random item from a list
        /// </summary>
        /// <typeparam name="T">The type of the elements in <paramref name="list"/></typeparam>
        /// <param name="list">The list to pick an element from</param>
        /// <returns>A random element</returns>
        public static T GetRandomItem<T>(this IList<T> list)
        {
            if (list.Count == 0)
                throw new InvalidOperationException("Cannot get random item from an empty IList.");

            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        /// <summary>
        ///     Returns the element at <paramref name="index"/> from <paramref name="list"/>
        ///     or the default value if there is none.
        /// </summary>
        /// <typeparam name="T">The type of the elements in <paramref name="list"/></typeparam>
        /// <param name="list">The list</param>
        /// <param name="index">The index</param>
        /// <returns>The element at <paramref name="index"/> or the default</returns>
        public static T GetValueSafe<T>(this IList<T> list, int index)
        {
            if (index >= 0 && index < list.Count)
            {
                return list[index];
            }

            return default(T);
        }

        /// <summary>
        ///     Returns the item at <paramref name="index"/> % <paramref name="list"/>.Count
        /// </summary>
        /// <typeparam name="T">The type of the list content</typeparam>
        /// <param name="list">The list</param>
        /// <param name="index">The wrapping index</param>
        /// <returns>The item at the given position</returns>
        public static T GetItemWrap<T>(this IList<T> list, int index)
        {
            return list[index % list.Count];
        }

        /// <summary>
        ///     Creates a flat array out of a 2-dimensional array.
        /// </summary>
        /// <param name="array">The array to flatten</param>
        /// <returns>A flattened array with the same elements</returns>
        public static T[] GetFlatArray<T>(this T[,] array)
        {
            T[] flat = new T[array.GetLength(0) * array.GetLength(1)];

            int i = 0;

            for (int x = 0; x < array.GetLength(0); x++)
            {
                for (int y = 0; y < array.GetLength(1); y++)
                {
                    flat[i++] = array[x, y];
                }
            }

            return flat;
        }

        /// <summary>
        ///     Creates a flat array out of a 3-dimensional array.
        /// </summary>
        /// <param name="array">The array to flatten</param>
        /// <returns>A flattened array with the same elements</returns>
        public static T[] GetFlatArray<T>(this T[,,] array)
        {
            T[] flat = new T[array.GetLength(0) * array.GetLength(1) * array.GetLength(2)];

            int i = 0;

            for (int x = 0; x < array.GetLength(0); x++)
            {
                for (int y = 0; y < array.GetLength(1); y++)
                {
                    for (int z = 0; z < array.GetLength(2); z++)
                    {
                        flat[i++] = array[x, y, z];
                    }
                }
            }

            return flat;
        }
    }
}
