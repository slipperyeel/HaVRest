using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HVRPuzzle;

[System.Serializable]
public class PuzzleCollectionData : ScriptableObject
{
    [SerializeField]
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
