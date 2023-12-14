using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIDragSlot : MonoBehaviour, IDropHandler
{
    //Primary

    public bool Lock = false;
    public bool Fixed = true;
    public bool Once = true;

    private Vector2 m_pivot = new Vector2(0.5f, 0.5f);
    private Vector2 m_anchorMin = new Vector2(0.5f, 0.5f);
    private Vector2 m_anchorMax = new Vector2(0.5f, 0.5f);

    //Hold

    private List<UIDragItem> Items = new List<UIDragItem>();

    public bool Emty => Items == null ? true : Items.Count == 0;

    public bool Add => !Once || Emty;

    //Event

    public enum EventType { AddItem, RemoveItem, }

    private Action<EventType, UIDragItem> onEvent;

    //

    private RectTransform m_rectTransform;

    private void Start()
    {
        m_rectTransform = GetComponent<RectTransform>();
        //
        m_rectTransform.pivot = m_pivot;
        m_rectTransform.anchorMin = m_anchorMin;
        m_rectTransform.anchorMax = m_anchorMax;
    }

    //

    public void SetAdd(UIDragItem Item)
    {
        if (Items.Contains(Item))
            return;
        //
        Items.Add(Item);
        onEvent?.Invoke(EventType.AddItem, Item);
    }

    public void SetRemove(UIDragItem Item)
    {
        if (!Items.Contains(Item))
            return;
        //
        Items.Remove(Item);
        onEvent?.Invoke(EventType.RemoveItem, Item);
    }

    public void SetEvent(Action<EventType, UIDragItem> onEvent)
    {
        this.onEvent += onEvent;
    }

    //

    public void OnDrop(PointerEventData eventData)
    {
        if (Lock)
            return;
        //
        if (Once && Items.Count > 0)
            return;
        //
        if (eventData.pointerDrag == null)
            return;
        //
        UIDragItem Drag = eventData.pointerDrag.GetComponent<UIDragItem>();
        if (Drag == null)
            return;
        //
        Drag.SetSlot(this);
        this.SetAdd(Drag);
    }
}