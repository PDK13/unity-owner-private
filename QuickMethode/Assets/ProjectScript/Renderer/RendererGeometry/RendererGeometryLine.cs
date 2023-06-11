using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(RendererGeometryPoint))]

//Script dùng để điều khiển việc vẽ trên Renderer
public class RendererGeometryLine : MonoBehaviour
{
    private RendererGeometryPoint m_Point;
    private LineRenderer m_LineRenderer;

    public float m_LineWidth = 0.1f;

    private void Start()
    {
        m_Point = GetComponent<RendererGeometryPoint>();
        m_LineRenderer = GetComponent<LineRenderer>();
    }

    [System.Obsolete]
    private void FixedUpdate()
    {
        SetLineRenderer();
    }

    [System.Obsolete]
    private void SetLineRenderer()
    {
        m_LineRenderer.SetWidth(m_LineWidth, m_LineWidth);

        int m_PointCount = m_Point.GetPoint().Count;

        m_LineRenderer.positionCount = m_PointCount + 1;

        Vector2 vVector2;
        Vector3 vVector3;

        for (int i = 0; i < m_PointCount; i++)
        {
            vVector2 = m_Point.GetPoint()[i];
            vVector3 = new Vector3(vVector2.x, vVector2.y, 0) + transform.position;
            m_LineRenderer.SetPosition(i, vVector3);
        }

        vVector2 = m_Point.GetPoint()[0];
        vVector3 = new Vector3(vVector2.x, vVector2.y, 0) + transform.position;
        m_LineRenderer.SetPosition(m_PointCount, vVector3);
    }
}
