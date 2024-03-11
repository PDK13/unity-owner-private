using System.Collections;
using UnityEngine;

public class SampleQueueEventB : MonoBehaviour, IQueueEvent
{
    [SerializeField] private bool m_activeThis = true;

    public void ISetInvoke()
    {
        if (m_activeThis)
            StartCoroutine(ISetEventA());
        else
            QueueEventManager.Instance.SetGroup().SetInvoke();
    }

    private IEnumerator ISetEventA()
    {
        Debug.Log("[Sample] Event B called!");
        yield return new WaitForSeconds(1f);
        Debug.Log("[Sample] Event B ended!");
        //
        QueueEventManager.Instance.SetGroup().SetInvoke();
    }
}