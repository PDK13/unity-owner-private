using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ColliderEvent : MonoBehaviour
{
    public static Action<string> onEvent;

    private enum TriggerType
    {
        None = 0,
        Once = 1,
        Destroy = 2,
    }

    [SerializeField] private string m_key;
    [SerializeField] private TriggerType m_trigger = TriggerType.None;

    private bool m_active = false;

    [Space]
    [SerializeField] private LayerMask m_checkLayer;
    [SerializeField] private List<string> m_checkTag = new List<string>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_active && (m_trigger == TriggerType.Once || m_trigger == TriggerType.Destroy))
            return;
        //
        if (!SetEvent(collision))
            return;
        //
        m_active = true;
        //
        if (m_trigger == TriggerType.Destroy)
            Destroy(this.gameObject);
    }

    private bool SetEvent(Collider2D Collision)
    {
        if (!m_checkTag.Contains(Collision.gameObject.tag) && m_checkTag.Count > 0)
            return false;
        //
        if (((1 << Collision.gameObject.layer) & m_checkLayer) != 0 || m_checkLayer == 0)
        {
            onEvent?.Invoke(m_key);
            //
            return true;
        }
        //
        return false;
    }
}