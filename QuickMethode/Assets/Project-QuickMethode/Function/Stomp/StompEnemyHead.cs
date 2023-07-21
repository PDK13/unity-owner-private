using System;
using UnityEngine;

public class StompEnemyHead : MonoBehaviour
{
    [SerializeField] private GameObject m_base;

    public GameObject Base => m_base;

    public Action<StompPlayerFoot> onStomp;

    public void SetStomp(StompPlayerFoot From)
    {
        onStomp?.Invoke(From);
    }
}