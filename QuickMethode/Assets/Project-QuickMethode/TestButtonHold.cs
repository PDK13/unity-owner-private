using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButtonHold : MonoBehaviour
{
    [SerializeField] private UIButtonHold m_btnTest;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            m_btnTest.SetButtonPress();
        //
        if (Input.GetKeyUp(KeyCode.Space))
            m_btnTest.SetButtonRelease();
    }

    public void BtnHold()
    {
        Debug.Log("[Button] Hold");
    }

    public void BtnDown()
    {
        Debug.Log("[Button] Down");
    }

    public void BtnUp()
    {
        Debug.Log("[Button] Up");
    }

    public void BtnEnter()
    {
        Debug.Log("[Button] Enter");
    }

    public void BtnExit()
    {
        Debug.Log("[Button] Exit");
    }
}