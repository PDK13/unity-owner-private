using System;
using System.Collections.Generic;
using UnityEngine;

public class QueueEventManager : SingletonManager<QueueEventManager>
{
    private QueueEventData m_data = new QueueEventData();

    public QueueEventData Data => m_data ??= new QueueEventData();
}

[Serializable]
public class QueueEventData
{
    private List<IQueueEvent> m_eventQueue = new List<IQueueEvent>();
    private int m_eventQueueIndex = 0;

    private IQueueEvent m_eventFinal = null;

    //

    public QueueEventData(IQueueEvent EventFinal = null)
    {
        m_eventQueue ??= new List<IQueueEvent>();
        m_eventQueueIndex = 0;
        //
        m_eventFinal = EventFinal;
    }

    //

    /// <summary>
    /// Add a event to queue!
    /// </summary>
    /// <param name="Event"></param>
    /// <param name="Force">When TRUE, the event will be add without check exist!</param>
    public void SetQueue(IQueueEvent Event, bool Force = false)
    {
        m_eventQueue ??= new List<IQueueEvent>();
        //
        if (!Force && m_eventQueue.Contains(Event))
            return;
        //
        m_eventQueue.Add(Event);
    }

    /// <summary>
    /// Add a event to final!
    /// </summary>
    /// <param name="Event"></param>
    public void SetFinal(IQueueEvent Event)
    {
        m_eventFinal = Event;
    }

    /// <summary>
    /// Start invoke the first event or continue the next event in queue!
    /// </summary>
    /// <param name="EventFinal">When event(s) in queue are invoked, then invoke this final event!</param>
    public void SetInvoke()
    {
        m_eventQueue ??= new List<IQueueEvent>();
        //
        if (m_eventQueueIndex <= m_eventQueue.Count - 1)
        {
            m_eventQueue[m_eventQueueIndex].ISetInvoke();
            m_eventQueueIndex++;
        }
        else
        if (m_eventFinal != null)
            m_eventFinal.ISetInvoke();
    }

    /// <summary>
    /// Reset index of event(s) in queue!
    /// </summary>
    public void SetReset(bool Clear = false)
    {
        if (Clear)
            m_eventQueue = new List<IQueueEvent>();
        m_eventQueueIndex = 0;
    }
}

public interface IQueueEvent
{
    void ISetInvoke();
}