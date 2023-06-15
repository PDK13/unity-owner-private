using QuickMethode;
using UnityEngine;

public class RendererCollider : MonoBehaviour
{
    private Collider2D m_Collider;
    private BoxCollider2D m_BoxCollider;
    private CircleCollider2D m_CircleCollider;

    private void OnDrawGizmos()
    {
        if (m_Collider == null)
        {
            m_BoxCollider = GetComponent<BoxCollider2D>();
            m_CircleCollider = GetComponent<CircleCollider2D>();

            if (m_BoxCollider != null)
                m_Collider = m_BoxCollider;
            else
            if (m_CircleCollider != null)
                m_Collider = m_CircleCollider;
        }

        if (m_BoxCollider != null)
            QGizmos.SetCollider2D(m_BoxCollider, Color.white);
        else
        if (m_CircleCollider != null)
            QGizmos.SetCollider2D(m_CircleCollider, Color.white);
    }
}
