using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroy : MonoBehaviour
{
    [SerializeField] private ParticleSystem m_base;

    private void Start()
    {
        SetStart();
    }

    public void SetStart()
    {
        m_base.Play();
        Destroy(m_base.gameObject, m_base.main.duration + m_base.main.startLifetimeMultiplier);
    }
}
