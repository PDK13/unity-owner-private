using QuickMethode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScan : MonoBehaviour
{
    [SerializeField] private RotateLimit m_rotateLimit; //Might use this component with "RotateLimit" component on same GameObject!!

    [Space]
    [SerializeField] [Min(0)] private float m_degSpeed = 1f;

    [Space]
    [SerializeField] [Min(0)] private float m_checkLength = 5f;
    [SerializeField] [Min(0)] private float m_checkRadius = 1f;
    [SerializeField] private LayerMask m_checkMask;

    [Space]
    [SerializeField] private bool m_targetCheck = true;
    [SerializeField] private GameObject m_targetLock;

    private int m_degDir = 1; //Scan!!

    public float DegSpeed { get => m_degSpeed; set => m_degSpeed = value; }

    public float CheckLength
    { 
        get => m_checkLength != 0 ? m_checkLength : Mathf.Infinity; 
        set => m_checkLength = value != 0 ? value : Mathf.Infinity; 
    }
    public float CheckRadius
    { 
        get => m_checkRadius; 
        set => m_checkRadius = value; 
    }
    public LayerMask CheckMask { get => m_checkMask; set => m_checkMask = value; }

    private Vector3 DirForward => QCircle.GetPosXY(m_rotateLimit.DegForward, 1f).normalized;
    private Vector3 DirCurrent => QCircle.GetPosXY(m_rotateLimit.DegCurrent, 1f).normalized;
    private Vector3 DirTarget => m_targetLock != null ? (m_targetLock.transform.position - transform.position).normalized : Vector3.zero;

    private Vector3 EulerCurrent => Vector3.forward * m_rotateLimit.DegCurrent;
    private Vector3 EulerToward => Vector3.forward * (m_degDir == 1 ? m_rotateLimit.DegLimitA : m_rotateLimit.DegLimitB); //Scan!!
    private Vector3 EulerTarget
    {
        get
        {
            float DegTaget = Vector3.SignedAngle(DirForward, DirTarget, Vector3.forward);
            return Vector3.forward * (DegTaget + m_rotateLimit.DegForward);
        }
    }

    public bool TargetCheck { get => m_targetCheck; set => m_targetCheck = value; }
    public GameObject TargetLock { get => m_targetLock; set => m_targetLock = value; }

    private void FixedUpdate()
    {
        if (m_rotateLimit == null)
            return;

        if (m_targetLock == null)
            SetScan();
        else
            SetFollow();
    }

    private void SetScan()
    {
        m_rotateLimit.SetDeg(Vector3.MoveTowards(EulerCurrent, EulerToward, m_degSpeed).z);

        SetCast();

        if (m_rotateLimit.DegLimitReach)
            m_degDir *= -1;
    }

    private void SetCast()
    {
        if (!m_targetCheck)
            return;

        var Cast = QCast.GetCircleCast2DDir(transform.position, DirCurrent, m_checkRadius, m_checkLength, m_checkMask);
        if (Cast.HasValue)
            m_targetLock = Cast.Value.Target;
    }

    private void SetFollow()
    {
        m_rotateLimit.SetDeg(Vector3.MoveTowards(EulerCurrent, EulerTarget, m_degSpeed).z);
    }

    private void OnDrawGizmos()
    {
        if (m_rotateLimit == null)
            return;

        m_rotateLimit.SetGizmos(CheckLength, m_targetLock != null ? Color.red : Color.gray);

        if (Application.isPlaying)
            QGizmos.SetSpherecastDir(transform.position, DirCurrent, m_checkRadius, m_checkLength, m_targetLock != null ? Color.red : Color.gray);
        else
            QGizmos.SetSpherecastDir(transform.position, DirForward, m_checkRadius, m_checkLength, Color.white);
    }
}