using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class DataManager : Singleton<DataManager>
{
    private List<GameObjectMomento> Momentos;
	private Dictionary<GameObjectMomento, object> dataDictionary;
	private static readonly string PREFABS_PATH = "Prefabs/";
	private static readonly string SAVEFILE_NAME = "SaveData.dat";

    protected void Awake()
    {
        Momentos = new List<GameObjectMomento>();
		dataDictionary = new Dictionary<GameObjectMomento, object> ();
    }

    /// <summary>
    /// All GameObjects must be spawned through here.
    /// </summary>
    /// <param name="prefab"></param>
    public T SpawnObject<T, M>(GameObject prefab, Vector3 pos, Quaternion rot, Vector3 scale)
        where M : GameObjectMomento
    {
        if (prefab != null)
        {
            object obj = GameObject.Instantiate(prefab, pos, rot);
			GameObject spawnObj;
			T typedObject;

			if (obj is GameObject)
            {
				spawnObj = (GameObject)obj;
            }
            else
            {
                try
                {
					spawnObj = (GameObject)Convert.ChangeType(obj, typeof(GameObject));
                }
                catch (InvalidCastException)
                {
					spawnObj = default(GameObject);
                }
            }

			if(spawnObj != null)
			{
				typedObject = spawnObj.GetComponent<T>();

	            // Add it to the momento list
	            object mObj = Activator.CreateInstance(typeof(M));
	            M momento = (M)Convert.ChangeType(mObj, typeof(M));

				momento.UpdateMomentoData(spawnObj, prefab.name);
	            Momentos.Add(momento);
				dataDictionary.Add (momento, spawnObj);
				return typedObject;
			}
			else
			{
				return default(T);
			}
        }

        return default(T);
    }

    public void SaveGameData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
		FileStream fs = File.Open(Application.persistentDataPath + "/" + SAVEFILE_NAME, FileMode.OpenOrCreate);

		for (int i = 0; i < Momentos.Count; i++) 
		{
			object obj;

			if (dataDictionary.TryGetValue (Momentos [i], out obj)) 
			{
				Momentos [i].UpdateMomentoData (obj, Momentos[i].PrefabName);
			}
		}
        formatter.Serialize(fs, Momentos);

        fs.Close();

		Debug.Log ("Game Data Saved");
    }

    public void LoadGameData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
		FileStream fs = File.Open(Application.persistentDataPath + "/" + SAVEFILE_NAME, FileMode.Open);

        Momentos = (List<GameObjectMomento>)formatter.Deserialize(fs);

        if (Momentos != null && Momentos.Count > 0)
        {
            for (int i = 0; i < Momentos.Count; i++)
            {
				GameObject loadedObject = (GameObject)Resources.Load<GameObject>(PREFABS_PATH + Momentos[i].PrefabName);

				if(loadedObject != null)
				{
					loadedObject = GameObject.Instantiate(loadedObject);
					Momentos[i].ApplyMomentoData(loadedObject);
				}
            }
        }

        fs.Close();

		Debug.Log ("Game Data Loaded");
    }
}