using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleQueueEventB : MonoBehaviour, IQueueEvent
{
    [SerializeField] private bool m_activeThis = true;

    public void SetInvoke()
    {
        if (m_activeThis)
            StartCoroutine(ISetEventA());
        else
            QueueEventManager.Instance.SetInvoke();
    }

    private IEnumerator ISetEventA()
    {
        Debug.Log("[Sample] Event B called!");
        yield return new WaitForSeconds(1f);
        Debug.Log("[Sample] Event B ended!");
        //
        QueueEventManager.Instance.SetInvoke();
    }
}