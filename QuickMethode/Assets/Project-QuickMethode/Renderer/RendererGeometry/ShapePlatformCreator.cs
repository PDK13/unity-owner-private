using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;

[ExecuteAlways]
public class ShapePlatformCreator : MonoBehaviour
{
    [SerializeField] private SpriteShapeController m_spriteShape;
    [SerializeField] private CompositeCollider2D m_compositeCollider;
    [SerializeField] private PolygonCollider2D m_poligonColider;

    [SerializeField] private PlatformStruct m_platformStruct;

    private void Awake()
    {
        m_platformStruct = new PlatformStruct(m_compositeCollider, m_poligonColider);
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        m_platformStruct.SetGenerate2();
        //
        for (int i = 0; i < m_platformStruct.Platforms.Count; i++)
        {
            Debug.DrawLine(
                this.transform.position + (Vector3)m_compositeCollider.offset + (Vector3)m_platformStruct.Platforms[i].PointA,
                this.transform.position + (Vector3)m_compositeCollider.offset + (Vector3)m_platformStruct.Platforms[i].PointB,
                Color.red);
        }
    }
}

[Serializable]
public class PlatformStruct
{
    public List<PlatformSingle> Platforms = new List<PlatformSingle>();

    [SerializeField] private CompositeCollider2D m_compositeCollider;
    [SerializeField] private PolygonCollider2D m_polygonCollider;

    public PlatformStruct(CompositeCollider2D SpriteShape, PolygonCollider2D polygonCollider)
    {
        m_compositeCollider = SpriteShape;
        m_polygonCollider = polygonCollider;
    }

    public void SetGenerate2()
    {
        Platforms = new List<PlatformSingle>();
        //
        for (int Group = 0; Group < m_polygonCollider.pathCount; Group++)
        {
            //Get Points in Group Path!!
            List<Vector2> Points = m_polygonCollider.GetPath(Group).ToList();
            //
            //Check Points in Group!!
            Vector2 PointA, PointB;
            for (int Point = 0; Point < Points.Count - 1; Point++)
            {
                PointA = Points[Point];
                PointB = Points[Point + 1];
                SetPlatformAdd(PointA, PointB);
            }
            PointA = Points[Points.Count - 1];
            PointB = Points[0];
            SetPlatformAdd(PointA, PointB);
        }
    }

    private void SetPlatformAdd(Vector2 PointA, Vector2 PointB)
    {
        if (PointA.x >= PointB.x)
            return;
        //
        double Deg = Math.Atan2(PointB.y - PointA.y, PointB.x - PointA.x) * Mathf.Rad2Deg;        //if (Deg > 90f || Deg < -90f)
        if (Deg > 60f || Deg < -60f)
            return;
        //
        //
        this.Platforms.Add(new PlatformSingle(PointA, PointB));
    }
}

[Serializable]
public class PlatformSingle
{
    public Vector2 PointA;
    public Vector2 PointB;
    public float Deg;
    public float Length;

    public PlatformSingle(Vector2 PointA, Vector2 PointB)
    {
        this.PointA = PointA;
        this.PointB = PointB;
        //
        Deg = Mathf.Atan2(PointB.y - PointA.y, PointB.x - PointA.x) * Mathf.Rad2Deg;
        Length = Vector2.Distance(PointA, PointB);
    }
}