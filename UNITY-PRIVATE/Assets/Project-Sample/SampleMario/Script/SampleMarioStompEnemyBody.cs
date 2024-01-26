using System;
using UnityEngine;

public class SampleMarioStompEnemyBody : MonoBehaviour
{
    public Action<SampleMarioStompPlayerBody> onHit;

    [SerializeField] private GameObject m_base;
    [SerializeField] private bool m_stay = true;

    public GameObject Base => m_base;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        onHit?.Invoke(collision.GetComponent<SampleMarioStompPlayerBody>());
        //
        collision.GetComponent<SampleMarioStompPlayerBody>().SetHit(this);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!m_stay)
        {
            return;
        }
        //
        onHit?.Invoke(collision.GetComponent<SampleMarioStompPlayerBody>());
        //
        collision.GetComponent<SampleMarioStompPlayerBody>().SetHit(this);
    }
}