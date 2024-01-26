using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleHashCode : MonoBehaviour
{
    [SerializeField] private string m_value = "10";

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            //Test result from debug log:
            //--------------------------------------------
            //--[Debug] Hash code of 10 is 98844765     --
            //--[Debug] Hash code of 15 is 858359652    --
            //--[Debug] Hash code of 10 is 98844765     --
            //--------------------------------------------
            //From test result, Hash Code will return a value to a number code of that value (if the value is number, Hash Code will be a number too and equa to that value)
            Debug.LogFormat("[Debug] Hash code of {0} is {1}", m_value, m_value.GetHashCode());
    }
}