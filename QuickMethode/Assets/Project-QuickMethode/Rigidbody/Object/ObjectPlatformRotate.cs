using QuickMethode;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ObjectPlatformRotate : MonoBehaviour
{
    [SerializeField] private DirectionX m_dir = DirectionX.Left;
    [SerializeField] private float m_speed = 10f;

    [SerializeField] private CircleCollider2D m_circleCollider;
    [SerializeField] private float m_radius = 1f;

    public DirectionX Dir { get => m_dir; set => m_dir = value; }
    public float Speed { get => m_speed; set => m_speed = value; }
    public float Radius
    {
        get => m_circleCollider != null ? m_circleCollider.radius * transform.localScale.x : m_radius;
        set
        {
            if (m_circleCollider != null)
                m_circleCollider.radius = value;
            else
                m_radius = value;
        }
    }
    public Vector2 Centre => m_circleCollider != null ? m_circleCollider.bounds.center : this.transform.position;
    public float SurfaceForce => m_speed * m_radius * (int)m_dir * Time.fixedDeltaTime;

    [HideInInspector] private Rigidbody2D m_rigidbody;

    private void Start()
    {
        m_rigidbody = QComponent.GetComponent<Rigidbody2D>(gameObject);
    }

    private void FixedUpdate()
    {
        m_rigidbody.angularVelocity = m_speed * (int)m_dir * -1;
        if (m_rigidbody.rotation >= 360f || m_rigidbody.rotation <= -360f)
            m_rigidbody.rotation = 0;
    }

    private void OnDrawGizmos()
    {
        QGizmos.SetLine(this.transform.position, this.transform.position + QCircle.GetPosXY(transform.eulerAngles.z, Radius), Color.red);
    }
}