using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;

[ExecuteAlways]
public class ShapePlatformCreator : MonoBehaviour
{
    [SerializeField] private SpriteShapeController m_spriteShape;
    [SerializeField] private CompositeCollider2D m_compositeCollider;

    private PlatformStruct m_platformStruct;

    private void Awake()
    {
        
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        //if (m_platformStruct == null && m_compositeCollider != null)
        //    m_platformStruct = new PlatformStruct(m_compositeCollider);
        ////
        //if (m_platformStruct != null)
        //{
        //    m_platformStruct.SetGenerate();
        //    //
        //    for (int i = 0; i < m_platformStruct.Platforms.Count; i++)
        //    {
        //        Debug.DrawLine(
        //            this.transform.position + (Vector3)m_compositeCollider.offset + m_platformStruct.Platforms[i].PointA,
        //            this.transform.position + (Vector3)m_compositeCollider.offset + m_platformStruct.Platforms[i].PointB,
        //            Color.red);
        //    }
        //}
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //
        if (m_platformStruct == null && m_compositeCollider != null)
            m_platformStruct = new PlatformStruct(m_compositeCollider);
        //
        if (m_platformStruct != null)
        {
            m_platformStruct.SetGenerate();
            //
            for (int i = 0; i < m_platformStruct.Platforms.Count; i++)
            {
                Debug.DrawLine(
                    this.transform.position + (Vector3)m_compositeCollider.offset + m_platformStruct.Platforms[i].PointA,
                    this.transform.position + (Vector3)m_compositeCollider.offset + m_platformStruct.Platforms[i].PointB,
                    Color.red);
            }
        }
    }
}

public class PlatformStruct
{
    public List<PlatformSingle> Platforms = new List<PlatformSingle>();

    private CompositeCollider2D m_compositeCollider;

    public PlatformStruct(CompositeCollider2D SpriteShape)
    {
        m_compositeCollider = SpriteShape;
    }

    public void SetGenerate()
    {
        Platforms = new List<PlatformSingle>();
        //
        List<List<Vector2>> Points = GetPointsBorderPos();
        for (int Group = 0; Group < Points.Count; Group++)
        {
            //Find a highest Point of this Group!!
            int IndexStart = 0;
            float HighStart = 0;
            for (int Index = 0; Index < Points[Group].Count; Index++)
            {
                if (Points[Group][Index].y <= HighStart)
                    continue;
                //
                IndexStart = Index;
                HighStart = Points[Group][Index].y;
            }
            //Check where can Stand!!
            //Check m_spriteShape Left to Right mean Stand, else not Stand!!
            int IndexNext = IndexStart;
            int IndexPrev = IndexNext - 1 >= 0 ? IndexNext - 1 : Points[Group].Count - 1;
            do
            {
                //Index Run!!
                IndexNext++;
                IndexPrev++;
                //
                if (IndexNext >= Points[Group].Count)
                    IndexNext = 0;
                //
                if (IndexPrev >= Points[Group].Count)
                    IndexPrev = 0;
                //
                //Check Start - End Index?!
                //
                if (IndexNext == IndexStart)
                    break;
                //
                //Check at Index!!
                //
                //===== This code is for tile!! =====
                //
                //if (Points[Group][IndexPrev].y != Points[Group][IndexNext].y)
                //    continue;
                ////
                //if (Points[Group][IndexPrev].x <= Points[Group][IndexNext].x)
                //    continue;
                //
                //===== This code is for tile!! =====
                //
                Vector2 PointA = Points[Group][IndexPrev];
                Vector2 PointB = Points[Group][IndexNext];
                Platforms.Add(new PlatformSingle(PointA, PointB));
            }
            while (IndexNext != IndexStart);
            //Done Check current Group!!
        }
    }

    public List<List<Vector2>> GetPointsBorderPos(bool Square = true)
    {
        //NOTE: Composite Collider split Points into Group if they're not contact with each other!!
        List<List<Vector2>> PointsBorder = new List<List<Vector2>>();
        for (int Group = 0; Group < m_compositeCollider.pathCount; Group++)
        {
            //Generate a new Group!!
            PointsBorder.Add(new List<Vector2>());
            //Get Points of this Group!!
            Vector2[] Points = new Vector2[m_compositeCollider.GetPathPointCount(Group)];
            m_compositeCollider.GetPath(Group, Points);
            for (int Index = 0; Index < Points.Length; Index++)
            {
                //Generate new Points into each Group!!
                if (Square)
                {
                    Vector2 Pos = new Vector2(Points[Index].x, Points[Index].y);
                    if (Points.Contains(Pos))
                        continue;
                    //
                    PointsBorder[Group].Add(Pos);
                }
                else
                    PointsBorder[Group].Add(Points[Index]);
            }
            //Done Generate current Group!!
        }

        //Result local pos Points of Collider!!
        return PointsBorder;
    }
}

public class PlatformSingle
{
    public Vector3 PointA;
    public Vector3 PointB;
    public float Deg;
    public float Length;

    public PlatformSingle(Vector3 PointA, Vector3 PointB)
    {
        this.PointA = PointA;
        this.PointB = PointB;
        //

    }
}