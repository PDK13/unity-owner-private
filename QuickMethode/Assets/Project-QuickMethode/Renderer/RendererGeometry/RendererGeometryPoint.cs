using System.Collections.Generic;
using UnityEngine;

public class RendererGeometryPoint : MonoBehaviour
{
    public bool m_debug = true;

    public float m_radius = 2;

    [Range(0, 360)]
    public float m_deg = 0;

    [Min(3)]
    public int m_point = 3;

    #region Renderer

    public List<Vector2> GetPoint(float Length, float Deg, int Point)
    {
        List<Vector2> Points = new List<Vector2>();

        float RadSpace = (360 / Point) * (Mathf.PI / 180);
        float RadStart = (Deg) * (Mathf.PI / 180);
        float RadCur = RadStart;

        Vector2 PointStart = new Vector2(Mathf.Cos(RadStart) * Length, Mathf.Sin(RadStart) * Length);
        Vector2 PointOld = PointStart;

        Points.Add(PointStart);

        for (int i = 1; i < Point; i++)
        {
            RadCur += RadSpace;
            Vector2 v_NewPoint = new Vector2(Mathf.Cos(RadCur) * Length, Mathf.Sin(RadCur) * Length);

            Points.Add(v_NewPoint);

            PointOld = v_NewPoint;
        }

        return Points;
    }

    public List<Vector2> GetPoint()
    {
        return GetPoint(m_radius, m_deg, m_point);
    }

    #endregion

    private void OnDrawGizmos()
    {
        //Thể hiện hình vẽ mẫu trên chính GameObject. Và đây cũng là gợi ý cho cách sử dụng danh sách điểm.
        //Đường vẽ màu "Vàng" là giữa điểm Kết thúc và Bắt đầu

        if (!m_debug) return;

        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere((Vector2)transform.position, m_radius);

        //Sử dụng danh sách điểm từ hàm phía dưới
        List<Vector2> m_PointDebug = GetPoint(m_radius, m_deg, m_point);

        for (int i = 1; i < m_PointDebug.Count; i++)
        {
            if (i % 2 == 0)
            {
                Gizmos.color = Color.white;
            }
            else
            {
                Gizmos.color = Color.black;
            }

            Gizmos.DrawLine(
                (Vector2)transform.position + m_PointDebug[i - 1],
                (Vector2)transform.position + m_PointDebug[i]);
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(
            (Vector2)transform.position + m_PointDebug[m_PointDebug.Count - 1],
            (Vector2)transform.position + m_PointDebug[0]);
    }
}