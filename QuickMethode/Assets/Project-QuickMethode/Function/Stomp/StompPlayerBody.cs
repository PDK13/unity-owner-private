using System;
using UnityEngine;

public class StompPlayerBody : MonoBehaviour
{
    public Action<StompEnemyBody> onHit;

    [SerializeField] private GameObject m_base;

    public GameObject Base => m_base;

    public void SetHit(StompEnemyBody From)
    {
        onHit?.Invoke(From);
    }
}