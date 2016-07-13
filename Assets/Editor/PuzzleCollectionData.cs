using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HVRPuzzle;

[System.Serializable]
public class PuzzleCollectionData : ScriptableObject
{
    [SerializeField, HideInInspector] // for some reason, on Unity 5.4 beta, showing this in the inspector spams errors
    private List<Puzzle> mCollection;
    public List<Puzzle> PuzzleCollection { get { return mCollection; } }

    public PuzzleCollectionData()
    {
        mCollection = new List<Puzzle>();
    }

    public void Add(Puzzle p)
    {
        mCollection.Add(p);
    }

    public void RemoveAt(int i)
    {
        mCollection.RemoveAt(i);
    }
}
