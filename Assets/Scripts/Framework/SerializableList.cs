using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

[Serializable]
// TODO: Add more collections control.
// TODO: Comment Code
public class SerializableList<T> : ISerializable
{
    private List<T> mInternalList;
    private string mKeyPrefix;

    // Keys
    private static readonly string kCountSuffixKey = "_count";

    public SerializableList()
    {
        mKeyPrefix = "";
        mInternalList = new List<T>();
    }

    public SerializableList(string keyPrefix)
    {
        mKeyPrefix = keyPrefix;
        mInternalList = new List<T>();
    }

    public int Count
    {
        get
        {
            if (mInternalList != null)
            {
                return mInternalList.Count;
            }
            else
            {
                return 0;
            }
        }
    }

    public T this[int index]
    {
        get
        {
            try
            {
                return mInternalList[index];
            }
            catch (System.NullReferenceException)
            {
                Debug.LogError("SerializableList has not been initialized, returning default value for type T");
                return default(T);
            }
        }
        set
       { 
            try
            {
                mInternalList[index] = value;
            }
            catch (System.Exception ex)
            {
                if (ex is NullReferenceException)
                {
                    Debug.LogError("SerializableList has not been initialized. Item cannot be added.");
                }
                else if (ex is IndexOutOfRangeException)
                {
                    Debug.LogError("Index out of range for internal list.");
                }
            }
        }
    }

    public void Add(T item)
    {
        if (mInternalList != null)
        {
            mInternalList.Add(item);
        }
    }

    public bool Remove(T item)
    {
        bool wasRemoved = false;
        if (mInternalList != null)
        {
            wasRemoved = mInternalList.Remove(item);
        }
        return wasRemoved;
    }

    ////////////////////////////////////////////////////////////////////////////
    /// Implementation of ISerializable (C# .Net)
    ////////////////////////////////////////////////////////////////////////////

    public SerializableList(SerializationInfo information, StreamingContext context)
    {
        mInternalList = new List<T>();
        int count = (int)information.GetValue(mKeyPrefix + kCountSuffixKey, typeof(int));

        for (int i = 0; i < count; i++)
        {
            mInternalList.Add((T)information.GetValue(mKeyPrefix + i, typeof(T)));
        }
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        if (mInternalList != null)
        {
            info.AddValue(mKeyPrefix + kCountSuffixKey, mInternalList.Count);
            for (int i = 0; i < mInternalList.Count; i++)
            {
                info.AddValue(mKeyPrefix + i, mInternalList[i]);
            }
        }
    }
}
