/// <summary>
/// Temporal Triggers. These are the types of temporal triggers that can affect a temporal object
/// </summary>
public enum eTemporalTriggerType
{
    None = -1,
    Hour,
    Day,
    Month,
    Year
}

/// <summary>
/// Temporal Trigger Outcomes. This list will probably get really large.
/// </summary>
public enum eTemporalTriggerOutcome
{
    None = -1,
    Meat_Spoil,
    Crop_Grow,
    Crop_Harvestable,
    Crop_Spoiled,
    Crop_Dead,
    Food_Unripe,
    Food_Ripe,
    Food_Spoiled,
    Food_Rotten
}

/// <summary>
/// Resource Types. These will be paired with a quantity and consumed by resource dependent objects.
/// Note: We can also use the resource system to do vitals.
/// </summary>
public enum eResourceType
{
    None = -1,
    Health,
    Stamina,
    Heat,
    Water,
    Food,
    Fuel,
    Flameable
}

public enum eImpactType
{
    None = -1,
    Blunt,
    Sharp,
    Pierce
}

public enum eMoisture
{
	Dry = 0,
	Mild,
	Normal,
	Damp,
	Wet
}

public enum eTemperature
{
	Hot = 0,
	Warm,
	Normal,
	Cool,
	Cold
}

public enum eBiome
{
	Desert = 0,
	Savannah,
	Plains,
	Forest,
	Swamp,
	Jungle,
	Tundra
}

public enum eTileType
{
    Dirt = 0,
    Grass,
    Stone,
    Sand, // special
    Snow  // special
}

public enum eDetailType
{
    Tree = 0,
    Grass,
    Fern,
    Flower,
    Rock
}

public enum eTileState
{
	Invalid,
	Normal,
	Tilled,
	Dug,
	Planted
}