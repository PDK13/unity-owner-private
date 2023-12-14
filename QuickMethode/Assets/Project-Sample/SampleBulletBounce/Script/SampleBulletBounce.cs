using UnityEngine;

public class SampleBulletBounce : MonoBehaviour
{
    private Rigidbody2D m_rigidbody;
    private Vector2 m_velocityLast;

    private void Start()
    {
        Destroy(gameObject, 10f);
    }

    private void Update()
    {
        m_velocityLast = m_rigidbody.velocity;
    }

    public void SetInit(Vector2 Dir)
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_rigidbody.velocity = Dir.normalized;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 dirBounce = Vector2.Reflect(m_velocityLast.normalized, collision.contacts[0].normal);
        m_rigidbody.velocity = dirBounce.normalized;
    }
}
