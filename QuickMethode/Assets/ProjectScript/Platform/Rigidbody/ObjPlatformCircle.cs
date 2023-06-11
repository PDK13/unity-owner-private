using QuickMethode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjPlatformCircle : MonoBehaviour
{
    [SerializeField] private DirectionX m_dir = DirectionX.None;
    public DirectionX Dir => m_dir;

    [SerializeField] private float m_speed = 50f;
    public float Speed => m_speed;
    public float Radius => m_collider.radius * transform.localScale.x;
    public Vector2 Centre => m_collider.bounds.center;

    private Rigidbody2D m_rigidbody;
    private CircleCollider2D m_collider;

    private void Start()
    {
        m_collider = QComponent.GetComponent<CircleCollider2D>(gameObject);

        m_rigidbody = QComponent.GetComponent<Rigidbody2D>(gameObject);
        m_rigidbody.bodyType = RigidbodyType2D.Kinematic;
        m_rigidbody.useFullKinematicContacts = true;
    }

    private void FixedUpdate()
    {
        m_rigidbody.angularVelocity = m_speed * (int)m_dir * -1;
        if (m_rigidbody.rotation >= 360f || m_rigidbody.rotation <= -360f)
            m_rigidbody.rotation = 0;
    }

    private void OnDrawGizmos()
    {
        m_collider = QComponent.GetComponent<CircleCollider2D>(gameObject);

        QGizmos.SetLine(
            this.transform.position,
            this.transform.position + QCircle.GetPosXY(transform.eulerAngles.z, Radius),
            Color.red);
    }
}
