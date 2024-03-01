using System;
using UnityEngine;

public class SampleMarioStompPlayerBody : MonoBehaviour
{
    public Action<SampleMarioStompEnemyBody> onHit;

    [SerializeField] private GameObject m_base;

    public GameObject Base => m_base;

    public void SetHit(SampleMarioStompEnemyBody From)
    {
        onHit?.Invoke(From);
    }
}