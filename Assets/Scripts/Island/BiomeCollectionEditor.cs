using UnityEngine;
using UnityEditor;
using System.Collections;

public class BiomeCollectionEditor : EditorWindow
{
    private static string kDirectoryPath = "Assets/Resources/Biome Data/";
    private static string kAssetName = "BiomeCollectionData";

    private static BiomeCollectionData mBiomeData = null;

    [MenuItem("Tools/Biome Collection Data/New Collection")]
    public static void CreateNewCollection()
    {
        string totalPath = string.Format("{0}{1}.asset", kDirectoryPath, kAssetName);
        mBiomeData = (BiomeCollectionData)ScriptableObject.CreateInstance("BiomeCollectionData");
        AssetDatabase.CreateAsset(mBiomeData, totalPath);
    }

    [MenuItem("Tools/Biome Collection Data/Delete Collection")]
    public static void ClearCollection()
    {
        DestroyImmediate(mBiomeData);
        mBiomeData = null;
    }
}
