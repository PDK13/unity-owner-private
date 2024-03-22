using UnityEngine;

public class ColliderPlatformData
{
    public Vector2 PointA { private set; get; }

    public Vector2 PointB { private set; get; }

    public Vector2[] Points => new Vector2[2] { PointA, PointB };

    public Vector2 PointCentre => (PointA + PointB) / 2;

    public float Deg => Mathf.Atan2(PointB.y - PointA.y, PointB.x - PointA.x) * Mathf.Rad2Deg;

    public float Length => Vector2.Distance(PointA, PointB);

    public ColliderPlatformData(Vector2 PointA, Vector2 PointB)
    {
        this.PointA = PointA;
        this.PointB = PointB;
    }
}