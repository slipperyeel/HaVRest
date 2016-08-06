using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
	[SerializeField]
	private Vector2 _coordinates;
	public Vector2 Coordinates { get { return _coordinates; } }

	[SerializeField]
	private Vector3 _terrainCoordinates;
	public Vector3 TerrainPosition { get { return _terrainCoordinates; } }

	[SerializeField]
	private GameObject _occupyingObject = null;
	public GameObject OccupyingObject { get { return _occupyingObject; } }

	// TODO need an item interface to see what item/object is occupying this tile

    public void Init(int x, int y, float height, int z, int w)
    {
        _coordinates = new Vector2(x, y);
        _terrainCoordinates = new Vector3(z, height * 10f, w); // 10f is the terrain height scale factor
    }

	public void SetOccupyingObject(GameObject obj)
	{
		_occupyingObject = obj;
	}
}
