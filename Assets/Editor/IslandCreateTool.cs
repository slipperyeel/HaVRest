using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;

public class IslandCreateTool : EditorWindow
{
	public static int _islandCount = 0;
	private WorldGenerator.IslandSize _size = WorldGenerator.IslandSize.Small;
	private int _seed = 0;
	private int _levels = 3;
    //private string _firstSetPath = "Detail Objects/First Set/";
    private string _secondSetPath = "Detail Objects/Second Set/";

	[MenuItem("Tools/Island Creator")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(IslandCreateTool));
	}

	void OnGUI()
	{
		GUILayout.Label("- Island Parameters -");
		GUILayout.Space(20);

		// seed
		GUILayout.BeginHorizontal();
		GUILayout.Label("Seed: ");
		_seed = EditorGUILayout.IntField(_seed);
		if (GUILayout.Button("Generate"))
		{
			_seed = MakeSeed();
		}
		GUILayout.EndHorizontal();

		// size
		GUILayout.BeginHorizontal();
		_size = (WorldGenerator.IslandSize)EditorGUILayout.EnumPopup("Size: ", _size);
		GUILayout.EndHorizontal();

		// number of levels
		GUILayout.BeginHorizontal();
		_levels = EditorGUILayout.IntField("Levels: ", _levels);
		GUILayout.EndHorizontal();

		GUILayout.Space(20);

		if (GUILayout.Button("Generate Island"))
		{
			GenerateIsland();
		}
	}

	private int MakeSeed()
	{
		return UnityEngine.Random.Range(0, int.MaxValue);
	}

	private void GenerateIsland()
	{
		GameObject generatorObj = new GameObject();
		IslandGenerator gen = generatorObj.AddComponent<IslandGenerator>();

		GameObject[] detailObjects = new GameObject[]
		{
			Resources.Load<GameObject>(_secondSetPath + "Pine_0"),
			Resources.Load<GameObject>(_secondSetPath + "Pine_1"),
			Resources.Load<GameObject>(_secondSetPath + "Pine_2"),
			Resources.Load<GameObject>(_secondSetPath + "Rock_Med"),
			//Resources.Load<GameObject>(_secondSetPath + "Rock_Lg"),
			Resources.Load<GameObject>(_secondSetPath + "Flower_Bud"),
			//Resources.Load<GameObject>(_secondSetPath + "Bush_Sm"),
			Resources.Load<GameObject>(_secondSetPath + "Grass"),
			Resources.Load<GameObject>(_secondSetPath + "Fern"),
			//Resources.Load<GameObject>(_secondSetPath + "Rock_Small")
		};

		gen._detailObjects = detailObjects;
		GameObject terrainObj = gen.Generate(_size, _seed, _levels);

		PrefabUtility.CreatePrefab("Assets/Resources/Custom Islands/" + terrainObj.name + ".prefab", terrainObj);

		DestroyImmediate(generatorObj);
		DestroyImmediate(terrainObj);
	}
}
