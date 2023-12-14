using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePlay : MonoBehaviour
{
    [SerializeField] protected ParticleSystem m_base;
    [SerializeField] protected bool m_logicOnAwake = false;

    private void Start()
    {
        if (m_logicOnAwake)
            SetStart();
    }

    public virtual void SetStart()
    {
        if (m_base == null)
            m_base = QComponent.GetComponent<ParticleSystem>(this);
        m_base.Play();
    }

    public virtual void SetPause()
    {
        if (m_base == null)
            m_base = QComponent.GetComponent<ParticleSystem>(this);
        m_base.Pause();
    }
}