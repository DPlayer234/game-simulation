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
        
        /// <summary> Temporary storage for the alphamap width </summary>
        private int alphamapWidth;

        /// <summary> Temporary storage for the alphamap height </summary>
        private int alphamapHeight;

        /// <summary> Temporary storage for the height scale </summary>
        private float heightScale;

        /// <summary>
        ///     Invokes the terrain painting
        /// </summary>
        public void Invoke()
        {
            GeneratorManager.AssertTerrain();
            
            this.alphamapWidth = GeneratorManager.TerrainData.alphamapWidth;
            this.alphamapHeight = GeneratorManager.TerrainData.alphamapHeight;
            this.heightScale = GeneratorManager.TerrainData.heightmapScale.y;

            this.PaintTerrain();
        }

        /// <summary>
        ///     Paints the terrain with textures
        /// </summary>
        private void PaintTerrain()
        {
            // Get the current alphamap for convinience
            float[,,] alphamaps = GeneratorManager.TerrainData.GetAlphamaps(0, 0, this.alphamapWidth, this.alphamapHeight);

            // Iterate and set the alphamap
            for (int x = 0; x < this.alphamapWidth; x++)
            {
                for (int y = 0; y < this.alphamapHeight; y++)
                {
                    float height = this.GetTerrainHeight(x, y);
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
        ///     Returns the terrain height at a given point on the alphamap.
        /// </summary>
        /// <returns>The height</returns>
        private float GetTerrainHeight(int alphaX, int alphaY)
        {
            float height = GeneratorManager.TerrainData.GetInterpolatedHeight(
                (float)alphaX / this.alphamapWidth,
                (float)alphaY / this.alphamapHeight);

            return height / this.heightScale;
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