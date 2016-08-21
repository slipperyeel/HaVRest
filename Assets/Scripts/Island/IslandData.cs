using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class IslandData
{
	[SerializeField]
	private int _id;
	public int ID { get { return _id; } }

	[SerializeField]
	private Tile[,] _tiles;

	private TerrainData _data;
	public TerrainData Data { get { return _data; } }

	private int _size;

	private Vector3 _worldPosition;
	public Vector3 WorldPosition { get { return _worldPosition; } }

	// TODO the item that is occupying this space, once we figure that out (IItem interface

	public IslandData(int id, int size, Vector3 worldPos, ref TerrainData data)
	{
		_id = id;
		_size = size;
		_worldPosition = worldPos;
		_tiles = new Tile[size,size];
		_data = data;
	}

	public void AddTile(Tile t, int x, int y)
	{
		_tiles[x,y] = t;
	}

	public void AddObjectToTile(GameObject obj, int x, int y)
	{
		Tile t = _tiles[x,y];
		if (t.OccupyingObject == null)
		{
			Vector3 terrainPos = t.WorldPosition;
            terrainPos.x += 0.5f;
            terrainPos.z += 0.5f;
			obj.transform.position = terrainPos;

			t.SetOccupyingObject(obj);
		}
	}

	public void ModifyTerrainAtTilePosition(int x, int y, float amount)
	{
		SetHeights(x, y, amount);
	}

	// UNTESTED
	public void ModifyTerrainAtWorldPosition(int x, int y, float amount)
	{
		Vector3 objPos = new Vector3(x, 0f, y);
		Vector3 tempCoord = (objPos - _worldPosition);
		Vector3 coord;
		coord.x = tempCoord.x / _size;
		coord.z = tempCoord.z / _size;

		int terrainX = (int) (coord.x * _size); 
		int terrainY = (int) (coord.z * _size);

		SetHeights(terrainX, terrainY, amount);
	}

    public Tile GetTile(int x, int y)
    {
        return _tiles[x, y];
    }

	private void SetHeights(int x, int y, float amount)
	{
		float[,] currentHeights = _data.GetHeights(y, x, 1, 1);
		currentHeights[0,0] += amount;
		_data.SetHeights(y, x, currentHeights);
	}
}
