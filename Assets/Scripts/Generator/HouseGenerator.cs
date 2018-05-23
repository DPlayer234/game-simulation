namespace DPlay.Generator
{
    using System.Collections.Generic;
    using DPlay.Extension;
    using UnityEngine;

    /// <summary>
    ///     This class generates a single house.
    /// </summary>
    public class HouseGenerator : MonoBehaviour
    {
        /// <summary> The maximum amount of attempts for placing a house </summary>
        private const int MaxPlacementAttempts = 64;

        /// <summary> The radius (in samples) in which to flatten </summary>
        [SerializeField]
        private int flattenSize;

        /// <summary> The prefab to be placed. It is assumed to have its pivot at the bottom. </summary>
        [SerializeField]
        private GameObject housePrefab;

        /// <summary> The chosen position to generate a house at </summary>
        private Vector3 chosenPosition;

        /// <summary> The sample position associated with <seealso cref="chosenPosition"/> </summary>
        private Vector2Int chosenSample;

        /// <summary> The list of all used areas </summary>
        private static List<RectInt> usedAreas;

        /// <summary>
        ///     Initializes the static properties of the <see cref="HouseGenerator"/> class.
        /// </summary>
        static HouseGenerator()
        {
            HouseGenerator.ResetUsedAreas();
        }

        /// <summary>
        ///     Invokes the generation of a house.
        /// </summary>
        public void Invoke()
        {
            GeneratorManager.AssertTerrain();

            this.ChoosePoint();
            this.FlattenArea();
            this.PlaceHouse();
        }

        /// <summary>
        ///     Resets all areas set to be in use by <seealso cref="HouseGenerator"/>s.
        /// </summary>
        public static void ResetUsedAreas()
        {
            HouseGenerator.usedAreas = new List<RectInt>();
        }

        /// <summary>
        ///     Gets a random point that would be valid for generation.
        /// </summary>
        /// <returns>A 2D Point</returns>
        private Vector2Int GetRandomPoint()
        {
            int attempt = 0;

            bool valid;
            Vector2Int samplePosition;

            do
            {
                samplePosition = new Vector2Int(
                    Random.Range(0, GeneratorManager.TerrainData.heightmapWidth - this.flattenSize - 1),
                    Random.Range(0, GeneratorManager.TerrainData.heightmapHeight - this.flattenSize - 1));

                valid = true;

                foreach (RectInt area in HouseGenerator.usedAreas)
                {
                    if (area.Contains(samplePosition) ||
                        area.Contains(samplePosition + new Vector2Int(this.flattenSize, 0)) ||
                        area.Contains(samplePosition + new Vector2Int(0, this.flattenSize)) ||
                        area.Contains(samplePosition + new Vector2Int(this.flattenSize, this.flattenSize)))
                    {
                        valid = false;
                        break;
                    }
                }
            } while (!valid || ++attempt > HouseGenerator.MaxPlacementAttempts);

            return samplePosition;
        }

        /// <summary>
        ///     Chooses a random point and assigns <seealso cref="chosenPosition"/>
        ///     and <seealso cref="chosenSample"/>.
        /// </summary>
        private void ChoosePoint()
        {
            Vector3 terrainSize = GeneratorManager.TerrainData.size;

            // Choose a random sample position
            Vector2Int samplePosition = this.GetRandomPoint();

            // Get the height at that point
            float height = GeneratorManager.TerrainData.GetHeight(samplePosition.x, samplePosition.y);

            // Create Vector and assign it
            this.chosenPosition = new Vector3(
                samplePosition.x * GeneratorManager.TerrainData.heightmapScale.x,
                height,
                samplePosition.y * GeneratorManager.TerrainData.heightmapScale.z);

            this.chosenSample = samplePosition;

            HouseGenerator.usedAreas.Add(new RectInt(this.chosenSample, new Vector2Int(this.flattenSize, this.flattenSize)));
        }

        /// <summary>
        ///     Finds the lowest height near <seealso cref="chosenSample"/> in
        ///     the range of <seealso cref="flattenSize"/> and returns it.
        /// </summary>
        /// <returns>Returns the lowest found height.</returns>
        private float GetAverageHeightInRange()
        {
            float averageHeight = 0.0f;
            int count = 0;

            for (int x = 0; x < this.flattenSize; x++)
            {
                for (int y = 0; y < this.flattenSize; y++)
                {
                    averageHeight += GeneratorManager.TerrainData.GetHeight(
                        this.chosenSample.x + x,
                        this.chosenSample.y + y);

                    ++count;
                }
            }

            averageHeight /= count;

            return averageHeight / GeneratorManager.TerrainData.heightmapScale.y;
        }

        /// <summary>
        ///     Flattens the area surrounding <seealso cref="chosenSample"/>.
        /// </summary>
        private void FlattenArea()
        {
            float averageHeight = this.chosenPosition.y = this.GetAverageHeightInRange();
            
            float[,] localHeights = new float[this.flattenSize, this.flattenSize];

            for (int x = 0; x < this.flattenSize; x++)
            {
                for (int y = 0; y < this.flattenSize; y++)
                {
                    localHeights[x, y] = averageHeight;
                }
            }

            GeneratorManager.TerrainData.SetHeights(this.chosenSample.x, this.chosenSample.y, localHeights);
        }

        /// <summary>
        ///     Places the house.
        /// </summary>
        private void PlaceHouse()
        {
            if (this.housePrefab == null)
            {
                Debug.LogWarning("There is no house-prefab.");
                return;
            }

            Vector3 scale = GeneratorManager.TerrainData.heightmapScale;
            scale.y = 0;

            MonoBehaviour.Instantiate(
                this.housePrefab,
                this.chosenPosition + scale * this.flattenSize,
                Quaternion.identity);
        }
    }
}
