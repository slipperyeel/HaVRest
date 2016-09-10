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
	private eTileState _state;
	public eTileState State { get { return _state; } }

    [SerializeField]
    private int _durability;
    public int Durability { get { return _durability; } }

	public System.Action<Tile, eTileState> StateChangeCallback;

	// TODO need an item interface to see what item/object is occupying this tile

    public void Init(int x, int y, float height, int z, int w)
    {
        _islandPos = new Vector2(x, y);
        _worldPos = new Vector3(z, height * 10f, w); // 10f is the terrain height scale
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

	public void SetState(eTileState state)
	{
		_state = state;

		if (StateChangeCallback != null)
		{
			StateChangeCallback(this, _state);
		}
	}

	void OnCollisionEnter(Collision other)
	{
		PhysicalItem item = other.gameObject.GetComponent<PhysicalItem>();
		if (item == null) return;

		eImpactType impact = item.ImpactType;
		switch (_state)
		{
		case eTileState.Invalid:
			// do nothing
			break;

		case eTileState.Normal:
			if (impact == eImpactType.Pierce)
			{
				SetState(eTileState.Dug);
			}
			else if (impact == eImpactType.Sharp)
			{
				SetState(eTileState.Tilled);
			}
			break;

		case eTileState.Dug:
		case eTileState.Tilled:
			if (impact == eImpactType.Blunt)
			{
				SetState(eTileState.Normal);
				break;
			}

			ResourceObject obj = other.gameObject.GetComponent<ResourceObject>();
			if (obj == null) return;

			Resource r = obj.Resource;
			if (r.IsPlantable)
			{
				SetState(eTileState.Planted);
			}
			break;

		case eTileState.Planted:
			// TODO decide if you can return to a previous state if planted
			break;
		}
	}
}
