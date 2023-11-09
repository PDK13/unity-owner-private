using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIDrag : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    private Canvas m_canvas;
    private Image m_image;
    private RectTransform m_rectTransform;

    public bool Lock = false;

    [SerializeField] private Vector2 m_Pivot = new Vector2(0.5f, 0.5f);
    private Vector2 m_AnchorMin = new Vector2(0.5f, 0.5f);
    private Vector2 m_AnchorMax = new Vector2(0.5f, 0.5f);

    [Serializable]
    private class DragEventSingle
    {
        [Space]
        public UnityEvent OnDragBegin;
        public UnityEvent OnDrag;
        public UnityEvent OnDragEnd;
    }

    [SerializeField] private DragEventSingle DragEvent;

    public bool Drag { private set; get; } = false;
    public bool Hold { private set; get; } = false;
    public bool Ready { private set; get; } = false;

    private void Start()
    {
        m_canvas = GetComponentInParent<Canvas>();
        m_image = GetComponentInParent<Image>();
        m_rectTransform = GetComponent<RectTransform>();
        //
        m_rectTransform.pivot = m_Pivot;
        m_rectTransform.anchorMin = m_AnchorMin;
        m_rectTransform.anchorMax = m_AnchorMax;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        SetEventPointerEnter();
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        SetEventPointerExit();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetEventPointerD();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        SetEventPointerU();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        SetEventOnBeginDrag();
    }

    public void OnDrag(PointerEventData eventData)
    {
        SetEventOnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SetEventOnEndDrag();
    }

    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log("OnDrop");
    }

    //Event

    private void SetEventPointerEnter()
    {
        if (Lock)
            return;
        //
        Ready = true;
    }

    private void SetEventPointerExit()
    {
        if (Lock)
            return;
        //
        Ready = false;
    }

    private void SetEventPointerD()
    {
        if (Lock)
            return;
        //
        Hold = true;
    }

    private void SetEventPointerU()
    {
        if (Lock)
            return;
        //
        Hold = false;
    }

    private void SetEventOnBeginDrag()
    {
        if (Lock)
            return;
        //
        Drag = true;
        m_image.raycastTarget = false;
        //
        DragEvent.OnDragBegin?.Invoke();
    }

    private void SetEventOnDrag(PointerEventData eventData)
    {
        if (Lock)
            return;
        //
        m_rectTransform.anchoredPosition += eventData.delta / m_canvas.scaleFactor;
        //
        DragEvent.OnDrag?.Invoke();
    }

    private void SetEventOnEndDrag()
    {
        if (Lock)
            return;
        //
        Drag = false;
        m_image.raycastTarget = true;
        //
        DragEvent.OnDragEnd?.Invoke();
    }
}