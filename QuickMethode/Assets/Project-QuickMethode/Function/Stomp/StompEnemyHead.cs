using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class StompEnemyHead : MonoBehaviour
{
    //NOTE: Should use 2 layer for Enemy Head and Player Foot that only contact each other!!

    [SerializeField] private GameObject m_enemy;
    [SerializeField] private string m_methode = "OnStomp";

    private void OnStompReceive(string EnemyReceive)
    {
        if (m_enemy != null)
            m_enemy.SendMessage(m_methode, EnemyReceive, SendMessageOptions.DontRequireReceiver);
    }
}