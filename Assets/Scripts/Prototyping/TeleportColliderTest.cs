using UnityEngine;
using System.Collections;

public class TeleportColliderTest : MonoBehaviour
{
    // TODO mikes
    // get the size of room scale and set the scale of this object to that size rounded down

    // keep track of the number of tiles you're touching
    // if the number is 9, do soemthing to allow the teleport
    // if < 9, or you're touching something non-tile, NO TELEPORTING

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Tile"))
        {
            other.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Tile"))
        {
            other.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
