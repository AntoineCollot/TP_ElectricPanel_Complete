using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerletRope : MonoBehaviour
{
    struct VerletPoint
    {
        public Vector3 position;
        public Vector3 oldPosition;
        public bool isFree;
    }

    [Header("Rope Attach")]
    public Transform ropeStart;
    public Transform ropeEnd;
    VerletPoint[] points;

    [Header("Rope Settings")]
    [Min(2)] public int lineSegments = 10;
    [Min(0)] public float lineLength = 1.3f;
    [Min(1)] public int solverIterations = 3;
    public float gravity;

    LineRenderer line;

    public float SegmentLength { get => lineLength / lineSegments; }

    // Start is called before the first frame update
    void Start()
    {
        CreateRope();
        line = GetComponent<LineRenderer>();
        line.positionCount = lineSegments;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLineVisuals();
    }

    private void FixedUpdate()
    {
        Simulate();
    }

    void CreateRope()
    {
        points = new VerletPoint[lineSegments];

        for (int i = 0; i < lineSegments; i++)
        {
            points[i].position = Vector3.Lerp(ropeStart.position, ropeEnd.position, (float)(i+1)/lineSegments);
            points[i].oldPosition = points[i].position;
            points[i].isFree = true;
        }

        points[0].isFree = false;
        points[lineSegments-1].isFree = false;
    }

    void Simulate()
    {
        //Apply movement
        for (int i = 0; i < lineSegments; i++)
        {
            Vector3 displacement = points[i].position - points[i].oldPosition;
            points[i].position += displacement*0.3f;
            points[i].position += Vector3.down * gravity * Time.fixedDeltaTime;
        }

        //Update old pos
        for (int i = 0; i < lineSegments; i++)
        {
            points[i].oldPosition = points[i].position;
        }

        for (int i = 0; i < solverIterations; i++)
        {
            ApplyConstraints();
        }
    }

    void ApplyConstraints()
    {
        points[0].position = ropeStart.position;


        points[lineSegments - 1].position = ropeEnd.position;

        for (int i = 0; i < lineSegments - 1; i++)
        {
            ApplyDistanceConstraint(i, i + 1, SegmentLength);
        }
    }

    void ApplyDistanceConstraint(int pointA, int pointB, float desiredSegmentLength)
    {
        Vector3 pointAtoB = points[pointB].position - points[pointA].position;
        float currentSegmentLength = pointAtoB.magnitude;
        currentSegmentLength = Mathf.Max(0.001f, currentSegmentLength);
        float errorFactor = (currentSegmentLength - desiredSegmentLength) / currentSegmentLength;

        if (points[pointA].isFree && points[pointB].isFree)
        {
            points[pointA].position += errorFactor * 0.5f * pointAtoB;
            points[pointB].position -= errorFactor * 0.5f * pointAtoB;
        }
        else if (points[pointA].isFree)
        {
            points[pointA].position += errorFactor * pointAtoB;
        }
        else if (points[pointB].isFree)
        {
            points[pointB].position -= errorFactor * pointAtoB;
        }
    }

    void UpdateLineVisuals()
    {
        for (int i = 0; i < lineSegments; i++)
        {
            line.SetPosition(i, points[i].position);
        }
    }
}
