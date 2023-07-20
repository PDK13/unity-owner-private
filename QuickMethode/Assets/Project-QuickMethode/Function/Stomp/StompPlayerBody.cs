using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompPlayerBody : MonoBehaviour
{
    //NOTE: Should use 2 layer for Enemy Head and Player Foot that only contact each other!!

    [SerializeField] private GameObject m_player;
    [SerializeField] private string m_methode = "OnHit";

    private void OnHitReceive(string EnemyReceive)
    {
        if (m_player != null)
            m_player.SendMessage(m_methode, EnemyReceive, SendMessageOptions.DontRequireReceiver);
    }
}