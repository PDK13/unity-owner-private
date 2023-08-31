using System.Collections.Generic;
using UnityEngine;

public class ColliderKeep : MonoBehaviour
{
    //Not recoment use this script, still some bug!!

    private enum KeepOption { None, Up, Down, Left, Right, }

    [SerializeField] private Transform m_baseObject;
    [SerializeField] private KeepOption m_keep = KeepOption.Down;

    [System.Serializable]
    private struct ColliderChild
    {
        public Collider2D Collider;
        public Vector2 BeginOffset;
        public Vector2 BeginSize;

        public ColliderChild(Collider2D Collider, Vector2 BeginOffset, Vector2 BeginSize)
        {
            this.Collider = Collider;
            this.BeginOffset = BeginOffset;
            this.BeginSize = BeginSize;
        }
    }

    private readonly List<ColliderChild> m_colliderList = new List<ColliderChild>();

    private void Awake()
    {
        Collider2D[] ColliderListGet = GetComponents<Collider2D>();
        foreach (Collider2D ColliderGet in ColliderListGet)
        {
            Vector2 BeginOffset = ColliderGet.offset;
            Vector2 BeginSize = ColliderGet.bounds.size;

            if (ColliderGet.GetComponent<BoxCollider2D>())
            {
                BeginSize += (Vector2.one * ColliderGet.GetComponent<BoxCollider2D>().edgeRadius);
            }

            m_colliderList.Add(new ColliderChild(ColliderGet, BeginOffset, BeginSize));
        }

        if (m_baseObject == null)
        {
            m_baseObject = transform;
        }
    }

    private void FixedUpdate()
    {
        SetCollider();
    }

    public void SetCollider()
    {
        //Set Collider stay at it true Pos, while it Scale or Size is difference at begining!!

        foreach (ColliderChild Collider in m_colliderList)
        {
            Vector2 Min = (Vector2)Collider.Collider.bounds.size - Collider.BeginSize;
            Vector2 Offset = Collider.BeginOffset;
            switch (m_keep)
            {
                case KeepOption.Up:
                    Offset.y -= Min.y / (2 * m_baseObject.localScale.y);
                    break;
                case KeepOption.Down:
                    Offset.y += Min.y / (2 * m_baseObject.localScale.y);
                    break;
                case KeepOption.Left:
                    Offset.x += Min.x / (2 * m_baseObject.localScale.x);
                    break;
                case KeepOption.Right:
                    Offset.x -= Min.x / (2 * m_baseObject.localScale.x);
                    break;
            }
            Collider.Collider.offset = Offset;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (m_colliderList == null)
        {
            return;
        }

        if (m_colliderList.Count > 0)
        {
            Vector2 Offset = m_colliderList[0].BeginOffset;
            Vector2 Size = m_colliderList[0].BeginSize;
            QGizmos.SetWireCube((Vector2)transform.position + Offset, Size, Color.green);
        }
    }
}
