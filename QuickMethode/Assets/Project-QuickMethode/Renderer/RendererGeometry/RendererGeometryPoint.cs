using QuickMethode;
using System.Collections.Generic;
using UnityEngine;

public class RendererGeometryPoint : MonoBehaviour
{
    [SerializeField] [Min(3)] private int m_point = 3;
    [SerializeField] [Min(0)] private float m_radius = 2;
    [SerializeField] [Range(0, 360)] private float m_deg = 0;

    #region Renderer

    public List<Vector2> GetPoint()
    {
        return QGeometry.GetGeometry(m_point, m_radius, m_deg);
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere((Vector2)transform.position, m_radius);

        List<Vector2> m_PointDebug = GetPoint();

        for (int i = 1; i < m_PointDebug.Count; i++)
        {
            Gizmos.color = i % 2 == 0 ? Color.white : Color.black;
            Gizmos.DrawLine((Vector2)transform.position + m_PointDebug[i - 1], (Vector2)transform.position + m_PointDebug[i]);
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine((Vector2)transform.position + m_PointDebug[m_PointDebug.Count - 1], (Vector2)transform.position + m_PointDebug[0]);
    }
}