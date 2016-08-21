using UnityEngine;
using System.Collections;

/// <summary>
/// Simple implementation of the subject monobehaviour for the player.
/// Automatically grabs and subscribes the collection, This should probably be done in the game manager but
/// this works for now.
/// </summary>
public class PlayerSubject : SubjectMonoBehaviour
{
    void Start()
    {
        WorldChunkCollection col = GameManager.Instance.ChunkCollection;
        if (col != null)
        {
            int colCount = col.Count;
            for (int i = 0; i < colCount; i++)
            {
                this.Subscribe(col[i]);
            }
        }
    }
	
	void Update ()
    {
        Notify();
	}
}
