using QuickMethode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlJumpY2D : MonoBehaviour
{
    #region Varible: Jump

    [Header("Jump")]
    [SerializeField] private bool m_jumpHold = true;
    [SerializeField] [Min(0)] private float m_jumpForce = 10f;
    [SerializeField] [Min(0)] private float m_jumpRatio = 1f;

    private float JumpForceCurrent => m_jumpForce * m_jumpRatio;

    //Event
    private bool m_jumpLock = false;
    private bool m_jumpUp = false;
    private bool m_jumpKeep = false;

    #endregion

    #region Varible: Hold

    [Header("Down")]
    [SerializeField] [Min(0)] private float m_downStop = 1f;
    [SerializeField] [Min(0)] private float m_downForce = 1f;

    [Header("Gravity")]
    [SerializeField] [Min(0)] private float m_gravity = 9.8f;
    [SerializeField] [Min(0)] private float m_gravityScale = 1f;

    #endregion

    #region Varible: Public

    public float JumpForce { get => m_jumpForce; set => m_jumpForce = value; }

    public float JumpRatio { get => m_jumpRatio; set => m_jumpRatio = value; }

    #endregion

    private Rigidbody2D m_rigidbody;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    #region Jump Progess

    public void SetProgessJump()
    {
        #region -------------------------------- Jump Press

        if (m_jumpLock)
        {
            //Gravity when Jump or on air!!
            float GravityForce = -Physics2D.gravity.y * m_gravityScale;
            m_rigidbody.AddForce(Vector2.down * GravityForce, ForceMode2D.Force);
        }

        if (!m_jumpLock && m_jumpUp)
        {
            //Jump Up start!!
            float ForceUp = Mathf.Sqrt(-Physics2D.gravity.y * JumpForceCurrent) * m_rigidbody.mass;
            m_rigidbody.AddForce(Vector2.up * ForceUp, ForceMode2D.Impulse);
        }

        if (!m_jumpUp)
            //Jump Up continue!!
            return;

        if (m_rigidbody.velocity.y < 0)
            //Jump Up end!!
            m_jumpUp = false;

        #endregion

        #region -------------------------------- Jump Hold

        if (!m_jumpHold)
            //Jump Hold optional!!
            return;

        if (m_rigidbody.velocity.y > 0 && !m_jumpKeep)
        {
            //Drag Down in middle!!
            float ForceDown = Mathf.Sqrt(-Physics2D.gravity.y * JumpForceCurrent * m_downStop) * m_rigidbody.mass;
            m_rigidbody.AddForce(Vector2.down * m_rigidbody.velocity.y * ForceDown, ForceMode2D.Force);
        }
        else
        if (m_rigidbody.velocity.y < 0)
        {
            //Drag Down at begin!!
            float ForceDown = Mathf.Sqrt(-Physics2D.gravity.y * JumpForceCurrent * m_downForce) * m_rigidbody.mass;
            m_rigidbody.AddForce(Vector2.down * m_rigidbody.velocity.y * ForceDown, ForceMode2D.Force);
        }

        #endregion
    } //Fixed Update!!

    public void SetEventClick()
    {
        if (m_jumpLock)
            return;
        m_jumpUp = true;
    } //Event Update!!

    public void SetEventHold()
    {
        m_jumpKeep = true;
    } //Event Update!!

    public void SetEventRelease()
    {
        m_jumpKeep = false;
    } //Event Update!!

    public void SetEventLock(bool Lock)
    {
        //Jump Lock revent from Jump press!!
        m_jumpLock = Lock;
    } //Event Update!!

    #endregion
}
