using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPress : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    public static UIPress Instance;

    //Use on ui canvas with rectransform full stretch screen to get full mouse press support!

    [SerializeField] private LayerMask m_layerCheck;
    [SerializeField] private float m_pressRadius = 0.1f;

    [SerializeField] private string m_messageSend = "OnPress";

    public Action onNoHit;

    [Space]
    [SerializeField] private bool m_debug;
    private Vector2? m_debugPositionLast;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("[UIPress] There are more than one instance of manager, so destroy newer instance!");
            Destroy(this.gameObject);
        }
        //
        Instance = this;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!Application.isEditor)
            return;
        //
        SetPress();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Application.isEditor)
            return;
        //
        SetPress();
    }

    private void SetPress()
    {
        Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        m_debugPositionLast = MousePos;
        //
        Collider2D MouseHit = Physics2D.OverlapCircle(MousePos, m_pressRadius, m_layerCheck);
        //
        if (MouseHit == null)
        {
            onNoHit?.Invoke();
            return;
        }
        //
        MouseHit.gameObject.SendMessage(m_messageSend, SendMessageOptions.DontRequireReceiver);
        Debug.LogFormat("[Press] {0}", MouseHit.gameObject.name);
    }

    private void OnDrawGizmos()
    {
        if (m_debugPositionLast.HasValue)
            Gizmos.DrawWireSphere(m_debugPositionLast.Value, m_pressRadius);
    }
}