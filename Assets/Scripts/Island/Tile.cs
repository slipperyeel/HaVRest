using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
    public enum eSoilType { Dirt, Grass, Sand, Rock };
    public enum eSoilToughness { Soft, Medium, Hard };

	[SerializeField]
	private Vector2 _coordinates;
	public Vector2 Coordinates { get { return _coordinates; } }

	[SerializeField]
	private Vector3 _terrainCoordinates;
	public Vector3 TerrainPosition { get { return _terrainCoordinates; } }

    [SerializeField]
    private eSoilType _type;
    public eSoilType Type { get { return _type; } }

    [SerializeField]
    private eSoilToughness _toughness;
    public eSoilToughness Toughness { get { return _toughness; } }

	[SerializeField]
	private GameObject _occupyingObject = null;
	public GameObject OccupyingObject { get { return _occupyingObject; } }

    [SerializeField]
    private int _durability;
    public int Durability { get { return _durability; } }

	// TODO need an item interface to see what item/object is occupying this tile

    public void Init(int x, int y, float height, int z, int w)
    {
        _coordinates = new Vector2(x, y);
        _terrainCoordinates = new Vector3(z, height * 10f, w); // 10f is the terrain height scale factor
    }

    public void SetTileType(eSoilType type, eSoilToughness toughness)
    {
        _type = type;
        _toughness = toughness;

        int baseDurability = 0;

        switch(type)
        {
            case eSoilType.Dirt:
                baseDurability = 5;
                break;

            case eSoilType.Grass:
                baseDurability = 5;
                break;

            case eSoilType.Sand:
                baseDurability = 3;
                break;

            case eSoilType.Rock:
                baseDurability = 8;
                break;
        }

        switch (toughness)
        {
            case eSoilToughness.Soft:
                baseDurability -= 1;
                break;

            case eSoilToughness.Medium:
                baseDurability += 1;
                break;

            case eSoilToughness.Hard:
                baseDurability += 3;
                break;
        }

        _durability = baseDurability;
    }

	public void SetOccupyingObject(GameObject obj)
	{
		_occupyingObject = obj;
	}
}
