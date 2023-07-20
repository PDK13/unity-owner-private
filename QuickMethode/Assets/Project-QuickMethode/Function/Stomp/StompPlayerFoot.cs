using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class StompPlayerFoot : MonoBehaviour
{
    //NOTE: Should use 2 layer for Enemy Head and Player Foot that only contact each other!!

    [SerializeField] private GameObject m_player;
    [SerializeField] private string m_methode = "OnStomp";

    [Space]
    [Tooltip("Send an message to 'StompEnemyHead'")]
    [SerializeField] private string m_enemyMessage = "";

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (m_player != null)
            m_player.SendMessage(m_methode, SendMessageOptions.DontRequireReceiver);
        //
        collision.collider.gameObject.SendMessage("OnStompReceive", m_enemyMessage, SendMessageOptions.DontRequireReceiver);
    }
}