using UnityEngine;

public class SampleQueueEventManager : MonoBehaviour
{
    [SerializeField] private SampleQueueEventA m_eventA;
    [SerializeField] private SampleQueueEventB m_eventB;

    private void Start()
    {
        QueueEventManager.Instance.SetGroup().SetAdd(m_eventA);
        QueueEventManager.Instance.SetGroup().SetAdd(m_eventB);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            QueueEventManager.Instance.SetGroup().SetInvoke();
    }
}