using UnityEngine;
using System.Collections;

[System.Serializable]
public class BiomeCollectionData : ScriptableObject
{
    [System.Serializable]
    public class BiomeData
    {
        public GameObject[] DetailObjects;
        public Texture[] Textures;
    }

    [SerializeField] private BiomeData[] _data;
    public BiomeData[] Data { get { return _data; } }

    public BiomeData GetBiome(eBiome biome)
    {
        return _data[(int)biome];
    }
}
