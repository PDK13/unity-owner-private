using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColliderPlatformTile : MonoBehaviour
{
    [SerializeField] private LayerMask m_colliderMask;

    private CompositeCollider2D m_composite;
    private ColliderPlatformTileData m_data;

    private void Awake()
    {
        m_composite = GetComponent<CompositeCollider2D>();
    }

    protected void Start()
    {
        SetInit();
    }

    #region ======================================= Platform

    [ContextMenu("Init")]
    private void SetInit()
    {
        m_composite = GetComponent<CompositeCollider2D>();
        m_data = new ColliderPlatformTileData(m_composite);
        m_data.SetInit();
        m_data.SetGenerate(m_colliderMask);
    }

    #endregion

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            if (m_composite == null)
                m_composite = GetComponent<CompositeCollider2D>();
            //
            ColliderPlatformTileData Data = new ColliderPlatformTileData(m_composite);
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

public class ColliderPlatformTileData
{
    private CompositeCollider2D m_composite;
    private List<ColliderPlatformDataSingle> m_platform = new List<ColliderPlatformDataSingle>();

    public ColliderPlatformTileData(CompositeCollider2D m_composite)
    {
        this.m_composite = m_composite;
    }

    //

    public void SetGenerate()
    {
        if (!Application.isPlaying)
            return;
        //
        GameObject Group = new GameObject("platform-group");
        Transform GroupTransform = Group.transform;
        GroupTransform.SetParent(m_composite.transform);
        GroupTransform.localPosition = Vector3.zero;
        //
        for (int i = 0; i < m_platform.Count; i++)
        {
            GameObject Platform = new GameObject("platform");
            Platform.layer = m_composite.gameObject.layer;
            Platform.tag = m_composite.gameObject.tag;
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
        m_composite.isTrigger = true;
        m_composite.enabled = false;
        m_composite.gameObject.tag = "Untagged";
        m_composite.gameObject.layer = LayerMask.NameToLayer("Default");
    }

    public void SetGenerate(LayerMask ColliderMask)
    {
        if (!Application.isPlaying)
            return;
        //
        GameObject Group = new GameObject("platform-group");
        Transform GroupTransform = Group.transform;
        GroupTransform.SetParent(m_composite.transform);
        GroupTransform.localPosition = Vector3.zero;
        //
        for (int i = 0; i < m_platform.Count; i++)
        {
            GameObject Platform = new GameObject("platform");
            Platform.layer = m_composite.gameObject.layer;
            Platform.tag = m_composite.gameObject.tag;
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
        m_composite.isTrigger = true;
        m_composite.enabled = false;
        m_composite.gameObject.tag = "Untagged";
        m_composite.gameObject.layer = LayerMask.NameToLayer("Default");
    }

    //

    public void SetInit()
    {
        m_platform = new List<ColliderPlatformDataSingle>();
        //
        var PlatformGroup = GetPlatform(m_composite);
        //
        for (int i = 0; i < PlatformGroup.Count; i++)
        {
            Vector2 PointA = (Vector2)m_composite.transform.position + PlatformGroup[i].Center + Vector2.left * (PlatformGroup[i].Length / 2);
            Vector2 PointB = (Vector2)m_composite.transform.position + PlatformGroup[i].Center + Vector2.right * (PlatformGroup[i].Length / 2);
            SetInit(PointA, PointB);
        }
    }

    private void SetInit(Vector2 PointA, Vector2 PointB)
    {
        this.m_platform.Add(new ColliderPlatformDataSingle(PointA, PointB));
    }

    public ColliderPlatformDataSingle[] Platform => m_platform.ToArray();

    //

    public List<(Vector2 Center, float Length)> GetPlatform(CompositeCollider2D From)
    {
        //NOTE: Caculate Platform Pos and Length of Collider on each Group!!
        List<(Vector2 Center, float Length)> Platform = new List<(Vector2 Center, float Length)>();

        List<List<Vector2>> Points = GetPointsBorderPos(From);
        for (int Group = 0; Group < Points.Count; Group++)
        {
            //Find a highest Point of this Group!!
            int IndexStart = 0;
            float HighStart = 0;
            for (int Index = 0; Index < Points[Group].Count; Index++)
            {
                if (Points[Group][Index].y <= HighStart)
                    continue;
                IndexStart = Index;
                HighStart = Points[Group][Index].y;
            }
            //Check where can Stand!!
            //Check from Left to Right mean Stand, else not Stand!!
            int IndexNext = IndexStart;
            int IndexPrev = IndexNext - 1 >= 0 ? IndexNext - 1 : Points[Group].Count - 1;
            do
            {
                //Index Run!!
                IndexNext++;
                IndexPrev++;
                if (IndexNext >= Points[Group].Count)
                    IndexNext = 0;
                if (IndexPrev >= Points[Group].Count)
                    IndexPrev = 0;
                //Check Start - End Index?!
                if (IndexNext == IndexStart)
                    break;
                //Check at Index!!
                if (Points[Group][IndexPrev].y != Points[Group][IndexNext].y)
                    continue;
                if (Points[Group][IndexPrev].x <= Points[Group][IndexNext].x)
                    continue;
                Vector2 PointA = Points[Group][IndexPrev];
                Vector2 PointB = Points[Group][IndexNext];
                Vector2 Center = m_composite.transform.InverseTransformPoint(QVector.GetMiddlePoint(PointA, PointB));
                float Length = Mathf.Abs(PointB.x - PointA.x);
                Platform.Add((Center, Length));
            }
            while (IndexNext != IndexStart);
            //Done Check current Group!!
        }

        //Result local Center of Platform!!
        return Platform;
    }

    private List<List<Vector2>> GetPointsBorderPos(CompositeCollider2D From, bool Square = true)
    {
        //NOTE: Composite Collider split Points into Group if they're not contact with each other!!
        List<List<Vector2>> PointsBorder = new List<List<Vector2>>();
        for (int Group = 0; Group < From.pathCount; Group++)
        {
            //Generate a new Group!!
            PointsBorder.Add(new List<Vector2>());
            //Get Points of this Group!!
            Vector2[] Points = new Vector2[From.GetPathPointCount(Group)];
            From.GetPath(Group, Points);
            for (int Index = 0; Index < Points.Length; Index++)
            {
                //Generate new Points into each Group!!
                if (Square)
                {
                    Vector2Int Pos = new Vector2Int(Mathf.RoundToInt(Points[Index].x), Mathf.RoundToInt(Points[Index].y));
                    if (Points.Contains(Pos))
                        continue;
                    PointsBorder[Group].Add(Pos);
                }
                else
                {
                    PointsBorder[Group].Add(Points[Index]);
                }
            }
            //Done Generate current Group!!
        }
        //Result local pos Points of Collider!!
        return PointsBorder;
    }
}