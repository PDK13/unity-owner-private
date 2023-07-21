using System;
using UnityEngine;

public class StompEnemyBody : MonoBehaviour
{
    public Action<StompPlayerBody> onHit;

    [SerializeField] private GameObject m_base;
    [SerializeField] private bool m_stay = true;

    public GameObject Base => m_base;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        onHit?.Invoke(collision.GetComponent<StompPlayerBody>());
        //
        collision.GetComponent<StompPlayerBody>().SetHit(this);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!m_stay)
            return;
        //
        onHit?.Invoke(collision.GetComponent<StompPlayerBody>());
        //
        collision.GetComponent<StompPlayerBody>().SetHit(this);
    }
}