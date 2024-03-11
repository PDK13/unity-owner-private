using UnityEngine;

public class SampleQueueEventManager : MonoBehaviour
{
    [SerializeField] private SampleQueueEventA m_eventA;
    [SerializeField] private SampleQueueEventB m_eventB;
    [SerializeField] private SampleQueueEventC m_eventC;

    private void Start()
    {
        QueueEventManager.Instance.Data.SetQueue(m_eventA);
        QueueEventManager.Instance.Data.SetQueue(m_eventB);
        QueueEventManager.Instance.Data.SetFinal(m_eventC);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            QueueEventManager.Instance.Data.SetInvoke();
    }
}