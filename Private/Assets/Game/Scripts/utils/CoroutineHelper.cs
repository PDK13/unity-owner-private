using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineHelper : MonoBehaviour
{
    private static GameObject m_go;

    public static Coroutine NewCoroutine(IEnumerator coroutine)
    {
        if(m_go == null)
        {
            m_go = new GameObject();
            m_go.name = "corountine";
            DontDestroyOnLoad(m_go);
        }
        CoroutineHelper helper = m_go.AddComponent<CoroutineHelper>();
        Coroutine cr = helper.StartTimer(coroutine);
        return cr;
    }

    Coroutine StartTimer(IEnumerator coroutine)
    {
        return StartCoroutine(coroutine);
    }
}
