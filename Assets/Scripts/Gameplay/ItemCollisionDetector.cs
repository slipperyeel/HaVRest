using UnityEngine;
using System.Collections;

/// <summary>
/// Class: ItemCollisionDectector
/// Subclass Of: MonoBehaviour
/// Purpose: To provide Collision information to a parent object so that a unique
/// collider can be used for touch / control and interaction with the world / other items.
/// </summary>
public class ItemCollisionDetector : MonoBehaviour
{
    // Member Variables
    private Collider mCollider;
    private Rigidbody mRigidBody;

    void Awake()
    {
        mCollider = GetComponent<Collider>();
        mRigidBody = GetComponent<Rigidbody>();

        Debug.Assert(mCollider != null, "ItemCollisionDetector needs a Collider in order to work.");
        //Debug.Assert(mRigidBody != null, "ItemCollisionDetector needs a RigidBody in order to work.");
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("ItemCollisioNDETECTORRRRRR");
        this.SendMessageUpwards("ItemCollisionDetector_CollisionEnter", collision);
    }

    void OnCollisionExit(Collision collision)
    {
        this.SendMessageUpwards("ItemCollisionDetector_CollisionExit", collision);
    }

    void OnCollisionStay(Collision collision)
    {
        this.SendMessageUpwards("ItemCollisionDetector_CollisionStay", collision);
    }
}
