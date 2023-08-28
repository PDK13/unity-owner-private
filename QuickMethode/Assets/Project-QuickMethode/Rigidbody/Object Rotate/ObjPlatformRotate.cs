using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ObjPlatformRotate : MonoBehaviour
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
            {
                m_circleCollider.radius = value;
            }
            else
            {
                m_radius = value;
            }
        }
    }
    public bool CircleCollider => m_circleCollider != null;
    public Vector2 Centre => m_circleCollider != null ? m_circleCollider.bounds.center : transform.position;
    public float SurfaceForce => m_speed * m_radius * (int)m_dir * Time.fixedDeltaTime;

    [HideInInspector] private Rigidbody2D m_rigidbody;

    private void Start()
    {
        m_rigidbody = QComponent.GetComponent<Rigidbody2D>(gameObject);
        m_rigidbody.bodyType = RigidbodyType2D.Kinematic;
    }

    private void FixedUpdate()
    {
        m_rigidbody.angularVelocity = m_speed * (int)m_dir * -1;
        if (m_rigidbody.rotation >= 360f || m_rigidbody.rotation <= -360f)
        {
            m_rigidbody.rotation = 0;
        }
    }

    private void OnDrawGizmos()
    {
        QGizmos.SetLine(transform.position, transform.position + QCircle.GetPosXY(transform.eulerAngles.z, Radius), Color.red);
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(ObjPlatformRotate))]
public class ObjPlatformRotateEditor : Editor
{
    private ObjPlatformRotate m_target;

    private SerializedProperty m_dir;
    private SerializedProperty m_speed;
    private SerializedProperty m_circleCollider;
    private SerializedProperty m_radius;

    private void OnEnable()
    {
        m_target = (target as ObjPlatformRotate);

        m_dir = serializedObject.FindProperty("m_dir");
        m_speed = serializedObject.FindProperty("m_speed");
        m_circleCollider = serializedObject.FindProperty("m_circleCollider");
        m_radius = serializedObject.FindProperty("m_radius");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(m_dir);
        EditorGUILayout.PropertyField(m_speed);
        QEditor.SetChanceCheckBegin();
        EditorGUILayout.PropertyField(m_circleCollider);
        if (QEditor.SetChanceCheckEnd())
        {
            if (!m_target.CircleCollider)
            {
                EditorGUILayout.PropertyField(m_radius);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}

#endif