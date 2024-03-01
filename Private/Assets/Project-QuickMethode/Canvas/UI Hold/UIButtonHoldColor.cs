using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonHoldColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public bool Lock = false;
    public bool PhoneDevice = false;

    public bool PhoneLogic => PhoneDevice || Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;

    [Min(0)]
    [Tooltip("Duration delay before active hold event")]
    public float DelayHold = 0f;

    [Serializable]
    public class ColorEventSingle
    {
        public Color Normal = Color.white;
        public Color Ready = Color.gray;
        public Color Hold = Color.yellow;
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

    public bool Ready { private set; get; } = false;

    public bool Hold { private set; get; } = false;

    public bool HoldActive { private set; get; } = false;

    [Space]
    public Image Image;

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
        if (Image != null)
            Image.color = ColorEvent.Ready;
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
                if (Image != null)
                    Image.color = ColorEvent.Hold;
            }
            else
            {
                if (Image != null)
                    Image.color = ColorEvent.Ready;
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
                if (Image != null)
                    Image.color = HoldActive ? ColorEvent.Hold : ColorEvent.Ready;
            }
            else
            {
                if (Image != null)
                    Image.color = ColorEvent.Normal;
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
            if (Image != null)
                Image.color = HoldActive ? ColorEvent.Hold : ColorEvent.Ready;
        }
        else
        if (Ready)
        {
            if (Image != null)
                Image.color = ColorEvent.Ready;
        }
        else
        {
            if (Image != null)
                Image.color = ColorEvent.Normal;
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
        if (Image != null)
            Image.color = ColorEvent.Hold;
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