using QuickMethode;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlatformTop2D : MonoBehaviour
{
    //Each Platform (GameObject Child) use once this script, can be use with Platform Effector 2D Component!

    #region Varible: Platform Collider

    private List<GameObject> m_collision = new List<GameObject>();
    private Vector2 m_posLast;

    //Collider Base Y is the limit for active platform logic, same to Platform Effector 2D Component!
    private float ColliderBaseY => QCollider2D.GetBorderPos(m_colliderBase, Direction.Down).y;

    //The last movement of this GameObject on world scene by last frame!
    private Vector2 DirLast => ((Vector2)transform.position - m_posLast);

    #endregion

    //Get the fisrt Collider 2D on this GameObject, shouldn't have muti Collider 2D on same GameObject!
    private Collider2D m_colliderBase;

    //Platform Effector Component is optional, it's Rotational Offset should be (-1) with Transform Euler!

    private void Start()
    {
        m_colliderBase = GetComponent<Collider2D>();

        m_posLast = transform.position;
    }

    private void LateUpdate()
    {
        foreach (GameObject Collision in m_collision)
            Collision.transform.position += (Vector3)DirLast;
        m_posLast = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (m_collision.Contains(collision.gameObject))
            return;

        Collider2D m_collisionCollider = collision.gameObject.GetComponent<Collider2D>();
        float m_collisionColliderY = QCollider2D.GetBorderPos(m_collisionCollider, Direction.Down).y;

        if (m_collisionColliderY < ColliderBaseY)
            return;

        m_collision.Add(collision.gameObject);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        m_collision.Remove(collision.gameObject);
    }

    private void OnDrawGizmos()
    {
        m_colliderBase = GetComponent<Collider2D>();

        float X1 = m_colliderBase.bounds.center.x - m_colliderBase.bounds.size.x / 2;
        float X2 = m_colliderBase.bounds.center.x + m_colliderBase.bounds.size.x / 2;
        QGizmos.SetLine(new Vector3(X1, ColliderBaseY), new Vector3(X2, ColliderBaseY), Color.red);
    }
}
