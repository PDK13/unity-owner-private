using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderMessageBase : MonoBehaviour
{
    [SerializeField] private string m_tag = "";

    [Space]
    [SerializeField] private LayerMask m_checkLayer;
    [SerializeField] private List<string> m_checkTag = new List<string>();

    private enum MessageType
    {
        None = 0,
        Collider = 1,
        Rigidbody = 2,
    }

    [Space]
    [SerializeField] private GameObject m_messageSend;
    [SerializeField] private MessageType m_messageType = MessageType.None;
    [SerializeField] private SendMessageOptions m_messageOptions = SendMessageOptions.DontRequireReceiver;

    [Space]
    [SerializeField] private string m_methodeEnter = "OnCheckEnter";
    [SerializeField] private string m_methodeStay = "OnCheckStay";
    [SerializeField] private string m_methodeExit = "OnCheckExit";

    private void Start()
    {
        m_messageSend ??= this.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SetMessage(m_tag, m_methodeEnter, collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        SetMessage(m_tag, m_methodeStay, collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        SetMessage(m_tag, m_methodeExit, collision);
    }
    
    private bool SetMessage(string Tag, string Methode, Collider2D Collision)
    {
        if (Methode == "")
            return false;

        if (Collision.gameObject.Equals(m_messageSend))
            return false;

        if (!m_checkTag.Contains(Collision.gameObject.tag) && m_checkTag.Count > 0)
            return false;

        if (((1 << Collision.gameObject.layer) & m_checkLayer) != 0 || m_checkLayer == 0)
        {
            switch (m_messageType)
            {
                case MessageType.None:
                    m_messageSend.SendMessage(Methode, m_messageOptions);
                    break;
                case MessageType.Collider:
                    m_messageSend.SendMessage(Methode, new ColliderMessageData(Tag, Collision.gameObject), m_messageOptions);
                    break;
                case MessageType.Rigidbody:
                    if (Collision.attachedRigidbody == null)
                        return false;
                    if (Collision.attachedRigidbody.gameObject.Equals(m_messageSend))
                        return false;
                    m_messageSend.SendMessage(Methode, new ColliderMessageData(Tag, Collision.attachedRigidbody.gameObject), m_messageOptions);
                    break;
            }
            return true;
        }

        return false;
    }
}