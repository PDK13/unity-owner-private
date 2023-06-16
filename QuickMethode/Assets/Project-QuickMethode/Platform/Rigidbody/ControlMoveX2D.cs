using QuickMethode;
using System;
using UnityEngine;

public class ControlMoveX2D : MonoBehaviour
{
    #region Varible: Move

    private DirectionX m_moveDir = DirectionX.None;

    [Header("Move")]
    [SerializeField] [Min(0)] private float m_moveForce = 10f;  //Can be change by methode!!
    [SerializeField] [Min(0)] private float m_moveRatio = 1f;   //Can be change by methode!!
    [SerializeField] [Min(0)] private float m_moveLimit = 10f;
    [SerializeField] [Min(0)] private float m_moveDecrease = 3f;
    [SerializeField] [Min(0)] private float m_moveChange = 20f;

    private Vector2 m_moveForceAdd = Vector2.zero;  //Velocity Move will be force continue still it value!!
    private bool m_moveFixed = false;               //Velocity Move will be force to Vector2.Left or Vector2.Right instead caculate!!

    [Header("Wall")]
    [SerializeField] [Min(0)] private float m_wallClimb = 0.1f;
    [SerializeField] [Min(0)] private float m_wallLength = 0.01f;
    [SerializeField] private float m_wallOffset = 0f;

    private bool m_wallCheckAhead;
    private bool m_wallCheckPush;   //Push will base on that object rigidbody (Gravity, Mass, etc...)!!
    private bool m_wallCheckClimb;  //Climb will continue avaible when Bottom still Check ground!!

    private Vector2 PosRayClimb => m_moveDir == DirectionX.Left ? PosRayLB : m_moveDir == DirectionX.Right ? PosRayRB : QCollider2D.GetBorderPos(m_colliderBase, Direction.Down);

    #endregion

    #region Varible: Surface

    [Header("Surface")]
    [SerializeField] [Min(0)] private float m_surfaceLimit = 70f;       //Deg!!
    [SerializeField] [Min(0)] private float m_surfaceLength = 0.05f;
    [SerializeField] private float m_surfaceOffset = 0f;

    private float m_degL;
    private float m_degR;

    private Vector2 PosRayL => QCollider2D.GetBorderPos(m_colliderBase, Direction.Down, Direction.Left) + Vector2.right * m_surfaceOffset;
    private Vector2 PosRayLB => PosRayL + Vector2.up * 0.001f;
    private Vector2 PosRayLT => PosRayL + Vector2.up * 0.0011f;

    private Vector2 PosRayR => QCollider2D.GetBorderPos(m_colliderBase, Direction.Down, Direction.Right) + Vector2.left * m_surfaceOffset;
    private Vector2 PosRayRB => PosRayR + Vector2.up * 0.001f;
    private Vector2 PosRayRT => PosRayR + Vector2.up * 0.0011f;

    private Vector2 DirMoveL
    {
        get
        {
            if (MoveFixed)
                return Vector2.left;

            if (m_degL >= 90f)
            {
                if (m_wallClimb > 0 && !m_wallCheckPush)
                    return Vector2.up;

                if (m_wallCheckPush)
                    return Vector2.left;

                return Vector2.zero;
            }

            if (m_surfaceLimit > 0)
            {
                if (m_degL != 0 && QCircle.GetDegOppositeUD(m_degL) <= m_surfaceLimit)
                    return QCircle.GetPosXY(m_degL, 1f);

                if (m_degR != 0 && m_degR <= m_surfaceLimit)
                    return QCircle.GetPosXY(m_degR, 1f) * -1f;
            }
            else
            if (m_degL != 0)
                return QCircle.GetPosXY(m_degL, 1f);

            return Vector2.left;
        }
    } //Get value after Surface and Wall Check!!
    private Vector2 DirMoveR
    {
        get
        {
            if (MoveFixed)
                return Vector2.right;

            if (m_degR >= 90f)
            {
                if (m_wallClimb > 0 && !m_wallCheckPush)
                    return Vector2.up;

                if (m_wallCheckPush)
                    return Vector2.right;

                return Vector2.zero;
            }

            if (m_surfaceLimit > 0)
            {
                if (m_degR != 0 && m_degR <= m_surfaceLimit)
                    return QCircle.GetPosXY(m_degR, 1f);

                if (m_degL != 0 && QCircle.GetDegOppositeUD(m_degL) <= m_surfaceLimit)
                    return QCircle.GetPosXY(m_degL, 1f) * -1f;
            }
            else
            if (m_degR != 0)
                return QCircle.GetPosXY(m_degR, 1f);

            return Vector2.right;
        }
    } //Get value after Surface and Wall Check!!

    #endregion

    #region Varible: Body

    [Header("Body")]
    [SerializeField] private bool m_checkAlways = true;         //Raycast will check over Time.FixedDeltaTime?!
    [SerializeField] private Collider2D m_colliderBase;         //Required Collider!!
    [SerializeField] private PhysicsMaterial2D m_materialMove;  //Fiction should be 0 value!!
    [SerializeField] private PhysicsMaterial2D m_materialStand; //Fiction should be more than 0 value!!
    [SerializeField] private LayerMask m_checkMask;             //Layer Mask for every raycast!!

    #endregion

    #region Varible: Public

    public DirectionX MoveDir { get => m_moveDir; set => m_moveDir = value; }

    public float MoveForce { get => m_moveForce; set => m_moveForce = value; }

    public float MoveRatio { get => m_moveRatio; set => m_moveRatio = value; }

    public Vector2 MoveForceAdd { get => m_moveForceAdd; set => m_moveForceAdd = value; }

    public bool MoveFixed { get => m_moveFixed; set => m_moveFixed = value; }

    public float DegL => m_degL;

    public float DegR => m_degR;

    public bool WallCheckAhead => m_wallCheckAhead;

    public bool WallCheckClimb => m_wallCheckClimb;

    public bool WallCheckPush => m_wallCheckPush;

    #endregion

    private Rigidbody2D m_rigidbody;
    
    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    #region Move Progess

    public void SetProgessMove()
    {
        if (m_colliderBase == null)
            return;

        //Physic Material!!

        if (m_moveDir != DirectionX.None)
            m_rigidbody.sharedMaterial = m_materialMove;
        else
            m_rigidbody.sharedMaterial = m_materialStand;

        //Physic Cacualte!!

        Vector2 VelocityForce = Vector2.zero;
        m_rigidbody.velocity = new Vector2(0f, m_rigidbody.velocity.y); //Reset Velocity X!!

        if (m_checkAlways)
        {
            SetCheckSurface();
            SetCheckWall(m_moveDir);
        }

        if (m_rigidbody.velocity.x > m_moveLimit || m_rigidbody.velocity.x < -m_moveLimit)
            m_rigidbody.AddForce(Vector3.left * (m_rigidbody.velocity.x / Mathf.Abs(m_rigidbody.velocity.x)) * m_moveDecrease, ForceMode2D.Impulse);
        else
        {
            if (m_moveChange > 0)
            {
                if (m_moveDir == DirectionX.Right && m_rigidbody.velocity.x < 0)
                    m_rigidbody.AddForce(Vector3.left * (m_rigidbody.velocity.x / Mathf.Abs(m_rigidbody.velocity.x)) * m_moveChange, ForceMode2D.Impulse);
                else
                if (m_moveDir == DirectionX.Left && m_rigidbody.velocity.x > 0)
                    m_rigidbody.AddForce(Vector3.left * (m_rigidbody.velocity.x / Mathf.Abs(m_rigidbody.velocity.x)) * m_moveChange, ForceMode2D.Impulse);
            }

            if (m_moveDir != DirectionX.None)
            {
                if (!m_checkAlways)
                {
                    SetCheckSurface();
                    SetCheckWall(m_moveDir);
                }

                if (m_moveDir == DirectionX.Right)
                {
                    float MoveForce = (m_degR != 90f || m_wallCheckClimb || m_wallCheckPush? m_moveForce : 0f) * m_moveRatio;
                    if (m_rigidbody.velocity.x < m_moveLimit)
                        VelocityForce += DirMoveR * MoveForce;
                }
                else
                if (m_moveDir == DirectionX.Left)
                {
                    float MoveForce = (m_degL != 90f || m_wallCheckClimb || m_wallCheckPush ? m_moveForce : 0f) * m_moveRatio;
                    if (m_rigidbody.velocity.x > -m_moveLimit)
                        VelocityForce += DirMoveL * MoveForce;
                }
            }
        }

        m_rigidbody.AddForce((VelocityForce + m_moveForceAdd) * 4f, ForceMode2D.Impulse); //Move Force Finally!!
    } //Fixed Update!!

    public void SetEventMove(DirectionX Move)
    {
        m_moveDir = Move;
    } //Event Update!!

    #endregion

    #region Surface Check: Always check ray first before force move!!

    private void SetCheckSurface()
    {
        m_degL = GetCheckSurfaceL();
        m_degR = GetCheckSurfaceR();
    } //Fixed Update!!

    private float GetCheckSurfaceL()
    {
        var RayHitResultB = QCast.GetRaycast2DDir(PosRayLB, Vector2.left, m_surfaceLength, m_checkMask);
        var RayHitResultT = QCast.GetRaycast2DDir(PosRayLT, Vector2.left, m_surfaceLength, m_checkMask);

        if (!RayHitResultB.HasValue || !RayHitResultT.HasValue)
            //Surely not Wall, continue Move Normally!!
            return 0;

        float Deg = QCircle.GetDeg(RayHitResultB.Value.Point, RayHitResultT.Value.Point);

        return Deg;
    }

    private float GetCheckSurfaceR()
    {
        var RayHitResultB = QCast.GetRaycast2DDir(PosRayRB, Vector2.right, m_surfaceLength, m_checkMask);
        var RayHitResultT = QCast.GetRaycast2DDir(PosRayRT, Vector2.right, m_surfaceLength, m_checkMask);

        if (!RayHitResultB.HasValue || !RayHitResultT.HasValue)
            //Surely not Wall, continue Move Normally!!
            return 0;

        float Deg = QCircle.GetDeg(RayHitResultB.Value.Point, RayHitResultT.Value.Point);

        return Deg;
    }

    #endregion

    #region Wall Check: Always check ray first before force move!!

    private void SetCheckWall(DirectionX Move)
    {
        GetCheckWallAhead(Move);
        m_wallCheckClimb = GetCheckWallClimb(Move);
    } //Fixed Update!!

    private void GetCheckWallAhead(DirectionX Move)
    {
        if (Move == DirectionX.None)
        {
            m_wallCheckAhead = false;
            m_wallCheckPush = false;
            return;
        }

        Vector2 PosRayAhead = QCollider2D.GetBorderPos(m_colliderBase, Move) + Vector2.up * m_wallOffset;
        var RayHitSideResult = QCast.GetRaycast2DDir(PosRayAhead, Vector2.right * (int)Move, m_wallLength, m_checkMask);

        m_wallCheckAhead = RayHitSideResult.HasValue; //Wall Check!!

        if (RayHitSideResult.HasValue) //Wall push check!!
        {
            Rigidbody2D TargetBody = RayHitSideResult.Value.Target.GetComponent<Rigidbody2D>();
            if (TargetBody != null)
                m_wallCheckPush = TargetBody.bodyType == RigidbodyType2D.Dynamic; //Wall can be push (Box)!!
            else
                m_wallCheckPush = false; //Wall can't be push!!
        }
        else
            m_wallCheckPush = false; //No wall to push!!
    }

    private bool GetCheckWallClimb(DirectionX Move)
    {
        if (m_wallClimb == 0)
            return false;

        if (Move == DirectionX.None)
            return false;

        if (m_wallCheckAhead)
            //Surely Wall, not continue Climb Up!!
            return false;

        var RayHitResult = QCast.GetRaycast2DDir(PosRayClimb, Vector2.down, m_wallClimb, m_checkMask);

        if (!RayHitResult.HasValue)
            return false;

        return true;
    }

    #endregion

    private void OnDrawGizmosSelected()
    {
        if (m_colliderBase == null)
            return;

        Vector2 Centre = m_rigidbody != null ? m_rigidbody.worldCenterOfMass : this.transform.position;

        QGizmos.SetWireSphere(Centre, 0.1f, Color.white);

        //Direction Move!!
        QGizmos.SetLine(Centre, Centre + DirMoveL * 0.2f, Color.white);
        QGizmos.SetLine(Centre, Centre + DirMoveR * 0.2f, Color.white);

        //Surface Check!!
        QGizmos.SetRay(PosRayLB, Vector2.left, m_surfaceLength, Color.red);
        QGizmos.SetRay(PosRayRB, Vector2.right, m_surfaceLength, Color.red);

        //Bottom Check!!
        QGizmos.SetRay(PosRayClimb, Vector2.down, m_wallClimb, Color.blue);

        //Ahead Check!!
        if (Application.isPlaying)
        {
            Vector2 PosRayAhead = QCollider2D.GetBorderPos(m_colliderBase, m_moveDir) + Vector2.up * m_wallOffset;
            QGizmos.SetRay(PosRayAhead, Vector2.right * (int)m_moveDir, m_wallLength, Color.yellow);
        }
        else
        {
            Vector2 PosRayAhead = QCollider2D.GetBorderPos(m_colliderBase, DirectionX.Right) + Vector2.up * m_wallOffset;
            QGizmos.SetRay(PosRayAhead, Vector2.right * (int)1, m_wallLength, Color.yellow);
        }
    }
}