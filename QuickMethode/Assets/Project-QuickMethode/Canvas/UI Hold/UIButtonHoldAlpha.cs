using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIButtonHoldAlpha : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public bool Lock = false;
    public bool PhoneDevice = false;

    public bool PhoneLogic => PhoneDevice || Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;

    [Min(0)]
    [Tooltip("Duration delay before active hold event")]
    public float DelayHold = 0f;

    [Serializable]
    public class AlphaEventSingle
    {
        [Range(0f, 1f)] public float Normal = 1.00f;
        [Range(0f, 1f)] public float Ready = 0.50f;
        [Range(0f, 1f)] public float Hold = 0.25f;
    }

    public AlphaEventSingle AlphaEvent;

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

    public bool Ready { private set; get; } = false;

    public bool Hold { private set; get; } = false;

    public bool HoldActive { private set; get; } = false;

    [Space]
    public CanvasGroup CanvasGroup;

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
        if (CanvasGroup != null)
            CanvasGroup.alpha = AlphaEvent.Ready;
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
        if (Lock)
            return;
        //
        if (PhoneLogic)
        {
            if (!Application.isEditor)
                SetEventPointerDown();
        }
        else
        {
            Ready = true;
            //
            if (Hold || HoldActive)
            {
                if (CanvasGroup != null)
                    CanvasGroup.alpha = AlphaEvent.Hold;
            }
            else
            {
                if (CanvasGroup != null)
                    CanvasGroup.alpha = AlphaEvent.Ready;
            }
            //
            PointerEvent.PointerEnter?.Invoke();
        }
    }

    private void SetEventPointerExit()
    {
        if (Lock)
            return;
        //
        if (PhoneLogic)
        {
            if (!Application.isEditor)
                SetEventPointerUp();
        }
        else
        {
            Ready = false;
            //
            if (Hold)
            {
                if (CanvasGroup != null)
                    CanvasGroup.alpha = HoldActive ? AlphaEvent.Hold : AlphaEvent.Ready;
            }
            else
            {
                if (CanvasGroup != null)
                    CanvasGroup.alpha = AlphaEvent.Normal;
            }
            //
            PointerEvent.PointerExit?.Invoke();
        }
    }

    private void SetEventPointerDown()
    {
        if (Lock)
            return;
        //
        Hold = true;
        PointerEvent.PointerDown?.Invoke();
        //
        StartCoroutine(ISetButtonHold());
    }

    private void SetEventPointerUp()
    {
        if (Lock)
            return;
        //
        Hold = false;
        HoldActive = false;
        //
        if (Hold)
        {
            if (CanvasGroup != null)
                CanvasGroup.alpha = HoldActive ? AlphaEvent.Hold : AlphaEvent.Ready;
        }
        else
        if (Ready)
        {
            if (CanvasGroup != null)
                CanvasGroup.alpha = AlphaEvent.Ready;
        }
        else
        {
            if (CanvasGroup != null)
                CanvasGroup.alpha = AlphaEvent.Normal;
        }
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
        if (CanvasGroup != null)
            CanvasGroup.alpha = AlphaEvent.Hold;
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