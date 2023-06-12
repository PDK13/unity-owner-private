using QuickMethode;
using UnityEngine;

public class SimplePlayer : MonoBehaviour
{
    private ControlMoveX2D m_bodyControlX;
    private ControlJumpY2D m_bodyControlY;

    //Debug
    private float m_posYStart;
    private float m_posYEnd;
    private float m_posYHighest;
    private float m_posYLast;
    //Debug

    private void Start()
    {
        m_bodyControlX = GetComponent<ControlMoveX2D>();
        m_bodyControlY = GetComponent<ControlJumpY2D>();
    }

    private void Update()
    {
        SetDebug();

        if (Input.GetKey(KeyCode.RightArrow))
            m_bodyControlX.SetEventMove(DirectionX.Right);
        else
        if (Input.GetKey(KeyCode.LeftArrow))
            m_bodyControlX.SetEventMove(DirectionX.Left);
        else
            m_bodyControlX.SetEventMove(DirectionX.None);

        if (Input.GetKeyDown(KeyCode.UpArrow))
            m_bodyControlY.SetEventClick();

        if (Input.GetKey(KeyCode.UpArrow))
            m_bodyControlY.SetEventHold();
        else
            m_bodyControlY.SetEventRelease();
    }

    private void FixedUpdate()
    {
        m_bodyControlX.SetProgessMove();
        m_bodyControlY.SetProgessJump();
    }

    private void SetDebug()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_posYStart = this.transform.position.y;
            m_posYHighest = this.transform.position.y;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            m_posYEnd = this.transform.position.y;
        }

        if (m_posYLast != this.transform.position.y)
            m_posYLast = this.transform.position.y;

        if (m_posYEnd < m_posYLast)
            m_posYEnd = m_posYLast;

        if (m_posYHighest < m_posYLast)
            m_posYHighest = m_posYLast;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        m_bodyControlY.SetEventLock(false);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        m_bodyControlY.SetEventLock(true);
    }

    private void OnDrawGizmos()
    {
        Vector3 PosStart = new Vector3(this.transform.position.x, m_posYStart, 0);
        Vector3 PosEnd = new Vector3(this.transform.position.x, m_posYEnd, 0);
        Vector3 PosHighest = new Vector3(this.transform.position.x, m_posYHighest, 0);
        QGizmos.SetLine(PosStart + Vector3.left, PosStart + Vector3.right, Color.green);
        QGizmos.SetLine(PosEnd + Vector3.left, PosEnd + Vector3.right, Color.green);
        QGizmos.SetLine(PosHighest + Vector3.left, PosHighest + Vector3.right, Color.red);
    }
}