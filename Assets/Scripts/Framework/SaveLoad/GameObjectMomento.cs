using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class GameObjectMomento
{
    private SerializeV3 mPosition;
    public Vector3 Position
    {
        get { return mPosition.ToVector3(); }
    }

    private SerializeQ mRotation;
    public Quaternion Rotation
    {
        get { return mRotation.ToQuaternion(); }
    }

    private SerializeV3 mScale;
    public Vector3 Scale
    {
        get { return mScale.ToVector3(); }
    }

    private bool mIsEnabled = true;
    public bool Active
    {
        get { return mIsEnabled; }
    }

    protected string mPrefabName;
    public string PrefabName
    {
        get { return mPrefabName; }
    }

    private string mUniqueId;
    public string UniqueId
    {
        get { return mUniqueId; }
    }

    public GameObjectMomento()
    {
        mPosition = new SerializeV3(Vector3.zero);
        mRotation = new SerializeQ(Quaternion.identity);
        mScale = new SerializeV3(Vector3.one);
        mIsEnabled = true;
        mPrefabName = "";
        mUniqueId = Guid.NewGuid().ToString();
    }

    public virtual void UpdateMomentoData(object obj, string prefabName)
    {

        if (obj != null)
		{
			GameObject go = (GameObject)obj;
            Debug.Log(go.name);
            if (go != null)
            {

                mPosition = new SerializeV3(go.transform.position);
                mRotation = new SerializeQ(go.transform.rotation);
                mScale = new SerializeV3(go.transform.localScale);
				mIsEnabled = go.activeSelf;
                mPrefabName = prefabName;
                Debug.Log("mPosition: " + mPosition.x);
            }
        }
    }

    public virtual void ApplyMomentoData(object obj)
    {
        if (obj != null)
        {
            GameObject go = (GameObject)obj;
            if (go != null)
            {
                Debug.Log(this.mPosition.ToVector3());
                go.transform.position = this.mPosition.ToVector3();
                go.transform.rotation = this.mRotation.ToQuaternion();
                go.transform.localScale = this.mScale.ToVector3();
				go.SetActive (this.mIsEnabled);
            }
        }
    }
}

[Serializable]
public class SerializeV3
{
    public float x;
    public float y;
    public float z;

    public SerializeV3(Vector3 v3)
    {
        x = v3.x;
        y = v3.y;
        z = v3.z;
    }

    public Vector3 ToVector3()
    {
        Vector3 v3 = new Vector3(this.x, this.y, this.z);
        return v3;
    }
}

[Serializable]
public class SerializeQ
{
    public float x;
    public float y;
    public float z;
    public float w;

    public SerializeQ(Quaternion q)
    {
        x = q.x;
        y = q.y;
        z = q.z;
        w = q.w;
    }

    public Quaternion ToQuaternion()
    {
        Quaternion q = new Quaternion(this.x, this.y, this.z, this.w);
        return q;
    }
}