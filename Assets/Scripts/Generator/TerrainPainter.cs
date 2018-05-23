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
    public class TerrainPainter : Singleton<TerrainPainter>
    {
        /// <summary> A list of all data to be used for painting the terrain </summary>
        [SerializeField]
        private TerrainPaintData[] paintData;

        /// <summary> Temporary storage for the heights of the terrain </summary>
        private float[,] heights;

        /// <summary>
        ///     Invokes the terrain painting
        /// </summary>
        public void Invoke()
        {
            GeneratorManager.AssertTerrain();

            int heightmapWidth = GeneratorManager.TerrainData.heightmapWidth;
            int heightmapHeight = GeneratorManager.TerrainData.heightmapHeight;

            this.heights = GeneratorManager.TerrainData.GetHeights(0, 0, heightmapWidth, heightmapHeight);

            this.PaintTerrain();
            this.ClearData();
        }

        /// <summary>
        ///     Paints the terrain with textures
        /// </summary>
        private void PaintTerrain()
        {
            // Set the alphamap resolution to match the heightmap resolution
            GeneratorManager.TerrainData.alphamapResolution = GeneratorManager.TerrainData.heightmapResolution;

            int alphamapWidth = GeneratorManager.TerrainData.alphamapWidth;
            int alphamapHeight = GeneratorManager.TerrainData.alphamapHeight;

            float[,,] alphamaps = GeneratorManager.TerrainData.GetAlphamaps(0, 0, alphamapWidth, alphamapHeight);

            // Iterate and set the alphamap
            for (int x = 0; x < alphamapWidth; x++)
            {
                for (int y = 0; y < alphamapHeight; y++)
                {
                    float height = this.heights[x, y];
                    float total = 0.0f;

                    for (int i = 0; i < this.paintData.Length; i++)
                    {
                        var data = this.paintData[i];

                        total += (height > data.MinimumHeight && height <= data.MaximumHeight) ? 1 : 0;
                    }

                    for (int i = 0; i < this.paintData.Length; i++)
                    {
                        var data = this.paintData[i];

                        alphamaps[x, y, data.TextureIndex] =
                            (height > data.MinimumHeight && height <= data.MaximumHeight) ? 1 / total : 0;
                    }
                }
            }

            // Update Alphamap
            GeneratorManager.TerrainData.SetAlphamaps(0, 0, alphamaps);
        }

        /// <summary>
        ///     Clears data no longer in use
        /// </summary>
        private void ClearData()
        {
            this.heights = null;
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