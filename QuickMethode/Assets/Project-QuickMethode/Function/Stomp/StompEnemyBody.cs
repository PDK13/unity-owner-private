using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompEnemyBody : MonoBehaviour
{
    //NOTE: Should use 2 layer for Enemy Head and Player Foot that only contact each other!!

    [SerializeField] private GameObject m_enemy;
    [SerializeField] private List<string> m_methode = new List<string>() { "OnHit" };

    [Space]
    [Tooltip("Send an message to 'StompEnemyHead'")]
    [SerializeField] private string m_playerMessage = "";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_enemy != null)
        {
            foreach (string Methode in m_methode)
                m_enemy.SendMessage(Methode, collision.gameObject, SendMessageOptions.DontRequireReceiver);
        }
        //
        collision.gameObject.SendMessage("OnHitReceive", m_playerMessage, SendMessageOptions.DontRequireReceiver);
    }
}