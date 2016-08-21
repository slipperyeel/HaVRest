using UnityEngine;
using System.Collections;

/// <summary>
/// A simple abstract class to enforce implementation of the observer pattern.
/// </summary>
public abstract class ObserverMonoBehaviour : MonoBehaviour
{
    //////////////////////////////////////////////////////////
    // Observer Pattern Implementation
    //////////////////////////////////////////////////////////

    /// <summary>
    /// Implement update specifically to handle the type of Subject you are observing.
    /// </summary>
    /// <param name="subject">The subject object this observer is monitoring the state of.</param>
    public abstract void Message(object subject);
}
