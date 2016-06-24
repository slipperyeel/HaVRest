using UnityEngine;
using System.Collections;

/// <summary>
/// This is a test class to spawn objects and save data.
/// </summary>
public class DebugInput : MonoBehaviour 
{
	[SerializeField]
	private GameObject obj1;

	[SerializeField]
	private GameObject obj2;

	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Q)) 
        {
            DataManager.Instance.SpawnObject<TemporalCrop, CropMomento>(obj1, this.transform.position, this.transform.rotation, Vector3.one);
        }
        else if (Input.GetKeyDown(KeyCode.E))
		{
            DataManager.Instance.SpawnObject<TestObject, TestMomento>(obj2, this.transform.position, this.transform.rotation, Vector3.one);
		}
        else if (Input.GetKeyDown(KeyCode.V))
        {
            DataManager.Instance.SaveGameData();
        }
	}
}
