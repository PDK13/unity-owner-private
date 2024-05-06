using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ColliderMessageChild : MonoBehaviour
{
    [Serializable]
    private class ColliderChildData
    {
        public string Tag = "";
        public Collider2D Collider;
        [HideInInspector] public bool Active = false;
    }

    [Space]
    [SerializeField] private List<ColliderChildData> m_child = new List<ColliderChildData>();

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

    private void Awake()
    {
        m_messageSend ??= this.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_methodeEnter == "")
            return;

        for (int i = 0; i < m_child.Count; i++)
        {
            if (!m_child[i].Active && m_child[i].Collider.IsTouching(collision))
            {
                m_child[i].Active = true;
                SetMessage(m_child[i].Tag, m_methodeEnter, collision);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (m_methodeStay == "")
            return;

        for (int i = 0; i < m_child.Count; i++)
        {
            if (m_child[i].Active && m_child[i].Collider.IsTouching(collision))
            {
                SetMessage(m_child[i].Tag, m_methodeStay, collision);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (m_methodeExit == "")
            return;

        for (int i = 0; i < m_child.Count; i++)
        {
            if (m_child[i].Active && !m_child[i].Collider.IsTouching(collision))
            {
                m_child[i].Active = false;
                SetMessage(m_child[i].Tag, m_methodeExit, collision);
            }
        }
    }

    private bool SetMessage(string Tag, string Methode, Collider2D Collision)
    {
        if (Methode == "")
            return false;

        if (Collision.gameObject.Equals(m_messageSend))
            return false;

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
}