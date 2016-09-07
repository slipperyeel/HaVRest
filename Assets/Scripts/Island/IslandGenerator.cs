using UnityEngine;
using System.Collections;
using UnityEditor;

public class IslandGenerator : MonoBehaviour
{
    //public enum DetailType { Tree_Pine_0, Tree_Pine_1, Tree_Pine_2, Rock_Medium, Rock_Large, Flower_Bud, Bush_Small, Grass, Fern, Rock_Small };
    public enum DetailType { Tree_Pine_0, Tree_Pine_1, Tree_Pine_2, Rock_Medium, Flower_Bud, Grass, Fern };

    public GameObject[] _detailObjects;

    public static int IslandID = 0;
    private int _id;
    private GameObject _currentTerrain;
    private SplatPrototype[] _terrainSplats;
	private eBiome[,] _biomeTable = new eBiome[,]
	{
		//     dry             mild           normal          damp           wet
		{ eBiome.Desert, eBiome.Savannah, eBiome.Plains, eBiome.Swamp, eBiome.Jungle },  // hot
		{ eBiome.Desert, eBiome.Savannah, eBiome.Plains, eBiome.Swamp, eBiome.Swamp },    // warm
		{ eBiome.Savannah, eBiome.Plains, eBiome.Plains, eBiome.Forest, eBiome.Forest },  // normal
		{ eBiome.Plains, eBiome.Plains, eBiome.Plains, eBiome.Forest, eBiome.Forest   },  // cool
		{ eBiome.Tundra, eBiome.Tundra, eBiome.Plains, eBiome.Plains, eBiome.Plains   }  // cold
	};

	public IslandData Generate(WorldGenerator.IslandSize size, int seed, Vector3 position, float[][] moistureNoise, float[][] tempNoise, int numHeightLevels, bool fromTool = false)
    {
        if (_terrainSplats == null)
        {
            _terrainSplats = LoadPrototypes();
        }

        _id = IslandID++;
        int realSize = (int)size;
        PerlinGenerator islandNoiseGen = new PerlinGenerator();
        int octaves = 5;
        float[][] noise = islandNoiseGen.Generate(seed, octaves, realSize);
        float[,] heights = CreateHeightMap(noise, realSize, numHeightLevels);

        Vector3 posWithOffset = new Vector3(position.x - (realSize / 2f), position.y, position.z - (realSize / 2f));

        TerrainData tData = ApplyDataToTerrain(ref _currentTerrain, heights, realSize, numHeightLevels, fromTool);
        _currentTerrain.transform.position = posWithOffset;

        IslandData data = new IslandData(_id, realSize, posWithOffset, ref tData);
        PopulateTiles(ref data, posWithOffset, moistureNoise, tempNoise, heights, realSize, ref tData);
        //DebugDrawGrid(posWithOffset, realSize, heights);

        PlaceDetails(heights, realSize, ref data, seed);

        return data;
    }

    public GameObject Generate(WorldGenerator.IslandSize size, int seed, int numHeightLevels)
    {
		//fake climate data
		PerlinGenerator moistureNoiseGen = new PerlinGenerator();
		PerlinGenerator tempNoiseGen = new PerlinGenerator();

		int octaves = 9; // higher octaves (10+) create large biomes
		float[][] moistureNoise = moistureNoiseGen.Generate(seed, octaves, (int)size);
		float[][] temperatureNoise = tempNoiseGen.Generate(seed, octaves, (int)size);

		// generate island
		Generate(size, seed, Vector3.zero, moistureNoise, temperatureNoise, numHeightLevels, true);
		return _currentTerrain;
    }

    private void DebugDrawGrid(Vector3 islandPos, int size, float[,] heights)
    {
        for (int i = 0; i < size; ++i)
        {
            for (int j = 0; j < size; ++j)
            {
                float height = heights[j, i] * 10f; // 10 is the terrain height scale
                Debug.DrawLine(new Vector3(islandPos.x + i, height, islandPos.z + j), new Vector3(islandPos.x + i, height, islandPos.z + j + 1), Color.black, 300f);
                Debug.DrawLine(new Vector3(islandPos.x + i, height, islandPos.z + j), new Vector3(islandPos.x + i + 1, height, islandPos.z + j), Color.black, 300f);
            }
        }
    }

    private TerrainData ApplyDataToTerrain(ref GameObject terrain, float[,] heights, int size, int numHeights, bool fromTool)
    {
        TerrainData data = new TerrainData();

        data.heightmapResolution = size;
        data.alphamapResolution = size;
        data.size = new Vector3(size, 10f, size);
        data.SetHeights(0, 0, heights);

        int islandID = UnityEngine.Random.Range(0, int.MaxValue);
        if (fromTool)
        {
            // have to save terrain data out as an asset for some reason
            AssetDatabase.CreateAsset(data, "Assets/Resources/Custom Islands/Data_" + islandID + ".asset");
        }

        data.splatPrototypes = _terrainSplats;
        //ApplyTextures(heights, ref data, size, numHeights);

        terrain = Terrain.CreateTerrainGameObject(data);
        terrain.name = string.Format("island_{0}", islandID);

        return data;
    }

    private SplatPrototype[] LoadPrototypes()
    {
        SplatPrototype[] newProtos = new SplatPrototype[23];
        Texture2D[] splatTextures = new Texture2D[]
        {
            (Texture2D)WorldGenerator.Instance.DesertTextures[(int)eTileType.Dirt],
            (Texture2D)WorldGenerator.Instance.DesertTextures[(int)eTileType.Grass],
            (Texture2D)WorldGenerator.Instance.DesertTextures[(int)eTileType.Stone],
            (Texture2D)WorldGenerator.Instance.DesertTextures[(int)eTileType.Sand],

            (Texture2D)WorldGenerator.Instance.SavannahTextures[(int)eTileType.Dirt],
            (Texture2D)WorldGenerator.Instance.SavannahTextures[(int)eTileType.Grass],
            (Texture2D)WorldGenerator.Instance.SavannahTextures[(int)eTileType.Stone],

            (Texture2D)WorldGenerator.Instance.PlainsTextures[(int)eTileType.Dirt],
            (Texture2D)WorldGenerator.Instance.PlainsTextures[(int)eTileType.Grass],
            (Texture2D)WorldGenerator.Instance.PlainsTextures[(int)eTileType.Stone],

            (Texture2D)WorldGenerator.Instance.ForestTextures[(int)eTileType.Dirt],
            (Texture2D)WorldGenerator.Instance.ForestTextures[(int)eTileType.Grass],
            (Texture2D)WorldGenerator.Instance.ForestTextures[(int)eTileType.Stone],

            (Texture2D)WorldGenerator.Instance.SwampTextures[(int)eTileType.Dirt],
            (Texture2D)WorldGenerator.Instance.SwampTextures[(int)eTileType.Grass],
            (Texture2D)WorldGenerator.Instance.SwampTextures[(int)eTileType.Stone],

            (Texture2D)WorldGenerator.Instance.JungleTextures[(int)eTileType.Dirt],
            (Texture2D)WorldGenerator.Instance.JungleTextures[(int)eTileType.Grass],
            (Texture2D)WorldGenerator.Instance.JungleTextures[(int)eTileType.Stone],

            (Texture2D)WorldGenerator.Instance.TundraTextures[(int)eTileType.Dirt],
            (Texture2D)WorldGenerator.Instance.TundraTextures[(int)eTileType.Grass],
            (Texture2D)WorldGenerator.Instance.TundraTextures[(int)eTileType.Stone],
            (Texture2D)WorldGenerator.Instance.TundraTextures[(int)eTileType.Snow]
        };

        for (int i = 0; i < 23; ++i)
        {
            SplatPrototype sp = new SplatPrototype();
            sp.texture = splatTextures[i];
            newProtos[i] = sp;
        }

        return newProtos;
    }

    private void ApplyTextures(float[,] heights, ref TerrainData data, int size, int numHeights)
    {
        float[,,] splatmapData = new float[size, size, _terrainSplats.Length];
        float levelIncrement = 1 / (float)numHeights;

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float height = heights[j, i];

                // the last index for splatmapData is the number of the texture on the terrain prefab
                // the value that that index is being set to seems to be regular alpha
                // set it based on terrain height

                // also need a better way to do this
                if (height <= levelIncrement * 0f)
                {
                    splatmapData[j, i, 0] = 1; // dirt
                }
                else if (height <= levelIncrement * 1f)
                {
                    splatmapData[j, i, 2] = 1; // dark grass
                }
                else if (height <= levelIncrement * 2f)
                {
                    splatmapData[j, i, 1] = 1; // normal grass
                }
                else if (height <= levelIncrement * 3f)
                {
                    splatmapData[j, i, 3] = 1; // light grass
                }
                else
                {
                    splatmapData[j, i, 4] = 1; // dead grass
                }

            }
        }

        // apply the new alpha
        data.SetAlphamaps(0, 0, splatmapData);
    }

    private float[,] CreateHeightMap(float[][] noise, int size, int heightLevels)
    {
        float[,] heights = new float[size, size];

        for (int i = 0; i < size; ++i)
        {
            for (int j = 0; j < size; ++j)
            {
                float noiseValue = noise[i][j] + 0.25f; // magic height offset?
                float maskValue = GetMaskValue(i, j, noiseValue, size);
                float mixedValue = noiseValue - maskValue;

                heights[i, j] = RoundValue(mixedValue, heightLevels);
            }
        }

        return heights;
    }

    private void GetWorldTerrainPosition(Vector3 terrainPos, int x, int y, out int tX, out int tY)
    {
        tX = (int)(terrainPos.x + x);
        tY = (int)(terrainPos.z + y);
    }

    private void PopulateTiles(ref IslandData data, Vector3 terrainPos, float[][] moisture, float[][] temps, float[,] heights, int size, ref TerrainData tData)
    {
        GameObject container = new GameObject();
        container.name = "Tile Colliders";
        container.transform.position = Vector3.zero;
        GameObject tileObject = null;
        Tile t = null;

        int[] offsets = new int[] { 0, 4, 7, 10, 13, 16, 19 };
        float[,,] splatData = new float[size, size, _terrainSplats.Length];

        for (int i = 0; i < size; ++i)
        {
            for (int j = 0; j < size; ++j)
            {
                float height = heights[j, i];

                // don't put tiles on the bottom most layer
                if (height > 0f)
                {
                    int terrainX, terrainY;
                    GetWorldTerrainPosition(terrainPos, i, j, out terrainX, out terrainY);

                    // create physical tile
                    tileObject = CreateTileObject();
                    tileObject.name = string.Format("Tile[{0},{1}]", i, j);
                    t = tileObject.AddComponent<Tile>();
                    t.Init(i, j, height, terrainX, terrainY);

                    // determine biome
					eMoisture moist = GetMoistureEnumFromValue(moisture[i][j]);
					eTemperature temp = GetTempEnumFromValue(temps[i][j]);
                    eBiome biome = _biomeTable[(int)moist, (int)temp];

					t.SetBiomeFields(moist, temp, biome);

                    // set terrain texture
                    int textureIndex = offsets[(int)biome] + (int)eTileType.Grass;
                    splatData[j, i, textureIndex] = 1;

                    tileObject.transform.position = t.WorldPosition + new Vector3(0.5f, 0.05f, 0.5f);

                    // add to parent container for a neat hierarchy
                    tileObject.transform.SetParent(container.transform);
                }
                else
                {
                    // just dirt if height is zero
                    int textureIndex = offsets[(int)eBiome.Jungle] + (int)eTileType.Dirt;
                    splatData[j, i, textureIndex] = 1;
                }

                // need to fill the island data with values, even if no tile is actually created
                data.AddTile(t, i, j);
            }
        }

        tData.SetAlphamaps(0, 0, splatData);
    }

	private eMoisture GetMoistureEnumFromValue(float value)
	{
		eMoisture m = eMoisture.Wet;
		if (value < 0.2f)
		{
			m = eMoisture.Dry;
		}
		else if (value < 0.4f)
		{
			m = eMoisture.Mild;
		}
		else if (value < 0.6f)
		{
			m = eMoisture.Normal;
		}
		else if (value < 0.8f)
		{
			m = eMoisture.Damp;
		}

		return m;
	}

	private eTemperature GetTempEnumFromValue(float value)
	{
		eTemperature t = eTemperature.Cold;
		if (value < 0.2f)
		{
			t = eTemperature.Hot;
		}
		else if (value < 0.4f)
		{
			t = eTemperature.Warm;
		}
		else if (value < 0.6f)
		{
			t = eTemperature.Normal;
		}
		else if (value < 0.8f)
		{
			t = eTemperature.Cool;
		}

		return t;
	}

    private GameObject CreateTileObject()
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.transform.localScale = new Vector3(1f, 0.1f, 1f);
        Destroy(obj.GetComponent<MeshRenderer>());
        Destroy(obj.GetComponent<MeshFilter>());

        return obj;
    }

    private float RoundValue(float value, int numHeights)
    {
        float levelIncrement = 1 / (float)numHeights;
        for (int i = 0; i < numHeights; ++i)
        {
            if (value <= (i * levelIncrement))
            {
                return i * levelIncrement;
            }
        }

        return numHeights * levelIncrement;
    }

    private float GetMaskValue(int x, int y, float noiseValue, int size)
    {
        int radius = size / 2;
        float dist = GetDistToCenter(x, y, radius - 1);
        return dist / radius;
    }

    private float GetDistToCenter(int x, int y, int center)
    {
        int deltaX = center - x;
        int deltaY = center - y;
        return Mathf.Sqrt((deltaX * deltaX) + (deltaY * deltaY));
    }

    private void PlaceDetails(float[,] heights, int size, ref IslandData data, int seed)
    {
        System.Random prng = new System.Random(seed);
        int noiseSeed = prng.Next();
        GameObject detailContainer = new GameObject();
        detailContainer.name = "Details Container";
        Tile t;
        eBiome currentBiome = eBiome.Desert;

        PerlinGenerator detailNoiseGen = new PerlinGenerator();
        float[][] noise = detailNoiseGen.Generate(noiseSeed, 3, size);
        
        int detailID = -1;
        for (int i = 0; i < size; ++i)
        {
            for (int j = 0; j < size; ++j)
            {
                detailID = -1;
                float height = heights[j, i];
                if (height > 0f)
                {
                    float value = noise[i][j];
                    currentBiome = data.GetTile(i, j).Biome;

                    if (value > 0.85f)
                    {
                        detailID = (int)eDetailType.Tree;

                        if (value > 0.95f)
                        {
                            // TODO set scale modifier here or something
                            //detail = _detailObjects[(int)DetailType.Tree_Pine_0];
                        }
                        else if (value > 0.9f)
                        {
                            //detail = _detailObjects[(int)DetailType.Tree_Pine_1];
                        }
                        else
                        {
                            //detail = _detailObjects[(int)DetailType.Tree_Pine_2];
                        }
                    }
                    else if (value > 0.75f)
                    {
                        detailID = (int)eDetailType.Flower;
                        //detail = _detailObjects[(int)DetailType.Flower_Bud];
                    }
                    else if (value > 0.68f)
                    {
                        detailID = (int)eDetailType.Grass;
                        //detail = _detailObjects[(int)DetailType.Grass];
                    }
                    else if (value > 0.65f)
                    {
                        detailID = (int)eDetailType.Fern;
                        //detail = _detailObjects[(int)DetailType.Fern];
                    }
                    else if (value < 0.025f)
                    {
                        detailID = (int)eDetailType.Rock;
                        //detail = _detailObjects[(int)DetailType.Rock_Medium];
                    }

                    //if (detail != null)
                    if (detailID > -1)
                    {
                        //GameObject detail = null;
                        GameObject detail = GetDetailsForBiome(currentBiome)[detailID];
                        GameObject detailObj = (GameObject)Instantiate(detail, Vector3.zero, Quaternion.identity);
                        float angle = prng.Next(359) + 1;
                        detailObj.transform.Rotate(Vector3.up, angle);
                        detailObj.transform.SetParent(detailContainer.transform);
                        data.AddObjectToTile(detailObj, i, j);
                    }
                }
            }
        }
    }

    private GameObject[] GetDetailsForBiome(eBiome biome)
    {
        if (biome == eBiome.Desert)
        {
            return WorldGenerator.Instance.DesertDetails;
        }
        else if (biome == eBiome.Savannah)
        {
            return WorldGenerator.Instance.SavannahDetails;
        }
        else if (biome == eBiome.Plains)
        {
            return WorldGenerator.Instance.PlainsDetails;
        }
        else if (biome == eBiome.Forest)
        {
            return WorldGenerator.Instance.ForestDetails;
        }
        else if (biome == eBiome.Swamp)
        {
            return WorldGenerator.Instance.SwampDetails;
        }
        else if (biome == eBiome.Jungle)
        {
            return WorldGenerator.Instance.JungleDetails;
        }
        else
        {
            return WorldGenerator.Instance.TundraDetails;
        }
    }

    private Vector3 _pos;
    private float[,,] _splatmapData;

    // returns an array containing the relative mix of textures
    // on the main terrain at this world position.
    // The number of values in the array will equal the number
    // of textures added to the terrain.
    private float[] GetTextureMix(Vector3 worldPos, TerrainData _data)
    {
        // calculate which splat map cell the worldPos falls within (ignoring y)
        int mapX = (int)(((worldPos.x - _pos.x) / _data.size.x) * _data.alphamapWidth);
        int mapZ = (int)(((worldPos.z - _pos.z) / _data.size.z) * _data.alphamapHeight);

        // get the splat data for this cell as a 1x1xN 3d array (where N = number of textures)
        _splatmapData = _data.GetAlphamaps(mapX, mapZ, 1, 1);

        // extract the 3D array data to a 1D array:
        float[] cellMix = new float[_splatmapData.GetUpperBound(2) + 1];
        for (int n = 0; n < cellMix.Length; ++n)
        {
            cellMix[n] = _splatmapData[0, 0, n];
        }

        return cellMix;
    }

    // returns the zero-based index of the most dominant texture
    // on the main terrain at this world position.
    private int GetMainTextureIndex(Vector3 worldPos, TerrainData data)
    {
        float[] mix = GetTextureMix(worldPos, data);
        float maxMix = 0;
        int maxIndex = 0;

        // loop through each mix value and find the maximum
        for (int n = 0; n < mix.Length; ++n)
        {
            if (mix[n] > maxMix)
            {
                maxIndex = n;
                maxMix = mix[n];
            }
        }

        return maxIndex;
    }

    // this may come in handy later!
    //	private void RadialPlace(int radius, int x, int z, int size, ref IslandData data)
    //	{
    //		int numberOfPoints = radius * 2;
    //		for (int pointNum = 0; pointNum < numberOfPoints; ++pointNum)
    //		{
    //			float i = pointNum / (float)numberOfPoints;
    //			float angle = i * Mathf.PI * 2f;
    //			float pointX = Mathf.Sin(angle) * radius;
    //			float pointZ = Mathf.Cos(angle) * radius;
    //			pointX = Mathf.RoundToInt(pointX + x);
    //			pointZ = Mathf.RoundToInt(pointZ + z);
    //
    //			if (pointX < size && pointZ < size)
    //			{
    //				GameObject detail = (GameObject)Instantiate(_detailObjects[(int)DetailType.Tree_Pine_0], Vector3.zero, Quaternion.identity);
    //				data.AddObjectToTile(detail, (int)pointX, (int)pointZ);
    //			}
    //		}
    //	}
}
