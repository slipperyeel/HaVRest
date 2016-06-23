using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestSaveLoad : MonoBehaviour 
{
    [SerializeField]
    private List<GameObject> GameObjectsToSpawn;

    public bool Load = false;

    void Start()
    {
        if (!Load)
        {
            for (int i = 0; i < GameObjectsToSpawn.Count; i++)
            {
                DataManager.Instance.SpawnObject<TestObject, TestMomento>(GameObjectsToSpawn[i], Vector3.zero, Quaternion.identity, Vector3.zero);
            }

			StartCoroutine (TimeDelayForTesting ());
        }
        else
        {
            DataManager.Instance.LoadGameData();
        }
    }

	private IEnumerator TimeDelayForTesting()
	{
		yield return new WaitForSeconds(10.0f);
		DataManager.Instance.SaveGameData();
	}
}