using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIDragSlot : MonoBehaviour, IDropHandler
{
    private RectTransform m_rectTransform;
    private RectTransform m_rectTransformObjectHold;

    public bool Lock = false;

    [Serializable]
    private class DropEventSingle
    {
        [Space]
        public UnityEvent OnDrop;
    }

    [SerializeField] private DropEventSingle DropEvent;

    public bool Hold => m_rectTransformObjectHold == null;
    public RectTransform ObjectHold => m_rectTransformObjectHold;

    private void Start()
    {
        m_rectTransform = GetComponent<RectTransform>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        SetEventDrop(eventData);
    }

    //Event

    private void SetEventDrop(PointerEventData eventData)
    {
        if (Lock)
            return;
        //
        if (eventData.pointerDrag != null)
        {
            m_rectTransformObjectHold = eventData.pointerDrag.GetComponent<RectTransform>();
            m_rectTransformObjectHold.anchoredPosition = m_rectTransform.anchoredPosition;
        }
        //
        DropEvent.OnDrop?.Invoke();
    }
}