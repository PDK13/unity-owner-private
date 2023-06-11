using UnityEngine;

[AddComponentMenu("Quick Methode/Rigidbody/Rigidbody Gravity")]
public class RigidbodyGravity : MonoBehaviour
{
    [SerializeField] private float m_GravityScale = 1.0f;

    [SerializeField] private float m_GravityGlobal = 9.81f;

    private Rigidbody m_Rigidbody;

    private void Awake()
    {
        if (GetComponent<Rigidbody2D>() == null)
        {
            if (GetComponent<Rigidbody>() == null)
            {
                gameObject.AddComponent<Rigidbody>();
            }
        }
        else
        {
            Debug.LogErrorFormat("{0}: Detect Rigidbody2D Component, please remove it!", name);
        }

        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        m_Rigidbody.useGravity = false;
    }

    private void FixedUpdate()
    {
        if (m_Rigidbody == null)
        {
            return;
        }

        Vector3 m_Gravity = GetGravityGlobalVector() * GetGravityScale();

        m_Rigidbody.AddForce(m_Gravity, ForceMode.Acceleration);
    }

    #region Set

    public void SetGravityScale(float m_GravityScale)
    {
        this.m_GravityScale = m_GravityScale;
    }

    public void SetGravityGlobal(float m_GravityGlobal)
    {
        this.m_GravityGlobal = m_GravityGlobal;
    }

    public void SetRigidbodyDrag(float m_GravityDrag)
    {
        m_Rigidbody.drag = m_GravityDrag;
    }

    #endregion

    #region Get

    public float GetGravityScale()
    {
        return m_GravityScale;
    }

    public float GetGravityGlobalFloat()
    {
        return m_GravityGlobal;
    }

    public Vector3 GetGravityGlobalVector()
    {
        return GetGravityGlobalFloat() * Vector3.down;
    }

    public float GetRigidbodyDrag()
    {
        return m_Rigidbody.drag;
    }

    #endregion
}
