using UnityEngine;

[AddComponentMenu("Quick Methode/Rigidbody/Rigidbody Rotate 3D")]
public class RigidbodyRotate3D : MonoBehaviour
{
    [SerializeField] private Rigidbody m_Rigidbody;

    private enum AxisType { Up, Right, Forward, }

    [SerializeField] private AxisType m_Axis = AxisType.Right;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();

        if (m_Rigidbody == null)
            Debug.LogErrorFormat("{0}: Require Componenet Rigidbody or Rigidbody2D.", name);
    }

    private void FixedUpdate()
    {
        switch (m_Axis)
        {
            case AxisType.Right:
                transform.right = new Vector3(m_Rigidbody.velocity.x, m_Rigidbody.velocity.y, m_Rigidbody.velocity.z);
                break;
            case AxisType.Up:
                transform.up = new Vector3(m_Rigidbody.velocity.x, m_Rigidbody.velocity.y, m_Rigidbody.velocity.z);
                break;
            case AxisType.Forward:
                transform.forward = new Vector3(m_Rigidbody.velocity.x, m_Rigidbody.velocity.y, m_Rigidbody.velocity.z);
                break;
        }
    }
}