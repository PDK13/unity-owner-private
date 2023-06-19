using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[RequireComponent(typeof(SpriteShapeController))]
[RequireComponent(typeof(RendererGeometryPoint))]
public class RendererGeometryShape : MonoBehaviour
{
    private SpriteShapeController m_spriteShape;

    private RendererGeometryPoint m_rendererGeometry;

    public void SetShape()
    {
        m_spriteShape = GetComponent<SpriteShapeController>();
        m_rendererGeometry = GetComponent<RendererGeometryPoint>();

        m_spriteShape.spline.Clear();

        List<Vector2> Points = m_rendererGeometry.GetPoint();

        for (int i = 0; i < Points.Count; i++)
        {
            Vector2 Pos = Points[i];
            Pos.x = Pos.x * (1);
            Points[i] = Pos;
        }

        for (int i = 0; i < Points.Count; i++)
        {
            m_spriteShape.spline.InsertPointAt(i, Points[i]);
        }
    }
}