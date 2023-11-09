using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonHoldAlpha : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public CanvasGroup Target;

    [Min(0)]
    [Tooltip("Duration delay before active hold event")]
    public float DelayHold = 0f;

    public bool Ready { private set; get; } = false;

    public bool Hold { private set; get; } = false;

    public bool HoldActive { private set; get; } = false;

    [Serializable]
    public class ColorEventSingle
    {
        public float Normal = 1.00f;
        public float Ready = 0.50f;
        public float Hold = 0.25f;
    }

    public ColorEventSingle ColorEvent;

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
        if (Target == null)
            Target = GetComponent<CanvasGroup>();
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
        Target.alpha = ColorEvent.Ready;
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
                Target.alpha = ColorEvent.Hold;
            else
                Target.alpha = ColorEvent.Ready;
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
                Target.alpha = HoldActive ? ColorEvent.Hold : ColorEvent.Ready;
            else
                Target.alpha = ColorEvent.Normal;
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
            Target.alpha = HoldActive ? ColorEvent.Hold : ColorEvent.Ready;
        else
        if (Ready)
            Target.alpha = ColorEvent.Ready;
        else
            Target.alpha = ColorEvent.Normal;
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
        Target.alpha = ColorEvent.Hold;
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