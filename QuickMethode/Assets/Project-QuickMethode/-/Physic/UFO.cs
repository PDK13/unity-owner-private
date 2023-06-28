using QuickMethode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour
{
    [Header("Sector")]
    [SerializeField] private float m_radius = 5f;
    [SerializeField] private float m_degOffset = 30f;
    [SerializeField] private int m_degDir = 1;

    [Header("Cast")]
    [SerializeField] private LayerMask m_layerMask;
    [SerializeField] private List<string> m_tagCheck = new List<string>() { "Untagged", };

    private float m_degStart;
    private float m_degCurrent => this.transform.eulerAngles.z;

    private Vector2 m_posStart => this.transform.position;
    private Vector2 m_posEnd => m_posStart + (Vector2)QCircle.GetPosXY(m_degCurrent, m_radius);

    private Vector3 m_eulerEnd => Vector3.forward * (m_degStart + m_degOffset * m_degDir);

    private void Start()
    {
        m_degStart = m_degCurrent;
    }

    private void FixedUpdate()
    {
        QTransform.SetRotate3DToward(transform, m_eulerEnd, 1f);

        if (m_degDir == -1 && transform.eulerAngles.z == m_eulerEnd.z)
            m_degDir *= -1;
        else
        if (m_degDir == 1 && transform.eulerAngles.z == m_eulerEnd.z)
            m_degDir *= -1;
    }

    private void OnDrawGizmos()
    {
        QGizmos.SetLine(m_posStart, m_posEnd, Color.red);

        if (!Application.isPlaying)
        {
            QGizmos.SetLine(m_posStart, m_posStart + (Vector2)QCircle.GetPosXY(m_degCurrent + m_degOffset, m_radius), Color.green);
            QGizmos.SetLine(m_posStart, m_posStart + (Vector2)QCircle.GetPosXY(m_degCurrent - m_degOffset, m_radius), Color.green);
        }
        else
        {
            QGizmos.SetLine(m_posStart, m_posStart + (Vector2)QCircle.GetPosXY(m_degStart + m_degOffset, m_radius), Color.green);
            QGizmos.SetLine(m_posStart, m_posStart + (Vector2)QCircle.GetPosXY(m_degStart - m_degOffset, m_radius), Color.green);
        }
    }
}