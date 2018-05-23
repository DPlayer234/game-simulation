namespace DPlay.Generator
{
    using DPlay.Extension;
    using UnityEngine;

    /// <summary>
    ///     The component manages and invokes the random generation.
    /// </summary>
    [DisallowMultipleComponent]
    public class GeneratorManager : Singleton<GeneratorManager>
    {
        /// <summary> The terrain to be used in the generation. </summary>
        [SerializeField]
        private Terrain terrain;

        /// <summary>
        ///     The Terrain to be used in the generation.
        /// </summary>
        public static Terrain Terrain
        {
            get
            {
                return GeneratorManager.Instance.terrain;
            }
        }

        /// <summary>
        ///     Returns the used TerrainData.
        /// </summary>
        public static TerrainData TerrainData
        {
            get
            {
                if (GeneratorManager.Terrain == null) return null;
                return GeneratorManager.Terrain.terrainData;
            }
        }

        /// <summary>
        ///     Returns the attached Terrain or throws an exception.
        /// </summary>
        /// <returns>The Terrain</returns>
        public static Terrain AssertTerrain()
        {
            if (GeneratorManager.Terrain == null)
                throw new GeneratorException("There is no Terrain attached to this GameObject.");

            return GeneratorManager.Terrain;
        }

        /// <summary>
        ///     Called by Unity to initialize the <see cref="GeneratorManager"/> class.
        /// </summary>
        private void Awake()
        {
            this.NewPreferOld(false);

            if (this.terrain == null)
            {
                if (!this.FindComponent(out this.terrain))
                    throw new GeneratorException("There is no Terrain assigned to the GeneratorManager.");
            }
        }

        /// <summary>
        ///     Called by Unity to initialize the <see cref="GeneratorManager"/> class
        ///     once it first becomes active.
        /// </summary>
        private void Start()
        {
            this.InvokeTerrainHeightGenerator();
            this.InvokeHouseGenerators();
            this.InvokeTerrainPainter();
        }

        /// <summary>
        ///     Invokes the terrain height generator.
        /// </summary>
        private void InvokeTerrainHeightGenerator()
        {
            if (TerrainHeightGenerator.Instance == null)
            {
                Debug.LogWarning("There is no TerrainHeightGenerator.");
                return;
            }

            TerrainHeightGenerator.Instance.Invoke();
        }

        /// <summary>
        ///     Invokes the terrain painter.
        /// </summary>
        private void InvokeTerrainPainter()
        {
            if (TerrainPainter.Instance == null)
            {
                Debug.LogWarning("There is no TerrainPainter.");
                return;
            }

            TerrainPainter.Instance.Invoke();
        }

        /// <summary>
        ///     Invokes all house generators.
        /// </summary>
        private void InvokeHouseGenerators()
        {
            HouseGenerator.ResetUsedAreas();

            HouseGenerator[] houseGenerators = this.GetComponentsInChildren<HouseGenerator>();

            foreach (HouseGenerator houseGenerator in houseGenerators)
            {
                houseGenerator.Invoke();
            }
        }
    }
}
