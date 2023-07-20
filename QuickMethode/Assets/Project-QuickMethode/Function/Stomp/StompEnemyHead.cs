using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class StompEnemyHead : MonoBehaviour
{
    //NOTE: Should use 2 layer for Enemy Head and Player Foot that only contact each other!!

    [SerializeField] private GameObject m_enemy;
    [SerializeField] private List<string> m_methode = new List<string>() { "OnStomp" };

    private void OnStompReceive(string EnemyReceive)
    {
        if (m_enemy != null)
        {
            foreach (string Methode in m_methode)
                m_enemy.SendMessage(Methode, EnemyReceive, SendMessageOptions.DontRequireReceiver);
        }
    }
}