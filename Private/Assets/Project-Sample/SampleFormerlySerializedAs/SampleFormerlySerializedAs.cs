using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SampleFormerlySerializedAs : MonoBehaviour
{
    //NOTE: When changed name of varible, in normal way, the value of that varible will be reset to new value in code.

    [FormerlySerializedAs("m_data")] public int m_data = 0;

    #region How 'FormerlySerializedAs' worked in unity editor?

    //How 'FormerlySerializedAs' worked in unity editor?
    //It used to saved the last value of old varible and saved that value to new varible when rename the varible.
    //It used only ONE time fer renamed, mean we need to changed value of 'FormerlySerializedAs' before renamed the varible.

    #endregion

    #region How to used 'FormerlySerializedAs' step by step?

    //How to used 'FormerlySerializedAs' step by step?
    //1. Changed value 'FormerlySerializedAs' same as named of varible want to changed.
    //2. Changed name of varible.
    //3. If want to changed name of varible again, restart from step 1.

    #endregion
}