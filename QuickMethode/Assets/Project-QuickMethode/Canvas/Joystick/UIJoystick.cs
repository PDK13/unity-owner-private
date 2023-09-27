using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private bool m_lockPos = true;
    [SerializeField] private bool m_autoReset = true;

    [Space]
    [SerializeField] private bool m_lockXL = false;
    [SerializeField] private bool m_lockXR = false;
    [SerializeField] private bool m_lockYU = false;
    [SerializeField] private bool m_lockYD = false;

    [Space]
    [SerializeField] private RectTransform m_joyStickLimit;

    private Vector2 m_joyStickLimitPosPrimary;

    [SerializeField] private RectTransform m_joyStickButton;

    private Canvas m_canvas;
    private Camera m_camera;

    private Vector2 m_valuePrimary;
    private Vector2 m_valueFixed;

    private bool m_touch = false;

    public Action onTouchOn;
    public Action onTouchOut;

    private void Start()
    {
        Vector2 center = new Vector2(0.5f, 0.5f);
        m_joyStickLimit.pivot = center;
        m_joyStickButton.anchorMin = center;
        m_joyStickButton.anchorMax = center;
        m_joyStickButton.pivot = center;
        m_joyStickButton.anchoredPosition = Vector2.zero;

        m_joyStickLimitPosPrimary = m_joyStickLimit.GetComponent<RectTransform>().anchoredPosition;

        if (m_canvas == null)
        {
            m_canvas = GetComponentInParent<Canvas>();

            if (m_canvas == null)
            {
                Debug.LogErrorFormat("{0}: This parent doesn't is Canvas.", name);
            }
        }
    }

    #region Input Value

    public Vector2 PrimaryValue => m_valuePrimary; //True Value when Drag!!

    public float PrimaryRadius => m_valuePrimary.magnitude; //Distance Value from Center!!

    public Vector2 FixedValue => m_valueFixed; //Fixed Value when Drag!!

    public float FixedRadius => m_valueFixed.magnitude; //Distance Value from Center!!

    public float Deg => QCircle.GetDeg360(Vector2.zero, m_valueFixed); //Deg Value from X-Axis Right!!

    public bool Touch => m_touch;

    #endregion

    #region Event Handler

    public void OnPointerDown(PointerEventData eventData)
    {
        onTouchOn?.Invoke();

        m_touch = true;

        if (m_canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            m_camera = m_canvas.worldCamera;
        }

        if (!m_lockPos)
        {
            Vector2 m_JoyStickLimit_Pos = RectTransformUtility.WorldToScreenPoint(m_camera, this.m_joyStickLimit.position);
            Vector2 m_JoyStickLimit = (eventData.position - m_JoyStickLimit_Pos) / m_canvas.scaleFactor;

            this.m_joyStickLimit.anchoredPosition = this.m_joyStickLimit.anchoredPosition + m_JoyStickLimit;
        }

        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 m_JoyStickLimit_Pos = RectTransformUtility.WorldToScreenPoint(m_camera, m_joyStickLimit.position);
        Vector2 m_JoyStickLimitRadius = m_joyStickLimit.sizeDelta / 2;
        //
        m_valuePrimary = (eventData.position - m_JoyStickLimit_Pos) / (m_JoyStickLimitRadius * m_canvas.scaleFactor);
        //
        if (m_lockXL && m_valuePrimary.x < 0)
            m_valuePrimary.x = 0;
        else
        if (m_lockXR && m_valuePrimary.x > 0)
            m_valuePrimary.x = 0;
        //
        if (m_lockYU && m_valuePrimary.y > 0)
            m_valuePrimary.y = 0;
        else
        if (m_lockYD && m_valuePrimary.y < 0)
            m_valuePrimary.y = 0;
        //
        m_valueFixed = m_valuePrimary;
        //
        if (m_valueFixed.magnitude > 0)
        {
            if (m_valueFixed.magnitude > 1)
                m_valueFixed = m_valueFixed.normalized;
        }
        else
            m_valueFixed = Vector2.zero;
        //
        m_joyStickButton.anchoredPosition = m_valueFixed * m_JoyStickLimitRadius * 1;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        onTouchOut?.Invoke();
        //
        m_touch = false;
        //
        m_valuePrimary = Vector2.zero;
        m_valueFixed = Vector2.zero;
        m_joyStickButton.anchoredPosition = Vector2.zero;
        //
        if (m_autoReset)
            m_joyStickLimit.anchoredPosition = m_joyStickLimitPosPrimary;
    }

    #endregion

    #region Lock JoyStick

    public void SetLockXL(bool LockXL)
    {
        this.m_lockXL = LockXL;
    }

    public void SetLockXR(bool LockXR)
    {
        this.m_lockXR = LockXR;
    }

    public void SetLockYU(bool LockYU)
    {
        this.m_lockYU = LockYU;
    }

    public void SetLockYD(bool LockYD)
    {
        this.m_lockYD = LockYD;
    }

    public bool GetLockXL()
    {
        return m_lockXL;
    }

    public bool GetLockXR()
    {
        return m_lockXR;
    }

    public bool GetLockYU()
    {
        return m_lockYU;
    }

    public bool GetLockYD()
    {
        return m_lockYD;
    }

    #endregion
}