using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// An autonomous world chunk collection.
/// Must be attached to the node which contains all of the world chunks.
/// </summary>
public class WorldChunkCollection : MonoBehaviour
{
    private List<WorldChunk> mInternalChunkCollection;

    //////////////////////////////////////////////////////////
    // WorldChunkCollection Implementation
    //////////////////////////////////////////////////////////

    /// <summary>
    /// Indexer
    /// </summary>
    /// <param name="index">index</param>
    /// <returns>world chunk at index index.</returns>
    public WorldChunk this[int index]
    {
        get
        {
            try
            {
                return mInternalChunkCollection[index];
            }
            catch (System.NullReferenceException)
            {
                Debug.LogError("WorldChunkCollection has not been initialized, returning null");
                return null;
            }
        }
        set
        {
            try
            {
                mInternalChunkCollection[index] = value;
            }
            catch (System.Exception ex)
            {
                if (ex is NullReferenceException)
                {
                    Debug.LogError("WorldChunkCollection has not been initialized. Item cannot be added.");
                }
                else if (ex is IndexOutOfRangeException)
                {
                    Debug.LogError("Index out of range for internal list.");
                }
            }
        }
    }

    /// <summary>
    /// Add a WorldChunk. (If we add new ones after the game has launched.)
    /// </summary>
    /// <param name="item">a world chunk item.</param>
    public void Add(WorldChunk item)
    {
        if (mInternalChunkCollection != null)
        {
            mInternalChunkCollection.Add(item);
        }
    }

    /// <summary>
    /// When a world chunk is destroyed, it should be removed. We could probably set up a messaging system to make this auto 
    /// but it's fine for now.
    /// </summary>
    /// <param name="item">a world chunk item</param>
    /// <returns>true if successful.</returns>
    public bool Remove(WorldChunk item)
    {
        if (mInternalChunkCollection != null)
        {
            return mInternalChunkCollection.Remove(item);
        }
        return false;
    }

    /// <summary>
    /// Returns the count of the collection
    /// </summary>
    public int Count
    {
        get
        {
            if (mInternalChunkCollection != null)
            {
                return mInternalChunkCollection.Count;
            }
            return 0;
        }
    }

    //////////////////////////////////////////////////////////
    // MonoBehaviour Implementation
    //////////////////////////////////////////////////////////

    void Awake()
        {
            mInternalChunkCollection = new List<WorldChunk>();
            int childCount = this.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Transform childTransform = this.transform.GetChild(i);
                WorldChunk chunk = childTransform.gameObject.GetComponent<WorldChunk>();

                if (chunk != null)
                {
                    if (mInternalChunkCollection != null)
                    {
                        mInternalChunkCollection.Add(chunk);
                    }
                }
            }
        }

    void OnDestroy()
    {
        if(mInternalChunkCollection != null)
        {
            mInternalChunkCollection.Clear();
            mInternalChunkCollection = null;
        }
    }
}
