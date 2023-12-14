using System.Collections;
using UnityEngine;

public class SampleMarioEnemy : MonoBehaviour
{
    [SerializeField] private SampleMarioStompEnemyHead m_head;
    [SerializeField] private SampleMarioStompEnemyBody m_body;

    [Space]
    [SerializeField] private float m_stompDelay = 1f;

    [Space]
    [SerializeField] private Collider2D m_bodyTrigger;
    [SerializeField] private Collider2D m_headTrigger;

    private void Awake()
    {
        m_head.onStomp += OnStomp;
        m_body.onHit += OnHit;
    }

    private void OnDestroy()
    {
        m_head.onStomp -= OnStomp;
        m_body.onHit -= OnHit;
    }

    private void OnStomp(SampleMarioStompPlayerFoot From)
    {
        Debug.LogFormat("[Debug] Stomp by {0}", From.Base.name);
        //
        StartCoroutine(ISetStompDelay());
    }

    private void OnHit(SampleMarioStompPlayerBody From)
    {

    }

    private IEnumerator ISetStompDelay()
    {
        m_bodyTrigger.enabled = false;
        m_headTrigger.enabled = false;

        yield return new WaitForSeconds(m_stompDelay);

        m_bodyTrigger.enabled = true;
        m_headTrigger.enabled = true;
    }
}