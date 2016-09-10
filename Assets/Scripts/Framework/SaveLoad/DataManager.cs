using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/// <summary>
/// First pass at an I/O Manager... currently only saves game object type stuff.
/// </summary>
public class DataManager : Singleton<DataManager>
{
    private List<GameObjectMomento> mMomentos;
    private Dictionary<object, GameObjectMomento> mDataDictionary;
    private static readonly string PREFABS_PATH = "Prefabs/";
    private static readonly string SAVEFILE_NAME = "SaveData.dat";
    private bool mIsSaving = false;
    private bool mIsLoading = false;
    private bool mIsDataLoaded = false;
    private bool mIsFirstBoot = false;

    public bool IsLoading
    {
        get { return mIsLoading; }
    }

    public bool IsDataLoaded
    {
        get { return mIsDataLoaded; }
    }

    public bool IsFirstBoot
    {
        get { return mIsFirstBoot; }
    }

    public void Init()
    {
        mMomentos = new List<GameObjectMomento>();
        mDataDictionary = new Dictionary<object, GameObjectMomento>();

        HVRItemFactory.LoadItems();
        StartCoroutine(TestFactoryEnumerator());
    }

    private IEnumerator TestFactoryEnumerator()
    {
        while(!HVRItemFactory.IsFactoryInitialized)
        {
            yield return null;
        }
        GameObject unused;
        HVRItemFactory.SpawnItem(ItemEnums.TestItem, Vector3.zero, default(Quaternion), Vector3.one, out unused, "Test Spawn Object");
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
            GameObject gameObj = (GameObject)obj;
            gameObj.transform.localScale = scale;
            
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
                Debug.Log(momento);
				momento.UpdateMomentoData(spawnObj, prefab.name);
	            mMomentos.Add(momento);
                mDataDictionary.Add(spawnObj, momento);

				return typedObject;
			}
			else
			{
				return default(T);
			}
        }

        return default(T);
    }

    public void DestroyObject(GameObject obj)
    {
        if (obj != null)
        {
            if (mDataDictionary != null && mMomentos != null)
            {
                if (mDataDictionary.ContainsKey(obj))
                {
                    GameObjectMomento momento = mMomentos.Find(m => m.UniqueId == mDataDictionary[obj].UniqueId);

                    if (momento != null)
                    {
                        mMomentos.Remove(momento);
                    }

                    mDataDictionary.Remove(obj);

                    GameObject.Destroy(obj);
                }
            }
        }
    }

    public void SaveGameData()
    {
        if (!mIsSaving)
        {
            mIsSaving = true;
            StartCoroutine(DoSave());
        }
    }

    private IEnumerator DoSave()
    {
        if (mMomentos != null && mMomentos.Count > 0)
        {
            if (mDataDictionary != null && mDataDictionary.Count > 0)
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream fs = File.Open(Application.persistentDataPath + "/" + SAVEFILE_NAME, FileMode.OpenOrCreate);

                foreach (KeyValuePair<object, GameObjectMomento> entry in mDataDictionary)
                {
                    entry.Value.UpdateMomentoData(entry.Key, entry.Value.PrefabName);
                    yield return null;
                }

                formatter.Serialize(fs, mMomentos);

                fs.Close();

                Debug.Log("Game Data Saved");
            }
        }

        mIsSaving = false;
    }

    public void LoadGameData(Action onDataLoadedCb)
    {
        if (!mIsDataLoaded)
        {
            if (!mIsLoading && !mIsSaving)
            {
                mIsLoading = true;
                StartCoroutine(DoLoad(onDataLoadedCb));
            }
        }
    }

    private IEnumerator DoLoad(Action onDataLoadedCb)
    {
        if (System.IO.File.Exists(Application.persistentDataPath + "/" + SAVEFILE_NAME))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream fs = File.Open(Application.persistentDataPath + "/" + SAVEFILE_NAME, FileMode.Open);

            mMomentos = (List<GameObjectMomento>)formatter.Deserialize(fs);

            if (mMomentos != null && mMomentos.Count > 0)
            {
                for (int i = 0; i < mMomentos.Count; i++)
                { 
                    GameObject loadedObject = (GameObject)Resources.Load<GameObject>(PREFABS_PATH + mMomentos[i].PrefabName);

                    yield return null;

                    if (loadedObject != null)
                    {
                        loadedObject = GameObject.Instantiate(loadedObject);
                        mMomentos[i].ApplyMomentoData(loadedObject);

                        yield return null;
                    }

                    if (!mDataDictionary.ContainsKey(loadedObject))
                    {
                        mDataDictionary.Add(loadedObject, mMomentos[i]);
                    }
                }
            }

            fs.Close();

            mIsFirstBoot = false;

            if (onDataLoadedCb != null)
            {
                onDataLoadedCb();
            }

            Debug.Log("Game Data Loaded");
        }
        else
        {
            if (onDataLoadedCb != null)
            {
                mIsFirstBoot = true;
                onDataLoadedCb();
            }
            Debug.Log("No save file exists.");
        }

        mIsLoading = false;
        mIsDataLoaded = true;
    }

	public override void OnDestroy()
	{
		mMomentos.Clear ();
		mMomentos = null;
		mDataDictionary.Clear ();
		mDataDictionary = null;
		base.OnDestroy ();
	}
}