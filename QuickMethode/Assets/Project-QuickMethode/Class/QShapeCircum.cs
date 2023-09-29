using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class QShapeCircum
{
    #region Local

    private float m_radius = 0f;

    private bool m_hollow  = false;
    private float m_radiusHollow = 0f;
    private int m_cutHollow = 1;

    private Vector3[] m_points;

    Spline m_spline;

    public QShapeCircum(SpriteShapeController SpriteShapeController)
    {
        m_spline = SpriteShapeController.spline;
    }

    #endregion
}