using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestSaveLoad : MonoBehaviour 
{
    public bool Load = false;

    void Start()
    {
        if (Load)
        {
            DataManager.Instance.LoadGameData(null);
        }
    }
}