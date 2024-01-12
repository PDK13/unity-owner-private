using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    private List<IQueueEvent> m_eventList = new List<IQueueEvent>();
    private int m_eventIndex = 0;

    private void Start()
    {
        IQueueEvent EventA = new EventA();
        IQueueEvent EventB = new EventA();

        m_eventList.Add(EventA);
        m_eventList.Add(EventB);
    }

    public void SetAdd(IQueueEvent Event)
    {
        m_eventList.Add(Event);
    }

    public void SetInvoke()
    {
        if (m_eventIndex >= m_eventList.Count)
            return;
        //
        m_eventList[m_eventIndex].SetInvoke();
        m_eventIndex++;
    }

    public void SetReset()
    {
        m_eventList.Clear();
        m_eventIndex = 0;
    }
}

public interface IQueueEvent
{
    void SetInvoke();
}

public class EventA : IQueueEvent
{
    public void SetInvoke()
    {
        
    }

    private IEnumerator IEventA()
    {
        Debug.Log("[Debug] Event A!");
        yield return new WaitForSeconds(1f);

    }
}

public class EventB : IQueueEvent
{
    public void SetInvoke()
    {
        Debug.Log("[Debug] Event B!");
    }
}