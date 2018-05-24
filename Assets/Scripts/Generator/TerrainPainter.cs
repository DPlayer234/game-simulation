namespace DPlay.Generator
{
    using System.Collections;
    using System.Collections.Generic;
    using DPlay.Extension;
    using UnityEngine;

    /// <summary>
    ///     This paints the terrain based on its height.
    /// </summary>
    [DisallowMultipleComponent]
    public class TerrainPainter : MonoSingleton<TerrainPainter>
    {
        /// <summary> A list of all data to be used for painting the terrain </summary>
        [SerializeField]
        private TerrainPaintData[] paintData;

        /// <summary> Temporary storage for the heightmap width </summary>
        private int heightmapWidth;

        /// <summary> Temporary storage for the heightmap height </summary>
        private int heightmapHeight;

        /// <summary> Temporary storage for the alphamap width </summary>
        private int alphamapWidth;

        /// <summary> Temporary storage for the alphamap height </summary>
        private int alphamapHeight;

        /// <summary>
        ///     Invokes the terrain painting
        /// </summary>
        public void Invoke()
        {
            GeneratorManager.AssertTerrain();
            TerrainHeightGenerator.AssertInstance();

            this.heightmapWidth = GeneratorManager.TerrainData.heightmapWidth;
            this.heightmapHeight = GeneratorManager.TerrainData.heightmapHeight;
            this.alphamapWidth = GeneratorManager.TerrainData.alphamapWidth;
            this.alphamapHeight = GeneratorManager.TerrainData.alphamapHeight;

            this.PaintTerrain();
        }

        /// <summary>
        ///     Paints the terrain with textures
        /// </summary>
        private void PaintTerrain()
        {
            // Create a new empty alphamap
            float[,,] alphamaps = new float[this.alphamapWidth, this.alphamapHeight, GeneratorManager.TerrainData.alphamapLayers];

            // Iterate and set the alphamap
            for (int x = 0; x < this.alphamapWidth; x++)
            {
                for (int y = 0; y < this.alphamapHeight; y++)
                {
                    float height = this.GetTerrainHeight(x, y);
                    int total = 0;

                    for (int i = 0; i < this.paintData.Length; i++)
                    {
                        total += this.paintData[i].IsInRange(height) ? 1 : 0;
                    }

                    for (int i = 0; i < this.paintData.Length; i++)
                    {
                        alphamaps[x, y, i] = this.paintData[i].IsInRange(height) ? 1.0f / total : 0.0f;
                    }
                }
            }

            // Update Alphamap
            GeneratorManager.TerrainData.SetAlphamaps(0, 0, alphamaps);
        }

        /// <summary>
        ///     Returns the terrain height at a given point on the alphamap.
        /// </summary>
        /// <returns>The height</returns>
        private float GetTerrainHeight(int alphaX, int alphaY)
        {
            return TerrainHeightGenerator.Instance.GetHeight(
                (float)alphaX / this.alphamapWidth * this.heightmapWidth,
                (float)alphaY / this.alphamapHeight * this.heightmapHeight);
        }

        /// <summary>
        ///     Called by Unity to initialize the <see cref="TerrainPainter"/>.
        /// </summary>
        private void Awake()
        {
            this.NewPreferOld(false);
        }
    }
}