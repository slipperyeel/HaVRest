using UnityEngine;
using System.Collections;

public class HVR_ArcPointer : MonoBehaviour
{
    [SerializeField]
    private LineRenderer _lineRenderer;

    [SerializeField]
    private Transform _businessEnd;

    public bool TestPointer = false;

    private Vector3[] _points;
    private Vector3 _velocity;
    private int _count = 0;

    void Start()
    {
        _points = new Vector3[10];
        _velocity = _businessEnd.transform.forward * 10f;
    }

    void Update()
    {
        _lineRenderer.enabled = TestPointer;
        if (TestPointer)
        {
            TraceArcTrajectory(_businessEnd.position, _velocity);
            _lineRenderer.SetVertexCount(_count);
            _lineRenderer.SetPositions(_points);
        }
    }

    private void TraceArcTrajectory(Vector3 initialPosition, Vector3 initialVelocity)
    {
        float timeDelta = 1.0f / initialVelocity.magnitude; // for example
        Vector3 gravity = Physics.gravity;

        bool positionFound = false;
        _count = 0;

        Vector3 position = initialPosition;
        Vector3 velocity = initialVelocity;
        while (!positionFound)
        {
            position += velocity * timeDelta + 0.5f * gravity * timeDelta * timeDelta;
            velocity += gravity * timeDelta;

            if (_count < 10)
            {
                _points[_count] = position;
            }

            _count += 1;
        
            positionFound = (position.y <= 0f);
        }
    }
}
