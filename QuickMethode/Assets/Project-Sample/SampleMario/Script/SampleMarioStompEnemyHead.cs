using System;
using UnityEngine;

public class SampleMarioStompEnemyHead : MonoBehaviour
{
    [SerializeField] private GameObject m_base;

    public GameObject Base => m_base;

    public Action<SampleMarioStompPlayerFoot> onStomp;

    public void SetStomp(SampleMarioStompPlayerFoot From)
    {
        onStomp?.Invoke(From);
    }
}