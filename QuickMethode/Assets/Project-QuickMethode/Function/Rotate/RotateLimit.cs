using UnityEngine;

public class RotateLimit : MonoBehaviour
{
    [SerializeField] private float m_degForward = 0;
    [SerializeField] [Range(0f, 360f)] private float m_degWidth = 60f;

    private float m_degCurrent;

    public float DegForward { get => m_degForward; set => m_degForward = value; }
    public float DegWidth { get => m_degWidth; set => m_degWidth = value < 0 ? 0 : value > 360 ? 360 : value; }
    public float DegCurrent { get => m_degCurrent; set => m_degCurrent = value; }
    public float DegLimitA => m_degForward + m_degWidth / 2;
    public float DegLimitB => m_degForward - m_degWidth / 2;
    public bool DegLimitReach => DegCurrent == DegLimitA || DegCurrent == DegLimitB;

    private void Awake()
    {
        SetDegReset();
    }

    public void SetDegReset()
    {
        SetDeg(DegForward);
    }

    public void SetDeg(float Deg360)
    {
        if (Deg360 > DegLimitA)
        {
            m_degCurrent = DegLimitA;
        }
        else
        if (Deg360 < DegLimitB)
        {
            m_degCurrent = DegLimitB;
        }
        else
        {
            m_degCurrent = Deg360;
        }

        transform.localEulerAngles = Vector3.forward * DegCurrent;
    }

    public void SetDegAdd(float Add)
    {
        if (DegCurrent + Add > DegLimitA || DegCurrent + Add < DegLimitB)
        {
            return;
        }

        m_degCurrent += Add;

        transform.localEulerAngles = Vector3.forward * DegCurrent;
    }

    public void SetGizmos(float CentreLength, Color CentreColor)
    {
        if (Application.isPlaying)
        {
            QGizmos.SetLine(transform.position, transform.position + QCircle.GetPosXY(DegCurrent, CentreLength), CentreColor, 0.1f);
        }

        QGizmos.SetLine(transform.position, transform.position + QCircle.GetPosXY(DegForward, CentreLength), Color.gray, 0.1f);
        QGizmos.SetLine(transform.position, transform.position + QCircle.GetPosXY(DegLimitA, CentreLength), Color.gray);
        QGizmos.SetLine(transform.position, transform.position + QCircle.GetPosXY(DegLimitB, CentreLength), Color.gray);
    }
}