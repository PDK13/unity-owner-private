using System.Collections;
using UnityEngine;

public class SampleQueueEventC : MonoBehaviour, IQueueEvent
{
    [SerializeField] private bool m_activeThis = true;

    public void ISetInvoke()
    {
        if (m_activeThis)
            StartCoroutine(ISetEventA());
        else
            QueueEventManager.Instance.Data.SetInvoke();
    }

    private IEnumerator ISetEventA()
    {
        Debug.Log("[Sample] Event C called!");
        yield return new WaitForSeconds(1f);
        Debug.Log("[Sample] Event C ended!");
        //
        QueueEventManager.Instance.Data.SetInvoke();
    }
}