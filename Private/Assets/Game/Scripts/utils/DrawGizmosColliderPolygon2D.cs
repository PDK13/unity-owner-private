using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGizmosColliderPolygon2D : MonoBehaviour
{
    [SerializeField] PolygonCollider2D m_collider;
    [SerializeField] bool m_enabled;

    void OnDrawGizmos()
    {
        if (!m_enabled)
            return;
        Gizmos.color = Color.cyan;
        //transform.position = m_collider.bounds.size;
        for (int i = 0; i < m_collider.pathCount; i++)
        {
            for (int j = 0; j < m_collider.GetPath(i).Length; j++)
            {
                Vector2 point1 = m_collider.GetPath(i)[j];
                if (j + 1 < m_collider.GetPath(i).Length)
                {
                    Vector2 point2 = m_collider.GetPath(i)[j + 1];
                    Gizmos.DrawLine(point1 + (Vector2)m_collider.transform.position, point2 + (Vector2)m_collider.transform.position);
                }
                else
                {
                    Gizmos.DrawLine(point1 + (Vector2)m_collider.transform.position, m_collider.GetPath(i)[0] + (Vector2)m_collider.transform.position);
                }
            }
        }

        /*Gizmos.color = Color.yellow;

        Vector3 size = transform.lossyScale;
        size.x *= m_collider.bounds.size.x;
        size.y *= m_collider.bounds.size.y;
        size.z *= m_collider.bounds.size.z;
        size = transform.rotation * size;

        Gizmos.DrawWireCube(transform.position + Vector3.up/2, size);*/
    }
}
