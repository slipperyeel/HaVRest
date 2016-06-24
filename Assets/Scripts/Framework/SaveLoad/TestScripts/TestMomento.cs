using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class TestMomento : GameObjectMomento
{
    private string Derp;

    public override void UpdateMomentoData(object obj, string prefabName)	
    {
        if (obj != null)
        {
			GameObject go = (GameObject)obj;
			if (go != null) 
			{
				TestObject test = go.GetComponent<TestObject> ();
				if (test != null)
				{
					Derp = test.Derp;
				}
				base.UpdateMomentoData(go, prefabName);
			}
        }
    }

    public override void ApplyMomentoData(object obj)
    {
        if (obj != null)
        {
			GameObject go = (GameObject)obj;
			if (go != null) 
			{
				TestObject test = go.GetComponent<TestObject> ();

                if (test != null)
                {
                    test.Derp = Derp;
                    base.ApplyMomentoData(test.gameObject);
                }
			}
        }
    }
}
