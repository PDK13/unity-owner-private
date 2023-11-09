using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class UIButtonHoldColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Image Target;

    [Min(0)]
    [Tooltip("Duration delay before active hold event")]
    public float DelayHold = 0f;

    public bool Ready { private set; get; } = false;

    public bool Hold { private set; get; } = false;

    public bool HoldActive { private set; get; } = false;

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

    private void Start()
    {
        if (Target == null)
            Target = GetComponent<Image>();
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
        Target.color = ColorEvent.Ready;
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
                Target.color = ColorEvent.Hold;
            else
                Target.color = ColorEvent.Ready;
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
                Target.color = HoldActive ? ColorEvent.Hold : ColorEvent.Ready;
            else
                Target.color = ColorEvent.Normal;
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
            Target.color = HoldActive ? ColorEvent.Hold : ColorEvent.Ready;
        else
        if (Ready)
            Target.color = ColorEvent.Ready;
        else
            Target.color = ColorEvent.Normal;
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
        Target.color = ColorEvent.Hold;
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