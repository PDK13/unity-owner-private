using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleQueueEventManager : MonoBehaviour
{
    [SerializeField] private SampleQueueEventA m_eventA;
    [SerializeField] private SampleQueueEventB m_eventB;

    private QueueEventData m_queueEventData;

    private void Start()
    {
        m_queueEventData = QueueEventManager.Instance.SetGroup();
        m_queueEventData.SetAdd(m_eventA);
        m_queueEventData.SetAdd(m_eventB);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            m_queueEventData.SetInvoke();
    }
}