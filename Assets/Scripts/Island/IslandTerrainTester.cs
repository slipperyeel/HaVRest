using UnityEngine;
using System.Collections;

public class IslandTerrainTester : MonoBehaviour
{
	private KeyCode[] _numberKeys = new KeyCode[]
	{
		KeyCode.Alpha0,
		KeyCode.Alpha1,
		KeyCode.Alpha2,
		KeyCode.Alpha3,
		KeyCode.Alpha4,
		KeyCode.Alpha5,
		KeyCode.Alpha6,
		KeyCode.Alpha7,
		KeyCode.Alpha8,
		KeyCode.Alpha9,
	};
	private int _numOfKeys;
		
	void Awake()
	{
		_numOfKeys = _numberKeys.Length;
	}

	void Update()
	{
		CheckKeyInput();
	}

	private void CheckKeyInput()
	{
		for (int i = 0; i < _numOfKeys; ++i)
		{
			if (Input.GetKeyDown(_numberKeys[i]))
			{
				WorldGenerator.Instance.SetCurrentIsland(i);
			}
		}

		if (Input.GetKeyDown("a"))
		{
			WorldGenerator.Instance.TestDropSphere(3, 8);
		}
		else if (Input.GetKeyDown("b"))
		{
			WorldGenerator.Instance.TestDropSphere(40, 16);
		}
		else if (Input.GetKeyDown("c"))
		{
			WorldGenerator.Instance.TestDropSphere(12, 60);
		}
		else if (Input.GetKeyDown("x"))
		{
			WorldGenerator.Instance.RaiseTerrain(3, 8, 0.25f);
		}
		else if (Input.GetKeyDown("y"))
		{
			WorldGenerator.Instance.RaiseTerrain(40, 16, 0.5f);
		}
		else if (Input.GetKeyDown("z"))
		{
			WorldGenerator.Instance.RaiseTerrain(12, 60, -0.5f);
		}
	}
}
