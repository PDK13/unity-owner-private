using UnityEngine;

public class SampleMarioPlayer : MonoBehaviour
{
    [SerializeField] private SampleMarioStompPlayerFoot m_head;
    [SerializeField] private SampleMarioStompPlayerBody m_body;

    [Space]
    [SerializeField] private float m_stompForce = 15f;

    [Space]
    [SerializeField] private Rigidbody2D m_rigidbody;

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

    private void OnStomp(SampleMarioStompEnemyHead From)
    {
        m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, m_stompForce);
    }

    private void OnHit(SampleMarioStompEnemyBody From)
    {
        Debug.LogFormat("[Debug] Hit by {0}!!", From.Base.name);
    }
}
