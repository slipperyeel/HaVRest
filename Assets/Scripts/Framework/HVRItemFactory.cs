using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public static class HVRItemFactory
{
    public static readonly string ITEM_DIRECTORY = "Prefabs/Items";
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

    public static bool SpawnItem(ItemEnums item, Vector3 position, Quaternion rotation, Vector3 scale, out GameObject instantiatedObj, string name = null)
    {
        bool success = false;
        instantiatedObj = null;
        GameObject itemPrefab = GetItemPrefab(item);
        if (DataManager.Instance)
        {
            if (itemPrefab != null)
            {
                switch (item)
                {
                    case ItemEnums.TestItem:
                        {
                            TestObject obj = DataManager.Instance.SpawnObject<TestObject, TestMomento>(itemPrefab, position, rotation, scale);
                            instantiatedObj = obj.gameObject;
                            if (obj != null)
                            {
                                if(name != null)
                                {
                                    obj.name = name;
                                }
                                success = true;
                            }
                            break;
                        }
                    case ItemEnums.EggPlant_Fruit:
                    case ItemEnums.Cucumber_Fruit:
                    case ItemEnums.Pumpkin_Fruit:
                    case ItemEnums.Tomato_Fruit:
                    case ItemEnums.Watermelon_Fruit:
                        {
                            TemporalFoodStuff obj = DataManager.Instance.SpawnObject<TemporalFoodStuff, FoodStuffMomento>(itemPrefab, position, rotation, scale);
                            instantiatedObj = obj.gameObject;
                            if (obj != null)
                            {
                                if (name != null)
                                {
                                    obj.name = name;
                                }
                                success = true;
                            }
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
        }
        itemPrefab = null;
        return success;
    }
}