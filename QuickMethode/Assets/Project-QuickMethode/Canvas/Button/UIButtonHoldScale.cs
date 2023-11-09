using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIButtonHoldScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform RectTransform;

    [Min(0)]
    [Tooltip("Duration delay before active hold event")]
    public float DelayHold = 0f;

    public bool Ready { private set; get; } = false;

    public bool Hold { private set; get; } = false;

    public bool HoldActive { private set; get; } = false;

    [Serializable]
    public class ScaleEventSingle
    {
        public Vector2 Normal = Vector2.one * 1.00f;
        public Vector2 Ready = Vector2.one * 1.25f;
        public Vector2 Hold = Vector2.one * 1.50f;
    }

    public ScaleEventSingle ScaleEvent;

    [Serializable]
    private class PointerEventSingle
    {
        [Space]
        public UnityEvent PointerHold;
        public UnityEvent PointerDown;
        public UnityEvent PointerUp;
        public UnityEvent PointerEnter;
        public UnityEvent PointerExit;
    }

    [SerializeField] private PointerEventSingle PointerEvent;

    private void Start()
    {
        if (RectTransform == null)
            RectTransform = GetComponent<RectTransform>();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
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
        SetEventPointerDown();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        SetEventPointerUp();
    }

    //Press

    public void SetButtonPress()
    {
        RectTransform.localScale = ScaleEvent.Ready;
        //
        SetEventPointerDown();
    }

    public void SetButtonRelease()
    {
        SetEventPointerUp();
    }

    //Event

    private void SetEventPointerEnter()
    {
        if (Application.platform == RuntimePlatform.Android)
            SetEventPointerDown();
        else
        {
            Ready = true;
            //
            if (Hold || HoldActive)
                RectTransform.localScale = ScaleEvent.Hold;
            else
                RectTransform.localScale = ScaleEvent.Ready;
            //
            PointerEvent.PointerEnter?.Invoke();
        }
    }

    private void SetEventPointerExit()
    {
        if (Application.platform == RuntimePlatform.Android)
            SetEventPointerUp();
        else
        {
            Ready = false;
            //
            if (Hold)
                RectTransform.localScale = HoldActive ? ScaleEvent.Hold : ScaleEvent.Ready;
            else
                RectTransform.localScale = ScaleEvent.Normal;
            //
            PointerEvent.PointerExit?.Invoke();
        }
    }

    private void SetEventPointerDown()
    {
        Hold = true;
        PointerEvent.PointerDown?.Invoke();
        //
        StartCoroutine(ISetButtonHold());
    }

    private void SetEventPointerUp()
    {
        Hold = false;
        HoldActive = false;
        //
        if (Hold)
            RectTransform.localScale = HoldActive ? ScaleEvent.Hold : ScaleEvent.Ready;
        else
        if (Ready)
            RectTransform.localScale = ScaleEvent.Ready;
        else
            RectTransform.localScale = ScaleEvent.Normal;
        //
        PointerEvent.PointerUp?.Invoke();
        //
        StopAllCoroutines();
    }

    private IEnumerator ISetButtonHold()
    {
        if (DelayHold > 0)
            yield return new WaitForSeconds(DelayHold);
        //
        HoldActive = true;
        RectTransform.localScale = ScaleEvent.Hold;
        //
        while (Hold)
        {
            PointerEvent.PointerHold?.Invoke();
            yield return null;
        }
        //
        HoldActive = false;
        //
    }
}