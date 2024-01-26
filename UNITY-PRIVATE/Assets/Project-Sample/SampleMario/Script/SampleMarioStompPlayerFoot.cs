using System;
using UnityEngine;

public class SampleMarioStompPlayerFoot : MonoBehaviour
{
    public Action<SampleMarioStompEnemyHead> onStomp;

    [SerializeField] private GameObject m_base;
    [SerializeField][Min(0)] private float m_velocityMinY = 0.5f;

    public GameObject Base => m_base;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.otherRigidbody.velocity.y > m_velocityMinY)
        {
            return;
        }
        //
        onStomp?.Invoke(collision.collider.GetComponent<SampleMarioStompEnemyHead>());
        //
        collision.collider.GetComponent<SampleMarioStompEnemyHead>().SetStomp(this);
    }
}