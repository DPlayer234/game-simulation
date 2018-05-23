namespace DPlay.Generator
{
    using DPlay.Extension;
    using UnityEngine;

    /// <summary>
    ///     This generates the terrain's height.
    /// </summary>
    [DisallowMultipleComponent]
    public class TerrainHeightGenerator : MonoSingleton<TerrainHeightGenerator>
    {
        /// <summary> This is the multiplier for the terrain height. </summary>
        [SerializeField]
        [Range(0f, 1f)]
        private float heightScale;

        /// <summary> This is the multiplier for the noise dimensions. </summary>
        [SerializeField]
        [Range(0f, 0.1f)]
        private float noiseScale;

        /// <summary> Random generator "position" offset for noise </summary>
        private Vector2 randomOffset;

        /// <summary>
        ///     Invokes the random terrain generation.
        /// </summary>
        public void Invoke()
        {
            GeneratorManager.AssertTerrain();

            this.GenerateTerrain();
        }

        /// <summary>
        ///     Returns the height of the terrain for a given point.
        /// </summary>
        /// <param name="position">The position to get the height for.</param>
        /// <returns>The height of the terrain.</returns>
        private float GetHeight(float x, float y)
        {
            x += this.randomOffset.x;
            y += this.randomOffset.y; 
            return this.heightScale * Mathf.PerlinNoise(x * this.noiseScale, y * this.noiseScale);
        }

        /// <summary>
        ///     Generates the terrain.
        /// </summary>
        private void GenerateTerrain()
        {
            // Get the referrence to the TerrainData
            TerrainData data = GeneratorManager.TerrainData;

            float[,] heights = new float[data.heightmapWidth, data.heightmapHeight];

            // Get new height values
            for (int x = 0; x < data.heightmapWidth; x++)
            {
                for (int y = 0; y < data.heightmapHeight; y++)
                {
                    heights[x, y] = this.GetHeight(x, y);
                }
            }

            // Update the height data
            data.SetHeights(0, 0, heights);
        }

        /// <summary>
        ///     Returns a new random offset. Also assigns it to <seealso cref="randomOffset"/>
        /// </summary>
        /// <returns>A new random offset.</returns>
        private Vector2 GetNewRandomOffset()
        {
            const float range = 1e5f;

            return this.randomOffset = new Vector2(Random.Range(-range, range), Random.Range(-range, range));
        }

        /// <summary>
        ///     Called by Unity to initialize the <see cref="TerrainHeightGenerator"/> class.
        /// </summary>
        private void Awake()
        {
            this.NewPreferOld(false);

            this.GetNewRandomOffset();
        }
    }
}
