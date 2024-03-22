using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class ColliderPlatformShape : MonoBehaviour
{
    [SerializeField] private LayerMask m_colliderMask;
    [SerializeField] private float m_degLimit = 90f;

    private PolygonCollider2D m_poligon;
    private ColliderPlatformShapeData m_data;

    private void Awake()
    {
        m_poligon = GetComponent<PolygonCollider2D>();
        //
        m_data = new ColliderPlatformShapeData(m_poligon, m_degLimit);
    }

    private void Start()
    {
        m_data.SetInit();
        m_data.SetGenerate(m_colliderMask);
    }

    [ContextMenu("Init")]
    private void SetInit() 
    {
        m_poligon = GetComponent<PolygonCollider2D>();
        m_data = new ColliderPlatformShapeData(m_poligon, m_degLimit);
        m_data.SetInit();
        m_data.SetGenerate(m_colliderMask);
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            if (m_poligon == null)
                m_poligon = GetComponent<PolygonCollider2D>();
            //
            ColliderPlatformShapeData Data = new ColliderPlatformShapeData(m_poligon, m_degLimit);
            Data.SetInit();
            //
            Gizmos.color = Color.red;
            for (int i = 0; i < Data.Platform.Length; i++)
            {
                Vector2 PointA = transform.position + (Vector3)Data.Platform[i].PointA;
                Vector2 PointB = transform.position + (Vector3)Data.Platform[i].PointB;
                //
                Gizmos.DrawLine(PointA, PointB);
                Gizmos.DrawWireSphere(PointA, 0.05f);
                Gizmos.DrawWireSphere(PointB, 0.05f);
            }
        }
    }
}

public class ColliderPlatformShapeData
{
    private PolygonCollider2D m_poligon;
    private float DegLimit;
    private List<ColliderPlatformDataSingle> m_platform = new List<ColliderPlatformDataSingle>();

    public ColliderPlatformShapeData(PolygonCollider2D polygonCollider, float degLimit)
    {
        m_poligon = polygonCollider;
        DegLimit = degLimit;
    }

    //

    public void SetGenerate()
    {
        if (!Application.isPlaying)
            return;
        //
        GameObject Group = new GameObject("platform-group");
        Transform GroupTransform = Group.transform;
        GroupTransform.SetParent(m_poligon.transform);
        GroupTransform.localPosition = Vector3.zero;
        //
        for (int i = 0; i < m_platform.Count; i++)
        {
            GameObject Platform = new GameObject("platform");
            Platform.layer = m_poligon.gameObject.layer;
            Platform.tag = m_poligon.gameObject.tag;
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
        m_poligon.enabled = false;
    }

    public void SetGenerate(LayerMask ColliderMask)
    {
        if (!Application.isPlaying)
            return;
        //
        GameObject Group = new GameObject("platform-group");
        Transform GroupTransform = Group.transform;
        GroupTransform.SetParent(m_poligon.transform);
        GroupTransform.localPosition = Vector3.zero;
        //
        for (int i = 0; i < m_platform.Count; i++)
        {
            GameObject Platform = new GameObject("platform");
            Platform.layer = m_poligon.gameObject.layer;
            Platform.tag = m_poligon.gameObject.tag;
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
        m_poligon.enabled = false;
    }

    //

    public void SetInit()
    {
        m_platform = new List<ColliderPlatformDataSingle>();
        //
        for (int Group = 0; Group < m_poligon.pathCount; Group++)
        {
            //=== GET POINTS IN GROUP
            Vector2[] Point = m_poligon.GetPath(Group);
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
        this.m_platform.Add(new ColliderPlatformDataSingle(PointA, PointB));
    }

    public ColliderPlatformDataSingle[] Platform => m_platform.ToArray();
}

public class ColliderPlatformDataSingle
{
    public Vector2 PointA { private set; get; }

    public Vector2 PointB { private set; get; }

    public Vector2[] Points => new Vector2[2] { PointA, PointB };

    public Vector2 PointCentre => (PointA + PointB) / 2;

    public float Deg => Mathf.Atan2(PointB.y - PointA.y, PointB.x - PointA.x) * Mathf.Rad2Deg;

    public float Length => Vector2.Distance(PointA, PointB);

    public ColliderPlatformDataSingle(Vector2 PointA, Vector2 PointB)
    {
        this.PointA = PointA;
        this.PointB = PointB;
    }
}