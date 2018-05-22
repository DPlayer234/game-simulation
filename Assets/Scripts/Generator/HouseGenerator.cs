namespace DPlay.Generator
{
    using DPlay.Extension;
    using UnityEngine;

    /// <summary>
    ///     This class generates a single house.
    /// </summary>
    public class HouseGenerator : MonoBehaviour
    {
        /// <summary> The radius (in samples) in which to flatten </summary>
        [SerializeField]
        private int flattenRadius;

        /// <summary> The prefab to be placed. It is assumed to have its pivot at the bottom. </summary>
        [SerializeField]
        private GameObject housePrefab;

        /// <summary> The chosen position to generate a house at </summary>
        private Vector3 chosenPosition;

        /// <summary> The sample position associated with <seealso cref="chosenPosition"/> </summary>
        private Vector2Int chosenSample;

        /// <summary>
        ///     Invokes the generation of a house.
        /// </summary>
        public void Invoke()
        {
            if (GeneratorManager.Terrain == null)
                throw new GeneratorException("There is no Terrain attached to this GameObject.");

            this.ChoosePoint();
            this.FlattenArea();
            this.PlaceHouse();
        }

        /// <summary>
        ///     Chooses a random point and assigns <seealso cref="chosenPosition"/>
        ///     and <seealso cref="chosenSample"/>.
        /// </summary>
        private void ChoosePoint()
        {
            Vector3 terrainSize = GeneratorManager.TerrainData.size;

            // Choose a random sample position
            Vector2Int samplePosition = new Vector2Int(
                Random.Range(this.flattenRadius, GeneratorManager.TerrainData.heightmapWidth - this.flattenRadius - 1),
                Random.Range(this.flattenRadius, GeneratorManager.TerrainData.heightmapHeight - this.flattenRadius - 1));

            // Get the height at that point
            float height = GeneratorManager.TerrainData.GetHeight(samplePosition.x, samplePosition.y);

            // Create Vector and assign it
            this.chosenPosition = new Vector3(
                samplePosition.x * GeneratorManager.TerrainData.heightmapScale.x,
                height,
                samplePosition.y * GeneratorManager.TerrainData.heightmapScale.z);

            this.chosenSample = samplePosition;
        }

        /// <summary>
        ///     Finds the lowest height near <seealso cref="chosenSample"/> in
        ///     the range of <seealso cref="flattenRadius"/> and returns it.
        /// </summary>
        /// <returns>Returns the lowest found height.</returns>
        private float GetAverageHeightInRange()
        {
            float averageHeight = 0.0f;
            int count = 0;

            for (int x = -this.flattenRadius; x < this.flattenRadius + 1; x++)
            {
                for (int y = -this.flattenRadius; y < this.flattenRadius + 1; y++)
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

            int diameter = this.flattenRadius * 2 + 1;
            float[,] localHeights = new float[diameter, diameter];

            for (int x = 0; x < diameter; x++)
            {
                for (int y = 0; y < diameter; y++)
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

            MonoBehaviour.Instantiate(this.housePrefab, this.chosenPosition, Quaternion.identity);
        }
    }
}
