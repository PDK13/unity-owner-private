using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTriggerMessage : MonoBehaviour
{
    private enum MessageType
    {
        None = 0,
        Collider = 1,
        Rigidbody = 2,
    }

    [SerializeField] private GameObject m_base;

    [Space]
    [SerializeField] private LayerMask m_checkLayer;
    [SerializeField] private List<string> m_checkTag = new List<string>();

    [Space]
    [SerializeField] private MessageType m_messageType = MessageType.None;
    [SerializeField] private string m_methodeEnter = "OnCheckEnter";
    [SerializeField] private string m_methodeStay = "OnCheckStay";
    [SerializeField] private string m_methodeExit = "OnCheckExit";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_methodeEnter == "")
            return;
        //
        if (collision.gameObject.Equals(m_base))
            return;
        //
        if (!m_checkTag.Contains(collision.gameObject.tag) && m_checkTag.Count > 0)
            return;
        //
        if (((1 << collision.gameObject.layer) & m_checkLayer) != 0 || m_checkLayer == 0)
        {
            switch (m_messageType)
            {
                case MessageType.None:
                    m_base.SendMessage(m_methodeEnter, SendMessageOptions.DontRequireReceiver);
                    break;
                case MessageType.Collider:
                    m_base.SendMessage(m_methodeEnter, collision.gameObject, SendMessageOptions.DontRequireReceiver);
                    break;
                case MessageType.Rigidbody:
                    if (collision.attachedRigidbody == null)
                        return;
                    if (collision.attachedRigidbody.gameObject.Equals(m_base))
                        return;
                    m_base.SendMessage(m_methodeEnter, collision.attachedRigidbody.gameObject, SendMessageOptions.DontRequireReceiver);
                    break;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (m_methodeStay == "")
            return;
        //
        if (collision.gameObject.Equals(m_base))
            return;
        //
        if (!m_checkTag.Contains(collision.gameObject.tag) && m_checkTag.Count > 0)
            return;
        //
        if (((1 << collision.gameObject.layer) & m_checkLayer) != 0 || m_checkLayer == 0)
        {
            switch (m_messageType)
            {
                case MessageType.None:
                    m_base.SendMessage(m_methodeStay, SendMessageOptions.DontRequireReceiver);
                    break;
                case MessageType.Collider:
                    m_base.SendMessage(m_methodeStay, collision.gameObject, SendMessageOptions.DontRequireReceiver);
                    break;
                case MessageType.Rigidbody:
                    if (collision.attachedRigidbody == null)
                        return;
                    if (collision.attachedRigidbody.gameObject.Equals(m_base))
                        return;
                    m_base.SendMessage(m_methodeStay, collision.attachedRigidbody.gameObject, SendMessageOptions.DontRequireReceiver);
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (m_methodeExit == "")
            return;
        //
        if (collision.gameObject.Equals(m_base))
            return;
        //
        if (!m_checkTag.Contains(collision.gameObject.tag) && m_checkTag.Count > 0)
            return;
        //
        if (((1 << collision.gameObject.layer) & m_checkLayer) != 0 || m_checkLayer == 0)
        {
            switch (m_messageType)
            {
                case MessageType.None:
                    m_base.SendMessage(m_methodeExit, SendMessageOptions.DontRequireReceiver);
                    break;
                case MessageType.Collider:
                    m_base.SendMessage(m_methodeExit, collision.gameObject, SendMessageOptions.DontRequireReceiver);
                    break;
                case MessageType.Rigidbody:
                    if (collision.attachedRigidbody == null)
                        return;
                    if (collision.attachedRigidbody.gameObject.Equals(m_base))
                        return;
                    m_base.SendMessage(m_methodeExit, collision.attachedRigidbody.gameObject, SendMessageOptions.DontRequireReceiver);
                    break;
            }
        }
    }
}