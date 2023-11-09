using UnityEngine;
using UnityEngine.EventSystems;

public class UIZoom : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [SerializeField] private float m_zoomSpeed = 0.01f;

    private float m_increment = 0;

#if UNITY_EDITOR

    public void Update()
    {
        SetZoom(Input.GetAxis("Mouse ScrollWheel") * 100f * m_zoomSpeed);
    }

#endif

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Input.touchCount == 2)
        {
            Touch Touch01 = Input.GetTouch(0);
            Touch Touch02 = Input.GetTouch(1);
            //
            Vector2 Touch01PrevPos = Touch01.position - Touch01.deltaPosition;
            Vector2 Touch02PrevPos = Touch02.position - Touch02.deltaPosition;
            //
            float PrevLength = (Touch01PrevPos - Touch02PrevPos).magnitude;
            float CurrentLength = (Touch01.position - Touch02.position).magnitude;
            //
            float Offset = CurrentLength - PrevLength;
            //
            SetZoom(Offset * m_zoomSpeed);
        }
    }

    private void SetZoom(float m_Increment)
    {
        this.m_increment = m_Increment;

        if (Camera.main.orthographic)
            Camera.main.orthographicSize = Camera.main.orthographicSize - m_Increment;
        else
            Camera.main.fieldOfView = Camera.main.fieldOfView - m_Increment;
    }

    public float GetIncrement()
    {
        return m_increment;
    }
}