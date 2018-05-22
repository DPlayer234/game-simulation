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
        ///     Called by Unity to initialize the <see cref="GeneratorManager"/> class.
        /// </summary>
        private void Awake()
        {
            this.NewPreferOld(true);

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
            this.InvokeTerrainGenerator();
            this.InvokeHouseGenerators();
        }

        /// <summary>
        ///     Invokes the terrain generator.
        /// </summary>
        private void InvokeTerrainGenerator()
        {
            if (TerrainGenerator.Instance == null)
            {
                Debug.LogWarning("There is no TerrainGenerator.");
                return;
            }

            TerrainGenerator.Instance.Invoke();
        }

        /// <summary>
        ///     Invokes all house generators.
        /// </summary>
        private void InvokeHouseGenerators()
        {
            HouseGenerator[] houseGenerators = this.GetComponentsInChildren<HouseGenerator>();

            foreach (HouseGenerator houseGenerator in houseGenerators)
            {
                houseGenerator.Invoke();
            }
        }
    }
}
