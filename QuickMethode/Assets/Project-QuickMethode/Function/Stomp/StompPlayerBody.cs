using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompPlayerBody : MonoBehaviour
{
    //NOTE: Should use 2 layer for Enemy Head and Player Foot that only contact each other!!

    [SerializeField] private GameObject m_player;
    [SerializeField] private List<string> m_methode = new List<string>() { "OnHit" };

    private void OnHitReceive(string EnemyReceive)
    {
        if (m_player != null)
        {
            foreach (string Methode in m_methode)
                m_player.SendMessage(Methode, EnemyReceive, SendMessageOptions.DontRequireReceiver);
        }
    }
}