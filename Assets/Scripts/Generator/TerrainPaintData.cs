namespace DPlay.Generator
{
    using System;
    using DPlay.Extension;
    using UnityEngine;

    /// <summary>
    ///     This stores information about how the <seealso cref="TerrainPainter"/> acts.
    /// </summary>
    [System.Serializable]
    public class TerrainPaintData : IComparable<TerrainPaintData>
    {
        /// <summary> The index of the texture </summary>
        public int TextureIndex;

        /// <summary> The minimum height the texture is used at </summary>
        [Range(0f, 1f)]
        public float MinimumHeight;

        /// <summary> The maximum height the texture is used at </summary>
        [Range(0f, 1f)]
        public float MaximumHeight;

        /// <summary>
        ///     Returns an integer indicating the "larger" element.
        /// </summary>
        /// <param name="other">The other element</param>
        /// <returns>An integer indicating the "larger" element.</returns>
        int IComparable<TerrainPaintData>.CompareTo(TerrainPaintData other)
        {
            return -this.MinimumHeight.CompareTo(other.MinimumHeight);
        }
    }
}
