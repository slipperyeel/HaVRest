using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// A world Chunk. Implements the Observer Monobehaviour pattern.
/// </summary>
public class WorldChunk : ObserverMonoBehaviour
{
    /// <summary>
    /// Sets chunks inactive if they are outside of the range.
    /// </summary>
    /// <param name="subject"></param>
    public override void Message(object subject)
    {
        if (subject != null)
        {
            PlayerSubject gObj = (PlayerSubject)subject;

            if (gObj != null)
            {
                Player player = gObj.GetComponent<Player>();
                {
                    Vector3 pPos = player.transform.position;
                    float distanceTo = Mathf.Abs(Vector3.Distance(this.transform.position, pPos));
                    if(distanceTo > GameManager.Instance.WorldChunkMinViewDistance)
                    {
                        this.gameObject.SetActive(false);
                    }
                    else
                    {
                        this.gameObject.SetActive(true);
                    }
                }
            }
        }
    }
}
    