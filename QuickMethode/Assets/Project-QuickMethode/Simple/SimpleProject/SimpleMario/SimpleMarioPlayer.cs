using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMarioPlayer : MonoBehaviour
{
    [SerializeField] private float m_stompForce = 15f;

    [Space]
    [SerializeField] private Rigidbody2D m_rigidbody;

    private void OnStomp()
    {
        m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, m_stompForce);
    }
}
