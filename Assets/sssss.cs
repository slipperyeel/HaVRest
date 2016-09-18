using UnityEngine;
using System.Collections;

public class sssss : MonoBehaviour {

    public GameObject topHand;
    public GameObject bottomHand;

    void FixedUpdate()
    {
        Vector3 heldDir = topHand.transform.position - bottomHand.transform.position;

        transform.forward = heldDir.normalized;
        transform.Rotate(Quaternion.Euler(0f, 0f, -Angle360(transform.up, topHand.transform.right, Vector3.right)).eulerAngles);
        float hiltDist = Vector3.Distance(bottomHand.transform.position, transform.position);
        Vector3 hiltDir = (bottomHand.transform.position - topHand.transform.position).normalized;

        Vector3 hiltOffset = hiltDir * hiltDist;
        transform.position = bottomHand.transform.position + hiltOffset;
    }
    float Angle360(Vector3 from, Vector3 to, Vector3 right)
    {
        float angle = Vector3.Angle(from, to);
        return (Vector3.Angle(right, to) > 90f) ? 360f - angle : angle;
    }
}
