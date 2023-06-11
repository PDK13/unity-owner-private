using UnityEngine;

[AddComponentMenu("Project Script/Rigidbody/Rigidbody Rotate 2D")]
public class RigidbodyRotate2D : MonoBehaviour
{
    [SerializeField] private Rigidbody2D m_Rigidbody;

    private enum AxisType { Up, Right, Forward, }

    [SerializeField] private AxisType m_Axis = AxisType.Right;

    private void Awake()
    {
        if (m_Rigidbody == null)
            m_Rigidbody = GetComponent<Rigidbody2D>();

        if (m_Rigidbody == null)
            Debug.LogErrorFormat("{0}: Require Componenet Rigidbody or Rigidbody2D.", name);
    }

    private void FixedUpdate()
    {
        SetAxis();
    }

    public void SetAxis()
    {
        if (m_Rigidbody == null)
            return;
        
        switch (m_Axis)
        {
            case AxisType.Right:
                transform.right = new Vector2(m_Rigidbody.velocity.x, m_Rigidbody.velocity.y);
                break;
            case AxisType.Up:
                transform.up = new Vector2(m_Rigidbody.velocity.x, m_Rigidbody.velocity.y);
                break;
            case AxisType.Forward:
                transform.forward = new Vector2(m_Rigidbody.velocity.x, m_Rigidbody.velocity.y);
                break;
        }
    }

    private void SetRotateTest()
    {
        Vector3 Direction = Vector3.right;
        float Angle = Vector3.SignedAngle(Vector3.left, Direction, Vector3.forward);
        this.transform.rotation = Quaternion.Euler(0, 0, Angle + 180f);
    }
}
