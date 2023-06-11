using QuickMethode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyControlY2D : MonoBehaviour
{
    private const float GRAVITY_FORCE = 9.8f;

    #region Varible: Jump

    [Header("Jump")]
    [SerializeField] private int m_jumpCount = 1;
    [SerializeField] private float m_jumpForce = 2.2f;
    [SerializeField] private float m_jumpRatio = 1f;

    //Current
    private bool m_downContinue = true; //Option
    private int m_jumpRemain = 0;

    //Event
    private bool m_jumpClick = false;
    private bool m_jumpKeep = false;

    #endregion

    #region Varible: Fall

    [Header("Up")]
    [SerializeField] [Min(0)] private float m_upRatio = 0.5f;
    [SerializeField] [Min(0)] private float m_upClick = 1.8f;
    [SerializeField] [Min(0)] private float m_upHold = 2.5f;

    private bool m_groundCheck = false;

    [Header("Gravity")]
    [SerializeField] private float m_gravityScale = 15f;
    [SerializeField] private Vector2 m_gravityDir = Vector2.down;

    private Vector2 GravityVelocity => m_gravityDir * m_gravityScale * GRAVITY_FORCE;

    #endregion

    #region Varible: Public

    public int JumpCount { get => m_jumpCount; set => m_jumpCount = value; }

    public float JumpForce { get => m_jumpForce; set => m_jumpForce = value; }

    public float JumpRatio { get => m_jumpRatio; set => m_jumpRatio = value; }

    #endregion

    private Rigidbody2D m_rigidbody;

    private void Start()
    {
        Application.targetFrameRate = 60;

        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            SetJumpClick();

        if (Input.GetKey(KeyCode.UpArrow))
            SetJumpHold();
        else
            SetJumpRelease();

        //if (Input.GetKeyDown(KeyCode.G))
        //{
        //    m_downContinue = !m_downContinue;

        //    if (m_downContinue)
        //        m_rigidbody.gravityScale = 0;
        //    else
        //        m_rigidbody.gravityScale = -4.5f;
        //}
    }

    private void FixedUpdate()
    {
        SetJumpPhysic();

        m_rigidbody.AddForce(GravityVelocity, ForceMode2D.Force); //Gravity always avaible!!
    }

    #region Event

    private void SetJumpClick()
    {
        if (m_jumpRemain == 0 && m_groundCheck)
            m_jumpRemain = m_jumpCount;

        if (m_jumpRemain == 0)
            return;

        m_jumpRemain--;

        m_jumpClick = true;
        if (m_jumpRemain > 0)
        {
            //Jump Extra!!
        }
    } //Called Update!

    private void SetJumpHold()
    {
        m_jumpKeep = true;
    } //Called Update!

    private void SetJumpRelease()
    {
        m_jumpKeep = false;
    } //Called Update!

    #endregion

    #region Jump Check

    private void SetJumpPhysic()
    {
        if (m_groundCheck && m_jumpClick)
        {
            m_jumpForce *= 1.0f; //?
            float ForceUp = Mathf.Sqrt(m_jumpForce * m_jumpRatio * (Physics2D.gravity.y * Mathf.Max(m_gravityScale, 18) * -2f)) * m_rigidbody.mass;
            m_rigidbody.AddForce(Vector2.up * ForceUp, ForceMode2D.Impulse);
        }

        if (!m_downContinue) 
            return;

        if (m_jumpClick)
        {
            if (m_rigidbody.velocity.y > 0 && !m_jumpKeep)
            {
                //Jump up when release before reach max highest!!
                float ForceUp = (-1f) * Mathf.Sqrt((-1f) * Physics2D.gravity.y * m_jumpForce * m_jumpRatio * m_upClick) * m_rigidbody.mass;
                Vector2 ForceVelocity = Vector2.up * m_rigidbody.velocity.y;
                m_rigidbody.AddForce(ForceVelocity * ForceUp * m_upRatio, ForceMode2D.Force);
            }
            else
            if (m_rigidbody.velocity.y < 0)
            {
                //Fall down!!
                float ForceUp = (-1f) * Mathf.Sqrt((-1f) * Physics2D.gravity.y * m_jumpForce * m_jumpRatio * m_upHold) * m_rigidbody.mass;
                Vector2 ForceVelocity = Vector2.up * m_rigidbody.velocity.y;
                m_rigidbody.AddForce(ForceVelocity * ForceUp * m_upRatio, ForceMode2D.Force);
                m_jumpClick = false;
            }
        }
    }

    #endregion

    private void OnCollisionEnter2D(Collision2D collision)
    {
        m_groundCheck = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        m_groundCheck = false;
    }
}
