using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class PlatformCreator : MonoBehaviour
{
    [SerializeField] private LayerMask m_colliderMask;
    [SerializeField] private float m_degLimit = 90f;

    private PolygonCollider2D m_poligonColider;
    private PlatformCreatorData m_platformData;

    private void Awake()
    {
        m_poligonColider = GetComponent<PolygonCollider2D>();
        //
        m_platformData = new PlatformCreatorData(m_poligonColider, m_degLimit);
    }

    private void Start()
    {
        m_platformData.SetInit();
        m_platformData.SetGenerate(m_colliderMask);
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            if (m_poligonColider == null)
                m_poligonColider = GetComponent<PolygonCollider2D>();
            //
            PlatformCreatorData ShapePlatformData = new PlatformCreatorData(m_poligonColider, m_degLimit);
            ShapePlatformData.SetInit();
            //
            Gizmos.color = Color.red;
            for (int i = 0; i < ShapePlatformData.Platform.Length; i++)
            {
                Vector2 PointA = transform.position + (Vector3)ShapePlatformData.Platform[i].PointA;
                Vector2 PointB = transform.position + (Vector3)ShapePlatformData.Platform[i].PointB;
                //
                Gizmos.DrawLine(PointA, PointB);
                Gizmos.DrawWireSphere(PointA, 0.05f);
                Gizmos.DrawWireSphere(PointB, 0.05f);
            }
        }
    }
}

public class PlatformCreatorData
{
    [SerializeField] private PolygonCollider2D m_polygonCollider;
    [SerializeField] private float DegLimit;
    [SerializeField] private List<ShapePlatformSingle> m_platform = new List<ShapePlatformSingle>();

    public ShapePlatformSingle[] Platform => m_platform.ToArray();

    public PlatformCreatorData(PolygonCollider2D polygonCollider, float degLimit)
    {
        m_polygonCollider = polygonCollider;
        DegLimit = degLimit;
    }

    //

    public void SetGenerate()
    {
        if (!Application.isPlaying)
            return;
        //
        GameObject Group = new GameObject("platform-group");
        //
        Transform GroupTransform = Group.transform;
        GroupTransform.SetParent(m_polygonCollider.transform);
        GroupTransform.localPosition = Vector3.zero;
        //
        for (int i = 0; i < m_platform.Count; i++)
        {
            GameObject Platform = new GameObject("platform");
            Platform.layer = m_polygonCollider.gameObject.layer;
            Platform.tag = m_polygonCollider.gameObject.tag;
            //
            Transform PlatformTransform = Platform.transform;
            PlatformTransform.SetParent(GroupTransform);
            PlatformTransform.localPosition = Vector3.zero;
            //
            EdgeCollider2D EdgeCollider2D = Platform.AddComponent<EdgeCollider2D>();
            EdgeCollider2D.points = m_platform[i].Points;
            EdgeCollider2D.usedByEffector = true;
            //
            PlatformEffector2D PlatformEffector2D = Platform.AddComponent<PlatformEffector2D>();
            PlatformEffector2D.useColliderMask = false;
            PlatformEffector2D.surfaceArc = 160f;
            PlatformEffector2D.rotationalOffset = m_platform[i].Deg;
        }
        //
        m_polygonCollider.enabled = false;
    }

    public void SetGenerate(LayerMask ColliderMask)
    {
        if (!Application.isPlaying)
            return;
        //
        GameObject Group = new GameObject("platform-group");
        //
        Transform GroupTransform = Group.transform;
        GroupTransform.SetParent(m_polygonCollider.transform);
        GroupTransform.localPosition = Vector3.zero;
        //
        for (int i = 0; i < m_platform.Count; i++)
        {
            GameObject Platform = new GameObject("platform");
            Platform.layer = m_polygonCollider.gameObject.layer;
            Platform.tag = m_polygonCollider.gameObject.tag;
            //
            Transform PlatformTransform = Platform.transform;
            PlatformTransform.SetParent(GroupTransform);
            PlatformTransform.localPosition = Vector3.zero;
            //
            EdgeCollider2D EdgeCollider2D = Platform.AddComponent<EdgeCollider2D>();
            EdgeCollider2D.points = m_platform[i].Points;
            EdgeCollider2D.usedByEffector = true;
            //
            PlatformEffector2D PlatformEffector2D = Platform.AddComponent<PlatformEffector2D>();
            PlatformEffector2D.colliderMask = ColliderMask;
            PlatformEffector2D.surfaceArc = 160f;
            PlatformEffector2D.rotationalOffset = m_platform[i].Deg;
        }
        //
        m_polygonCollider.enabled = false;
    }

    //

    public void SetInit()
    {
        m_platform = new List<ShapePlatformSingle>();
        //
        for (int Group = 0; Group < m_polygonCollider.pathCount; Group++)
        {
            //=== GET POINTS IN GROUP
            Vector2[] Point = m_polygonCollider.GetPath(Group);
            //
            //=== CHECK POINTS IN GROUP
            for (int Index = 0; Index < Point.Length - 1; Index++)
                SetInit(Point[Index], Point[Index + 1]);
            SetInit(Point[Point.Length - 1], Point[0]);
        }
    }

    private void SetInit(Vector2 PointA, Vector2 PointB)
    {
        if (PointA.x >= PointB.x)
            return;
        //
        double Deg = Math.Atan2(PointB.y - PointA.y, PointB.x - PointA.x) * Mathf.Rad2Deg;
        if (Deg > DegLimit || Deg < -DegLimit)
            return;
        //
        this.m_platform.Add(new ShapePlatformSingle(PointA, PointB));
    }
}

public class ShapePlatformSingle
{
    public Vector2 PointA { private set; get; }

    public Vector2 PointB { private set; get; }

    public Vector2[] Points => new Vector2[2] { PointA, PointB };

    public Vector2 PointCentre => (PointA + PointB) / 2;

    public float Deg => Mathf.Atan2(PointB.y - PointA.y, PointB.x - PointA.x) * Mathf.Rad2Deg;

    public float Length => Vector2.Distance(PointA, PointB);

    public ShapePlatformSingle(Vector2 PointA, Vector2 PointB)
    {
        this.PointA = PointA;
        this.PointB = PointB;
    }
}