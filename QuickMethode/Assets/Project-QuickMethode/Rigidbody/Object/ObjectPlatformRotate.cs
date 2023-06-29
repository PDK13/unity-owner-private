using QuickMethode;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ObjectPlatformRotate : MonoBehaviour
{
    #region Varible: Circle

    [SerializeField] private DirectionX m_dir = DirectionX.None;
    [SerializeField] private float m_speed = 50f;
    [SerializeField] private float m_radius = 5f;

    public float SurfaceForce => m_speed * m_radius * (int)m_dir * Time.fixedDeltaTime;

    #endregion

    private Rigidbody2D m_rigidbody;

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
}