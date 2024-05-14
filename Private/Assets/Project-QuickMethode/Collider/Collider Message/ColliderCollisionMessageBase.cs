using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCollisionMessageBase : MonoBehaviour
{
    [SerializeField] private string m_tag = "";

    [Space]
    [SerializeField] private LayerMask m_checkLayer;
    [SerializeField] private List<string> m_checkTag = new List<string>();

    private enum MessageTargetType
    {
        None = 0,
        Collider = 1,
        Rigidbody = 2,
    }

    [Space]
    [SerializeField] private GameObject m_base;
    [SerializeField] private MessageTargetType m_messageType = MessageTargetType.None;
    [SerializeField] private SendMessageOptions m_messageOptions = SendMessageOptions.DontRequireReceiver;

    [Space]
    [SerializeField] private string m_methodeEnter = "OnCheckEnter";
    [SerializeField] private string m_methodeStay = "OnCheckStay";
    [SerializeField] private string m_methodeExit = "OnCheckExit";

    private void Start()
    {
        m_base ??= this.gameObject;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SetMessage(m_tag, m_methodeEnter, collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        SetMessage(m_tag, m_methodeStay, collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        SetMessage(m_tag, m_methodeExit, collision);
    }

    private bool SetMessage(string Tag, string Methode, Collision2D Collision)
    {
        if (Methode == "")
            return false;

        if (Collision.gameObject.Equals(m_base))
            return false;

        if (!m_checkTag.Contains(Collision.gameObject.tag) && m_checkTag.Count > 0)
            return false;

        if (((1 << Collision.gameObject.layer) & m_checkLayer) != 0 || m_checkLayer == 0)
        {
            switch (m_messageType)
            {
                case MessageTargetType.None:
                    m_base.SendMessage(Methode, m_messageOptions);
                    break;
                case MessageTargetType.Collider:
                    if (string.IsNullOrEmpty(Tag))
                        m_base.SendMessage(Methode, new ColliderMessageData(Tag, Collision.gameObject), m_messageOptions);
                    else
                        m_base.SendMessage(Methode, Collision.gameObject, m_messageOptions);
                    break;
                case MessageTargetType.Rigidbody:
                    if (Collision.rigidbody == null)
                        return false;
                    if (Collision.rigidbody.gameObject.Equals(m_base))
                        return false;
                    if (string.IsNullOrEmpty(Tag))
                        m_base.SendMessage(Methode, new ColliderMessageData(Tag, Collision.rigidbody.gameObject), m_messageOptions);
                    else
                        m_base.SendMessage(Methode, Collision.rigidbody.gameObject, m_messageOptions);
                    break;
            }
            return true;
        }

        return false;
    }
}
