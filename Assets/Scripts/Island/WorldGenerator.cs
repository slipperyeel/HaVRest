using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class WorldGenerator : MonoBehaviour
{
	public static WorldGenerator Instance;

    public float DesertThreshold = 0.2f;
    public float SavannahThreshold = 0.4f;
    public float PlainsThreshold = 0.6f;
    public float ForestThreshold = 0.8f;
    public float JungleThreshold = 1f; // not used

	public enum IslandSize { Small = 128, Medium = 256, Large = 512 };

	[SerializeField]
	private GameObject _islandGenPrefab;

	public int WorldSeed = -1;
	public int NumberOfIslands;
	public int MaxWorldSize;
	public int MinNumberOfHeightLevels;
	public float SmIslandChance;
	public float MdIslandChance;
	public float LgIslandChance;

	private List<IslandData> _islands;

	private IslandData _currentIsland;
    public IslandData CurrentIsland { get { return _currentIsland; } }

	void Awake()
	{
		Instance = this;
	}

	void Start()
	{
		_islands = new List<IslandData>(NumberOfIslands);

		// TODO
		// will need to load the save file and get the seed
		// if there is no save, create a new seed
		if (WorldSeed == -1)
		{
			WorldSeed = UnityEngine.Random.Range(0, int.MaxValue);
		}

		GenerateWorld(WorldSeed);

		_currentIsland = _islands[0];
	}

	private void GenerateWorld(int worldSeed)
	{
		GameObject islGen = (GameObject)Instantiate(_islandGenPrefab, Vector3.zero, Quaternion.identity);
		IslandGenerator generator = islGen.GetComponent<IslandGenerator>();

		System.Random worldPRNG = new System.Random(worldSeed);

        // biome/climate regions
        PerlinGenerator climateNoiseGen = new PerlinGenerator();
        int octaves = 9; // higher octaves (10+) create large biomes
        float[][] biomeNoise = climateNoiseGen.Generate(worldSeed, octaves, MaxWorldSize);

        for (int i = 0; i < NumberOfIslands; ++i)
		{
			MakeNewIsland(generator, worldPRNG.Next(), biomeNoise);
		}
	}

	private void MakeNewIsland(IslandGenerator gen, int islandSeed, float[][] biomeNoise)
	{
		System.Random islandPRNG = new System.Random(islandSeed);

		IslandSize size = DetermineIslandSize(islandPRNG.Next());
        int islandSize = (int)size;
        int x = islandPRNG.Next(islandSize, MaxWorldSize - islandSize);
		int z = islandPRNG.Next(islandSize, MaxWorldSize - islandSize);
		Vector3 position = new Vector3(x, 0f, z);
		int numOfHeightLevels = islandPRNG.Next(MinNumberOfHeightLevels) + MinNumberOfHeightLevels; // dont want island with < min height levels

        // should check to make sure islands can't overlap each other

        // copy out the relevant climate noise
        float[][] islandSpecificBiomeData = new float[islandSize][];
        //Debug.LogFormat("specific: {0}x{1}", islandSpecificBiomeData.Length, islandSpecificBiomeData[0].Length);
        //Debug.LogFormat("base: {0}x{1}", biomeNoise.Length, biomeNoise[0].Length);

        for (int i = 0; i < islandSize; ++i)
        {
            islandSpecificBiomeData[i] = new float[islandSize];

            for (int j = 0; j < islandSize; ++j)
            {
                //Debug.LogFormat("specific -> i: {0}, j:{1}, base -> i:{2} j:{3}",i, j, i + x, j + z);
                islandSpecificBiomeData[i][j] = biomeNoise[i + x][j + z];
            }
        }


		Debug.LogFormat("new island at {0} of size {1} with {2} levels ({3})", position, size.ToString(), numOfHeightLevels, islandSeed);

		// TODO
		// create an island data class that will be returned from this function
		// should hold things like:
		// tile array, native species of plants/vegetables/etc, general climate, size, soil classification, ...

		IslandData data = gen.Generate(size, islandSeed, position, islandSpecificBiomeData, numOfHeightLevels);
		_islands.Add(data);
	}

	private IslandSize DetermineIslandSize(int random)
	{
		float threshold = random / (float)int.MaxValue;

		// favor smaller islands, making larger ones rare
		if (threshold < SmIslandChance)
		{
			return IslandSize.Small;
		}
		else if (threshold < SmIslandChance + MdIslandChance)
		{
			return IslandSize.Medium;
		}
		else
		{
			return IslandSize.Large;
		}
	}

	public void SetCurrentIsland(int id)
	{
		_currentIsland = _islands[id];
		Debug.Log("Current island is now " + id);
	}

	public void TestDropSphere(int x, int y)
	{
		GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		_currentIsland.AddObjectToTile(obj, x, y);
	}

	public void RaiseTerrain(int x, int y, float amount)
	{
		_currentIsland.ModifyTerrainAtTilePosition(x, y, amount);
	}
}
