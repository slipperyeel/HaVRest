using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public static class HVRItemFactory
{
    private static readonly string ITEM_DIRECTORY = "Prefabs/Items";
    private static readonly string RESOURCES_DIRECTORY = "Resources";
    private static List<GameObject> mItemPrefabs = new List<GameObject>();

    public static bool IsFactoryInitialized = false;

    public static void LoadItems()
    {
        IsFactoryInitialized = false;
        mItemPrefabs.Clear();
        DataManager.Instance.StartCoroutine(DoLoadWork());
    }

    private static IEnumerator DoLoadWork()
    {
        DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/" + RESOURCES_DIRECTORY + "/" + ITEM_DIRECTORY);
        FileInfo[] info = dir.GetFiles("*.prefab");
        for (int i = 0; i < info.Length; i++)
        {
            GameObject loadedObject = null;
            try
            {
                string fileName = ITEM_DIRECTORY + "/" + Path.GetFileNameWithoutExtension(info[i].FullName);
                loadedObject = (GameObject)Resources.Load(fileName);
                mItemPrefabs.Add(loadedObject);
            }
            catch (Exception e)
            {
                Debug.Log("Prefab with name: " + info[i].Name + " is not valid." + " GENERIC ERROR: " + e);
            }

            yield return null;
        }

        IsFactoryInitialized = true;
    }

    public static GameObject GetItemPrefab(ItemEnums item)
    {
        GameObject prefab = null;
        for(int i = 0; i < mItemPrefabs.Count; i++)
        {
            ItemFactoryData data = mItemPrefabs[i].GetComponent<ItemFactoryData>();
            if(data != null)
            {
                if(data.ItemEnum == item)
                {
                    prefab = mItemPrefabs[i];
                    break;
                }
            }
        }
        return prefab;
    }
}