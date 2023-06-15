using QuickMethode;
using UnityEngine;

public class CheckPointGet : MonoBehaviour
{
    [Header("Check Point(s)")]

    //First Check-Point head to go
    [SerializeField] private Transform m_CheckPointFirst;

    //Offset Direction in Angle (Deg)
    [SerializeField] private float m_CheckPointOffsetDirection = 90f;

    private Transform m_PointNext;

    private void Start()
    {
        m_PointNext = m_CheckPointFirst;
    }

    public void SetCheckPointNext(Transform m_PointNext)
    {
        this.m_PointNext = m_PointNext;
    }

    public float GetPointNextOffsetRotate()
    {
        return GetDegTargetXZ(transform, m_PointNext.transform);
    }

    public void SetCheckPointOffsetDirection(float m_PointOffsetDirectionDeg)
    {
        this.m_CheckPointOffsetDirection = m_PointOffsetDirectionDeg;
    }


    public float GetPointOffsetDirection()
    {
        return m_CheckPointOffsetDirection;
    }

    public bool GetPointNextRonDirection(float m_PointOffsetAngleHigher)
    {
        return GetPointNextOffsetRotate() >= m_PointOffsetAngleHigher;
    }

    public bool GetPointNextRDirection(float m_PointOffsetAngleLower)
    {
        return GetPointNextOffsetRotate() <= m_PointOffsetAngleLower;
    }

    private float GetDegTargetXZ(Transform TransformMain, Transform TransformTarket) //Check(?!)
    {
        float Distance = Vector3.Distance(TransformMain.transform.position, TransformTarket.position);
        float Deg = TransformMain.transform.eulerAngles.y;

        Vector3 DirStart = QVector.GetDir(TransformMain.transform.position, TransformMain.transform.position + QCircle.GetPosXZ(-Deg, Distance));
        Vector3 DirEnd = QVector.GetDir(TransformMain.transform.position, TransformMain.transform.position + QVector.GetDir(TransformMain.transform.position, TransformTarket.position) * Distance);

        Vector2 DirFrom = new Vector2(DirStart.x, DirStart.z);
        Vector2 DirTo = new Vector2(DirEnd.x, DirEnd.z);

        return Vector2.Angle(DirFrom, DirTo);
    } //This methode is old, should use "QCircle.GetDegTargetOffset()" instead!!
}
