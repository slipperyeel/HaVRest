using UnityEngine;
using System.Collections;

public class IslandGenerator : MonoBehaviour
{
	public enum DetailType { Tree_Pine_0, Tree_Pine_1, Tree_Pine_2, Rock_Medium, Rock_Large, Flower_Bud, Bush_Small, Grass, Fern, Rock_Small };

	[SerializeField]
	private GameObject _terrainPrefab;

	[SerializeField]
	private GameObject[] _detailObjects;

	public static int IslandID = 0;
	private int _id;
	private GameObject _currentTerrain;

	// TODO return island data here (see calling class for more details)
	public IslandData Generate(WorldGenerator.IslandSize size, int seed, Vector3 position, int numHeightLevels)
	{
		_id = IslandID++;
		int realSize = (int)size;
		PerlinGenerator islandNoiseGen = new PerlinGenerator();
		int octaves = 5;
		float[][] noise = islandNoiseGen.Generate(seed, octaves, realSize);
		float[,] heights = CreateHeightMap(noise, realSize, numHeightLevels);

		Vector3 posWithOffset = new Vector3(position.x - (realSize / 2f), position.y, position.z - (realSize / 2f));
		_currentTerrain = (GameObject)Instantiate(_terrainPrefab, posWithOffset, Quaternion.identity);
		_currentTerrain.name = string.Format("Island {0}", _id);
		TerrainData tData = ApplyDataToTerrain(ref _currentTerrain, heights, realSize, numHeightLevels);

		IslandData data = new IslandData(_id, realSize, posWithOffset, ref tData);
		PopulateTiles(ref data, posWithOffset, heights, realSize);
		//DebugDrawGrid(posWithOffset, realSize, heights);

		PlaceDetails(heights, realSize, ref data, seed);

		return data;
	}

	private void DebugDrawGrid(Vector3 islandPos, int size, float[,] heights)
	{
		for (int i = 0; i < size; ++i)
		{
			for (int j = 0; j < size; ++j)
			{
				float height = heights[j,i] * 10f; // 10 is the terrain height scale
				Debug.DrawLine(new Vector3(islandPos.x + i, height, islandPos.z + j), new Vector3(islandPos.x + i, height, islandPos.z + j + 1), Color.black, 10f);
				Debug.DrawLine(new Vector3(islandPos.x + i, height, islandPos.z + j), new Vector3(islandPos.x + i + 1, height, islandPos.z + j), Color.black, 10f);
			}
		}
	}

	private TerrainData ApplyDataToTerrain(ref GameObject terrain, float[,] heights, int size, int numHeights)
	{
		Terrain t = terrain.GetComponent<Terrain>();
		TerrainCollider c = terrain.GetComponent<TerrainCollider>();
		TerrainData prefabData = t.terrainData;
		TerrainData data = new TerrainData();//t.terrainData;

		data.heightmapResolution = size;
		data.alphamapResolution = size;
		data.size = new Vector3(size, 10f, size);
		data.SetHeights(0, 0, heights);

		data.splatPrototypes = CopyPrototypes(prefabData.splatPrototypes);
		ApplyTextures(heights, ref data, size, numHeights);

		t.terrainData = data;
		c.terrainData = data;

		return data;
	}

	private SplatPrototype[] CopyPrototypes(SplatPrototype[] old)
	{
		SplatPrototype[] newProtos = new SplatPrototype[old.Length];
		for(int i = 0; i < old.Length; ++i)
		{
			SplatPrototype oldProto = old[i];
			SplatPrototype newProto = new SplatPrototype();
			newProtos[i] = newProto;

			newProto.texture = oldProto.texture;
			newProto.tileSize = oldProto.tileSize;
			newProto.tileOffset = oldProto.tileOffset;
		}

		return newProtos;
	}

	private void ApplyTextures(float[,] heights, ref TerrainData data, int size, int numHeights)
	{
		float[,,] splatmapData = new float[size, size, data.alphamapLayers];
		float levelIncrement = 1 / (float)numHeights;

		for (int i = 0; i < size; i++)
		{
			for (int j = 0; j < size; j++)
			{
				float height = heights[j,i];

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
		float[,] heights = new float[size,size];

		for (int i = 0; i < size; ++i)
		{
			for (int j = 0; j < size; ++j)
			{
				float noiseValue = noise[i][j] + 0.25f; // magic height offset?
				float maskValue = GetMaskValue(i, j, noiseValue, size);
				float mixedValue = noiseValue - maskValue;

				heights[i,j] = RoundValue(mixedValue, heightLevels);
			}
		}

		return heights;
	}

	private void GetWorldTerrainPosition(Vector3 terrainPos, int x, int y, out int tX, out int tY)
	{
		tX = (int)(terrainPos.x + x);
		tY = (int)(terrainPos.z + y);
	}

	private void PopulateTiles(ref IslandData data, Vector3 terrainPos, float[,] heights, int size)
	{
		for (int i = 0; i < size; ++i)
		{
			for (int j = 0; j < size; ++j)
			{
				float height = heights[j,i];
				int terrainX, terrainY;
				GetWorldTerrainPosition(terrainPos, i, j, out terrainX, out terrainY);

				Tile t = new Tile(i, j, height, terrainX, terrainY);
				data.AddTile(t, i, j);
			}
		}
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

	private void CreateScatter(float x, float y, float w, float h, int rows, int cols)
	{
		float xd = w/cols;
		float yd = h/rows;
		for (int ix = 0;ix <(cols+1);ix++) {
			for (int iy = 0;iy<(rows+1);iy++) {
				float p = 0.4f;
				//CreatePoint( Vector2(x+xd*ix+rndf(-p,p)*xd,y+yd*iy+rndf(-p,p)*yd));
			}
		}
	}

	private void PlaceDetails(float[,] heights, int size, ref IslandData data, int seed)
	{
		System.Random prng = new System.Random(seed);
		int noiseSeed = prng.Next();

		PerlinGenerator detailNoiseGen = new PerlinGenerator();
		float[][] noise = detailNoiseGen.Generate(noiseSeed, 3, size);
		GameObject detail = null;
		for (int i = 0; i < size; ++i)
		{
			for (int j = 0; j < size; ++j)
			{
				float height = heights[j, i];
				if (height > 0f)
				{
					float value = noise[i][j];
					if (value > 0.85f)
					{
						detail = _detailObjects[(int)DetailType.Tree_Pine_0];
					}
					else if (value > 0.75f)
					{
						detail = _detailObjects[(int)DetailType.Flower_Bud];
					}
					else if (value > 0.7f)
					{
						detail = _detailObjects[(int)DetailType.Grass];
					}
					else if (value > 0.685f)
					{
						detail = _detailObjects[(int)DetailType.Bush_Small];
					}
					else if (value < 0.025f)
					{
						detail = _detailObjects[(int)DetailType.Rock_Small];
					}
					else
					{
						detail = null;
					}

					if (detail != null)
					{
						GameObject detailObj = (GameObject)Instantiate(detail, Vector3.zero, Quaternion.identity);
						float angle = prng.Next(359) + 1;
						detailObj.transform.Rotate(Vector3.up, angle);
						detailObj.transform.SetParent(_currentTerrain.transform);
						data.AddObjectToTile(detailObj, i, j);
					}
				}
			}
		}
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
