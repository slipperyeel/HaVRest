using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
	[SerializeField]
	private Vector2 _islandPos;
	public Vector2 IslandPosition { get { return _islandPos; } }

	[SerializeField]
	private Vector3 _worldPos;
	public Vector3 WorldPosition { get { return _worldPos; } }

	[SerializeField]
	private eMoisture _moisture;
	public eMoisture Moisture { get { return _moisture; } }

	[SerializeField]
	private eTemperature _temp;
	public eTemperature Temperature { get { return _temp; } }

    [SerializeField]
	private eBiome _biome;
	public eBiome Biome { get { return _biome; } }

	[SerializeField]
	private GameObject _occupyingObject = null;
	public GameObject OccupyingObject { get { return _occupyingObject; } }

    [SerializeField]
    private int _durability;
    public int Durability { get { return _durability; } }

	// TODO need an item interface to see what item/object is occupying this tile

    public void Init(int x, int y, float height, int z, int w)
    {
        _islandPos = new Vector2(x, y);
        _worldPos = new Vector3(z, height * 10f, w); // 10f is the terrain height scale factor
    }

	public void SetBiomeFields(eMoisture moist, eTemperature temp, eBiome biome)
    {
		_moisture = moist;
		_temp = temp;
		_biome = biome;

		// for now, durability will just be a straight addition of moisture/temp
		// drier/colder means harder, moist/warm means softer
		int count = System.Enum.GetNames(typeof(eMoisture)).Length;
		_durability = (count - (int)moist) + (int)temp;
    }

	public void SetOccupyingObject(GameObject obj)
	{
		_occupyingObject = obj;
	}
}
