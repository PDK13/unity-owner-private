using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBodyXY2D : MonoBehaviour
{
    [SerializeField] private ControlMoveX2D m_controlX;
    [SerializeField] private ControlJumpY2D m_controlY;

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
            m_controlX.MoveDir = QuickMethode.DirectionX.Left;
        else
        if (Input.GetKey(KeyCode.RightArrow))
            m_controlX.MoveDir = QuickMethode.DirectionX.Right;
        else
            m_controlX.MoveDir = QuickMethode.DirectionX.None;
        //
        if (Input.GetKeyDown(KeyCode.UpArrow))
            m_controlY.SetEventClick();

        if (Input.GetKey(KeyCode.UpArrow))
            m_controlY.SetEventHold();
        else
            m_controlY.SetEventRelease();
    }

    private void FixedUpdate()
    {
        m_controlX.SetProgessMove();
        m_controlY.SetProgessJump();
    }
}
