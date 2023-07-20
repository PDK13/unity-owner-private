using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompEnemyBody : MonoBehaviour
{
    //NOTE: Should use 2 layer for Enemy Head and Player Foot that only contact each other!!

    [SerializeField] private GameObject m_enemy;
    [SerializeField] private string m_methode = "OnHit";

    [Space]
    [Tooltip("Send an message to 'StompEnemyHead'")]
    [SerializeField] private string m_playerMessage = "";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_enemy != null)
            m_enemy.SendMessage(m_methode, collision.gameObject, SendMessageOptions.DontRequireReceiver);
        //
        collision.gameObject.SendMessage("OnHitReceive", m_playerMessage, SendMessageOptions.DontRequireReceiver);
    }
}