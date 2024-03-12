using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class ContinuousImpulse : MonoBehaviour
{
    private bool m_active;
 
    [CinemachineImpulseDefinitionProperty]
    [SerializeField] private CinemachineImpulseDefinition m_impulseDefinition = new CinemachineImpulseDefinition();
 
    float LastEventTime = 0;

    public void Active(bool active, float duration)
    {
        m_active = active;
        m_impulseDefinition.m_ImpulseDuration = duration;
        StopAllCoroutines();
        if (m_active)
            StartCoroutine(ITimer(duration));
    }
    
    void Update()
    {
        if(!m_active)
            return;
        var now = Time.time;
        float eventLength = m_impulseDefinition.m_TimeEnvelope.m_AttackTime + m_impulseDefinition.m_TimeEnvelope.m_SustainTime;
        if (now - LastEventTime > eventLength)
        {
            m_impulseDefinition.CreateEvent(transform.position, new Vector3(0.1f, 0.1f, 0));
            LastEventTime = now;
        }
    }

    IEnumerator ITimer(float time)
    {
        yield return new WaitForSeconds(time);
        m_active = false;
    }
}
