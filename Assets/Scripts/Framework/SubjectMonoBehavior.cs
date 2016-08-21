using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A simple abstract class to enforce implementation of the subject pattern.
/// note: only abstract so that this class must be used as a base class.
/// </summary>
public abstract class SubjectMonoBehaviour : MonoBehaviour
{
    // Set of observers
    private List<ObserverMonoBehaviour> mObservers;

    //////////////////////////////////////////////////////////
    // Observer Pattern Implementation
    //////////////////////////////////////////////////////////

    /// <summary>
    /// Subscribe an observer to this subject.
    /// </summary>
    /// <param name="observer">a MonoBehaviour which implements ObserverMonoBehaviour</param>
    public void Subscribe(ObserverMonoBehaviour observer)
    {
        if (observer != null)
        {
            if (mObservers != null)
            {
                mObservers.Add(observer);
            }
        }
    }

    /// <summary>
    /// UnSubscribe an observer to this subject.
    /// </summary>
    /// <param name="observer">a MonoBehaviour which implements ObserverMonoBehaviour</param>
    /// <returns>true if the unsubscribe was succesful</returns>
    public bool UnSubscribe(ObserverMonoBehaviour observer)
    {
        if (observer != null)
        {
            if (mObservers != null)
            {
                return mObservers.Remove(observer);
            }
        }
        return false;
    }

    /// <summary>
    /// Notifies all subscribed observers that the state of this subject has changed.
    /// </summary>
    public void Notify()
    {
        int obsCount = mObservers.Count;
        for (int i = 0; i < obsCount; i++)
        {
            mObservers[i].Message(this);
        }
    }

    //////////////////////////////////////////////////////////
    // MonoBehaviour Implementation
    //////////////////////////////////////////////////////////

    protected virtual void Awake()
    {
        mObservers = new List<ObserverMonoBehaviour>();
    }
}
