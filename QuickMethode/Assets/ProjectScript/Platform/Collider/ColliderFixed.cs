using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuickMethode;

public class ColliderFixed : MonoBehaviour
{
    private enum FixedOption { Center, BorderOut, BorderIn, }

    [SerializeField] private Collider2D m_baseCollider;
    private Vector2 m_baseCenter;

    public bool BaseCollider => m_baseCollider != null;

    [Space]
    [SerializeField] private FixedOption m_fixed = FixedOption.Center;

    [Space]
    [SerializeField] private DirectionX m_fixedX = DirectionX.None;
    [SerializeField] private float m_fixedOffsetX = 0f;

    public DirectionX FixedX => m_fixedX;

    [Space]
    [SerializeField] private DirectionY m_fixedY = DirectionY.Down;
    [SerializeField] private float m_fixedOffsetY = 0f;

    public DirectionY FixedY => m_fixedY;

    private Collider2D[] m_colliderChild;
    private Vector2 m_childOffset;

    public void SetFixed()
    {
        m_colliderChild = GetComponents<Collider2D>();

        m_baseCenter = m_baseCollider.bounds.center;
        m_childOffset = m_baseCenter - (Vector2)this.transform.position;
        m_childOffset.x /= this.transform.localScale.x;
        m_childOffset.y /= this.transform.localScale.y;

        switch (m_fixedX)
        {
            case DirectionX.Left:
                m_childOffset += Vector2.left * (m_baseCollider.bounds.size.x / 2 + m_fixedOffsetX) / this.transform.localScale.x;
                break;
            case DirectionX.Right:
                m_childOffset += Vector2.right * (m_baseCollider.bounds.size.x / 2 + m_fixedOffsetX) / this.transform.localScale.x;
                break;
        }
        switch (m_fixedY)
        {
            case DirectionY.Up:
                m_childOffset += Vector2.up * (m_baseCollider.bounds.size.y / 2 + m_fixedOffsetY) / this.transform.localScale.y;
                break;
            case DirectionY.Down:
                m_childOffset += Vector2.down * (m_baseCollider.bounds.size.y / 2 + m_fixedOffsetY) / this.transform.localScale.y;
                break;
        }

        foreach (Collider2D ColliderChild in m_colliderChild)
        {
            ColliderChild.offset = m_childOffset;

            if (m_fixed == FixedOption.BorderOut)
            {
                switch (m_fixedX)
                {
                    case DirectionX.Left:
                        ColliderChild.offset += Vector2.left * (ColliderChild.bounds.size.x / 2) / this.transform.localScale.x;
                        break;
                    case DirectionX.Right:
                        ColliderChild.offset += Vector2.right * (ColliderChild.bounds.size.x / 2) / this.transform.localScale.x;
                        break;
                }
                switch (m_fixedY)
                {
                    case DirectionY.Up:
                        ColliderChild.offset += Vector2.up * (ColliderChild.bounds.size.y / 2) / this.transform.localScale.y;
                        break;
                    case DirectionY.Down:
                        ColliderChild.offset += Vector2.down * (ColliderChild.bounds.size.y / 2) / this.transform.localScale.y;
                        break;
                }
            }
            else
            if (m_fixed == FixedOption.BorderIn)
            {
                switch (m_fixedX)
                {
                    case DirectionX.Left:
                        ColliderChild.offset += Vector2.right * (ColliderChild.bounds.size.x / 2) / this.transform.localScale.x;
                        break;
                    case DirectionX.Right:
                        ColliderChild.offset += Vector2.left * (ColliderChild.bounds.size.x / 2) / this.transform.localScale.x;
                        break;
                }
                switch (m_fixedY)
                {
                    case DirectionY.Up:
                        ColliderChild.offset += Vector2.down * (ColliderChild.bounds.size.y / 2) / this.transform.localScale.y;
                        break;
                    case DirectionY.Down:
                        ColliderChild.offset += Vector2.up * (ColliderChild.bounds.size.y / 2) / this.transform.localScale.y;
                        break;
                }
            }
        }
    }
}