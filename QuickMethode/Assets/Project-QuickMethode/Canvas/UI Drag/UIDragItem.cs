using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIDragItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    //Primary

    public bool Lock = false;

    private Vector2 m_pivot = new Vector2(0.5f, 0.5f);
    private Vector2 m_anchorMin = new Vector2(0.5f, 0.5f);
    private Vector2 m_anchorMax = new Vector2(0.5f, 0.5f);

    public bool Drag { private set; get; } = false;

    public UIDragSlot Slot { get; private set; } = null;

    //Event

    public enum EventType { DragBegin, Drag, DragEnd, }

    private Action<EventType> onEvent;

    //

    private Canvas m_canvas;
    private Image m_image;
    private RectTransform m_rectTransform;

    private void Start()
    {
        m_canvas = GetComponentInParent<Canvas>();
        m_image = GetComponentInParent<Image>();
        m_rectTransform = GetComponent<RectTransform>();
        //
        m_rectTransform.pivot = m_pivot;
        m_rectTransform.anchorMin = m_anchorMin;
        m_rectTransform.anchorMax = m_anchorMax;
    }

    //

    public void SetSlot(UIDragSlot Slot)
    {
        this.Slot = Slot;
        //
        if (Slot.Fixed)
            m_rectTransform.anchoredPosition = Slot.GetComponent<RectTransform>().anchoredPosition;
    }

    public void SetEvent(Action<EventType> onEvent)
    {
        this.onEvent += onEvent;
    }

    //

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Lock)
            return;
        //
        Drag = true;
        m_image.raycastTarget = false;
        //
        if (Slot != null)
        {
            Slot.SetRemove(this);
            Slot = null;
        }
        //
        onEvent?.Invoke(EventType.DragBegin);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Lock)
            return;
        //
        m_rectTransform.anchoredPosition += eventData.delta / m_canvas.scaleFactor;
        //
        onEvent?.Invoke(EventType.Drag);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Lock)
            return;
        //
        Drag = false;
        m_image.raycastTarget = true;
        //
        onEvent?.Invoke(EventType.DragEnd);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;
        //
        UIDragItem Drag = eventData.pointerDrag.GetComponent<UIDragItem>();
        if (Drag == null)
            return;
        //
        if (!Slot.Add)
            return;
        //
        Drag.SetSlot(Slot);
        Slot.SetAdd(Drag);
    }
}